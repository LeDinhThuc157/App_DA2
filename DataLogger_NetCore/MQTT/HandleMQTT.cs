using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DataLogger_NetCore.Class;
using MQTTnet.Client;
using MQTTnet;
using System.Threading.Tasks;
using System.Threading;

namespace DataLogger_NetCore.MQTT
{
    public class HandleMQTT
    {

        public IMqttClient mqttClient;
        public bool connected = false;
        public static List<string> projectList = new List<string>();
        public static string url = "white-dev.aithings.vn:1883";
        public HandleMQTT()
        {
        }

        Task handleMsg(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                //Console.WriteLine(e.ApplicationMessage.Topic + "\t" + e.ApplicationMessage.ConvertPayloadToString());
                string id = e.ApplicationMessage.Topic.Split('/')[1];
                updateData(id, e.ApplicationMessage.ConvertPayloadToString());
            }
            catch (Exception ex)
            {
                Program.saveLog(ex.ToString());
            }
            return Task.CompletedTask;
        }

        Task handleDisconnected(MqttClientDisconnectedEventArgs e)
        {
            connected = false;
            return Task.CompletedTask;
        }

        public async void subscribe()
        {
            try
            {
                var mqttFactory = new MqttFactory();

                mqttClient = mqttFactory.CreateMqttClient();

                var mqtturl = url.Split(':');
                var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(mqtturl[0], int.Parse(mqtturl[1])).Build();

                // Setup message handling before connecting so that queued messages
                // are also handled properly. When there is no event handler attached all
                // received messages get lost.
                mqttClient.ApplicationMessageReceivedAsync += handleMsg;

                await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

                if (projectList.Count == 0) projectList.Add("doan2");
                foreach (var item in projectList)
                {
                    var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
                    .WithTopicFilter(
                        f =>
                        {
                            f.WithTopic(item + "/+/data");
                        })
                    .Build();

                    await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
                    Console.WriteLine("MQTT subscription ok: " + item);
                }
            }
            catch (Exception)
            { }
        }

        void updateData(string id, string mqttData)
        {
            var dataString = mqttData.Split('\t');
            string timestamp = dataString[0];
            string device_id = id.ToString();
            Program.Device.lastTimeSystem = Program.getTime().ToOADate();
            Program.Device.Status = true;
            DateTime temp;
            DateTime.TryParseExact(timestamp, "yyyyMMddHHmmss", CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out temp);
            if (temp.Year < 2023) return;

            Dictionary<string, string> codedata = new Dictionary<string, string>();
            Dictionary<string, double> numdata = new Dictionary<string, double>();
            for (int i = 1; i < dataString.Length; i++)
            {
                var str = dataString[i];
                var s = str.Split(':');
                if (s.Length == 2)
                {
                    codedata[s[0]] = s[1];
                    double tmpval;
                    if (double.TryParse(s[1], out tmpval))
                        numdata[s[0]] = tmpval;
                }
            }

            DeviceData.getDevice(id).insertData(temp, codedata);
            double receivedtime = temp.ToOADate();

            if (Program.Device.Device_id == "aithing")
            {
                var device = Program.Device;

                //if (numdata.ContainsKey("TYPE") && numdata["TYPE"] != device.type)
                //    Program.mqttHandle.sendCommand(device.Device_id, "c:type:" + device.type); //resync device type if necessary

                device.lastReceived = receivedtime;
                device.lastTimeSystem = Program.getTime().ToOADate();
                if (device.Status == false)
                    device.Status = true;

                if (device.inputCode == null)
                {
                    device.inputCode = new List<string>();
                }
                foreach (string code in codedata.Keys)
                {
                    if (!device.inputCode.Contains(code))
                    {
                        device.inputCode.Add(code);
                    }
                }
                device.lastData = numdata;
                Program.Device_replace(Program.Device);
            }
            else
            {
                Program.Device = new Device(device_id, true, device_id, -1, receivedtime, numdata, Program.getTime().ToOADate());//VSI location
                Program.Device.version_running = "0";
                Program.Device.inputCode = codedata.Keys.ToList();
                Program.Device_Collection.InsertOne(Program.Device);
            }

            //OneSignalService.createNotification(Program.Devices[device_id], mqttData);
        }

        public bool sendCommand(string device_id, string command)
        {
            return mqttClient.PublishStringAsync("white/" + device_id + "/control", command).Result.IsSuccess;
        }
    }

}
