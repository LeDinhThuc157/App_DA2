using DataLogger_NetCore.Class;
using DataLogger_NetCore.MQTT;
using Microsoft.AspNetCore.Hosting;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataLogger_NetCore
{
    class Program
    {
        public static Device Device { get; set; }
        public static IMongoCollection<Device> Device_Collection;
        public static IMongoCollection<DeviceData> Aithing_Collection;

        //public static IMongoCollection<emailinfo> Email_Collection;

        // Save tokenkey in dictionary --> username                                                                                     

        public static string Admin_username;// = "admin@aquasoft";
        public static string Admin_password;// = "admin";
        public static int count = 0;
        public static int[] APIport;
        public static int check_time = 1000;
        public static IWebHost _webApp;
        // user log file
        // public static Dictionary<string, Dictionary<double, List<string>>> event_logs = new Dictionary<string, Dictionary<double, List<string>>>();
        //public static object id_user_log_lock = new object();
        public static string version = ""; // fix user log save file
        public static string MongoConnection;
        public static string Database;
        public static string hostname = "";
        public static string startupPath = Directory.GetCurrentDirectory();

        public static IMongoDatabase datalogger;
        public static HandleMQTT mqttHandle;

        //static void Main()
        //{
        //    Console.WriteLine(Program.getTime().ToString());
        //}
        static void Main()
        {
            //GetAquaboxDataAndPushToCollector();

            // Console.Clear();
            readConfig();
            saveLog("version:\t" + version);

            double timenow = Program.getTime().ToOADate();

            saveLog("MongoDB:\t" + MongoConnection);
            Console.WriteLine("> MongoDB:\t" + MongoConnection);
            MongoClient dbClient = new MongoClient(MongoConnection);
            datalogger = dbClient.GetDatabase(Database);

            saveLog("Database:\t" + Database);
            Device_Collection = datalogger.GetCollection<Device>("Device");

            Device = Device_Collection.Find(new BsonDocument()).ToList().FirstOrDefault();
            if (Device == null)
            {
                Device = new Device("aithing", false, "aithing", -1, -1, new Dictionary<string, double>(), -1);
                Device_Collection.InsertOne(Device);
            }
            if (APIport != null)
            {
                for (int i = 0; i < APIport.Length; i++)
                {
                    // hostname = (hostname == "") ? "*": hostname;
                    string baseAddress = "http://" + hostname + ":" + APIport[i].ToString() + "/";
                    _webApp = new WebHostBuilder()
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseKestrel()
                        .UseStartup<Startup>()
                        .UseUrls(baseAddress)
                        .Build();

                    _webApp.Start();
                    saveLog("API port:\t" + APIport[i]);
                    saveLog("Hostname:\t" + hostname);
                }
            }
            //DeviceTemplate.loadTemplate();
            mqttHandle = new HandleMQTT();
            mqttHandle.subscribe();

            while (true)
            {

                try
                {
                    checkStatus();
                    Thread.Sleep(10 * check_time);
                    if (!mqttHandle.mqttClient.IsConnected)
                    {
                        mqttHandle.subscribe();
                    }

                    //Console.WriteLine("Thoi gian cap nhat!!!" + Program.getTime());
                }
                catch (Exception ex)
                {
                    saveLog("Check Status: -> " + ex.Message);
                }
            }
        }
        public static void readConfig()
        {
            try
            {
                string filepath = startupPath + @"/";
                if (File.Exists(@"" + filepath + "config.txt"))
                {
                    var s = File.ReadAllLines(filepath + "config.txt");
                    foreach (var str in s)
                    {
                        if (str.Length > 0)
                        {
                            string[] tmp = str.Split("\t");
                            switch (tmp[0])
                            {
                                case "version:":
                                    string version = tmp[1];
                                    Program.version = version;
                                    break;
                                case "API port:":
                                    //int[] APIport = new int[tmp.Length - 1];
                                    List<int> APIport = new List<int>();
                                    for (int i = 0; i < tmp.Length - 1; i++)
                                    {
                                        if (tmp[i + 1] != "")
                                            //APIport[i] = int.Parse(tmp[i + 1]);
                                            APIport.Add(int.Parse(tmp[i + 1]));
                                    }
                                    Program.APIport = APIport.ToArray();
                                    break;
                                case "Admin account:":
                                    if (tmp[1] != "" && tmp[2] != "")
                                    {
                                        string admin_user = tmp[1];
                                        string admin_pass = tmp[2];
                                        Program.Admin_username = admin_user;
                                        Program.Admin_password = admin_pass;
                                    }
                                    break;
                                case "Check status(s):":
                                    if (tmp[1] != "")
                                    {
                                        int check_time = int.Parse(tmp[1]);
                                        Program.check_time = check_time;
                                    }
                                    break;
                                case "MongoDB Connection:":
                                    if (tmp[1] != "")
                                    {
                                        string MongoConnection = tmp[1];
                                        Program.MongoConnection = MongoConnection;
                                    }
                                    break;
                                case "Project:":
                                    HandleMQTT.projectList = tmp[1].Split(',').ToList();
                                    break;
                                case "Database:":
                                    if (tmp[1] != "")
                                    {
                                        string Database = tmp[1];
                                        Program.Database = Database;
                                    }
                                    break;
                                case "Hostname:":
                                    if (tmp[1] != "")
                                    {
                                        string hostname = tmp[1];
                                        Program.hostname = hostname;
                                    }
                                    break;
                                case "MQTTUrl:":
                                    if (tmp[1] != "")
                                        HandleMQTT.url = tmp[1];
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    Program.saveLog("config file dont exist!");
                    Console.WriteLine("config file dont exist!");
                }
            }
            catch (Exception ex)
            {
                Program.saveLog(ex.ToString());
            }
        }

        public static void checkStatus()
        {
            if (Program.Device.Status)// true state
            {
                if (Program.getTime().ToOADate() - Program.Device.lastTimeSystem > 0.007) //disconnected = 10 mins = 0.007
                {
                    Program.Device.Status = false;
                }
            }
        }

        public static void saveLog(string log)
        {
            try
            {
                File.AppendAllText("log.txt", "[" + Program.getTime().ToString("dd/MM/yyyy HH:mm:ss") + "]: " + log + "\r\n");
            }
            catch (Exception)
            {
                //Console.WriteLine("savelog ex : " + ex.Message);
            }
        }

        public static void actionLog(string log)
        {
            try
            {
                File.AppendAllText("actionlog.txt", "[" + Program.getTime().ToString("dd/MM/yyyy HH:mm:ss") + "]: " + log + "\r\n");
            }
            catch (Exception)
            {
                //Console.WriteLine("actionLog ex : " + ex.Message);
            }
        }

        public static void Device_replace(Device dv)
        {
            var filter = Builders<Device>.Filter.Eq("Device_id", dv.Device_id);
            Device_Collection.ReplaceOne(filter, dv);
        }

        public static void Device_delete(string device_id)
        {
            var filter = Builders<Device>.Filter.Eq("Device_id", device_id);
            Device_Collection.DeleteOne(filter);
        }

        public static void WriteLine(string content)
        {
            Console.WriteLine("[" + Program.getTime().ToString("yyyy-MM-dd HH:mm:ss") + "]\t" + content);
        }

        public static DateTime getTime()
        {
            // Lấy múi giờ của Việt Nam
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

            // Lấy thời gian hiện tại
            DateTime nowUtc = DateTime.UtcNow;

            // Chuyển đổi thời gian hiện tại sang múi giờ của Việt Nam
            DateTime nowVietnam = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, vietnamTimeZone);
            return nowVietnam;
        }
    }


}
