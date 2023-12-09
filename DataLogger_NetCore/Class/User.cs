using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataLogger_NetCore.Class
{
    [BsonIgnoreExtraElements]
    public class Device
    {
        [BsonId]
        public string Device_id { get; set; } //device_id
        public bool Status { get; set; }
        public string Device_name { get; set; }
        public double lastEdit { get; set; }
        public double lastReceived { get; set; }
        public double lastTimeSystem { get; set; }
        public Dictionary<string, double> lastData { get; set; }
        public string version_running { get; set; }
        public string hw_config { get; set; }
        public List<string> inputCode { get; set; }

        [JsonConstructor]
        public Device(string Device_id, bool Status, string Device_name, double lastEdit, double lastReceived, Dictionary<string, double> lastData, double lastTimeSystem)
        {
            this.Device_id = Device_id;
            this.Status = Status;
            this.Device_name = Device_name;
            this.lastReceived = lastReceived;
            this.lastEdit = lastEdit;
            this.lastData = lastData;
            this.lastTimeSystem = lastTimeSystem;
        }
    }
    public class Device_respond
    {
        public string Device_id { get; set; } //device_id
        public bool Status { get; set; }
        public string Device_name { get; set; }
        public double lastEdit { get; set; }
        public double lastReceived { get; set; }
        public double lastTimeSystem { get; set; }
        public Dictionary<string, double> lastData { get; set; }
        public string version_running { get; set; }

        public string hw_config { get; set; }
        public List<string> inputCode { get; set; }
    }
    public class HttpRequest
    {
        public string tokenkey;
        public string username;
        public string password;
        public string newpassword;
        public string newdevicename;
        public string permission;
        public int year;
        public int month;
        public int day;
        public int start_year;
        public int start_month;
        public int start_day;
        public int end_year;
        public int end_month;
        public int end_day;
        public string device_id;
        public string device_name;
        public double latitude;
        public double longitude;
        public string numInput;
        public List<string> listNumInput;
        public string area;
        public int numberPage;
        public string fromDate;
        public string toDate;
        public Byte configMode;
        public Byte configPumpA;
        public Byte configPumpB;
        // lossblock
        public List<string> list_device;
        public Byte value;
        public int device_type;
        public string wifissid;
        public string wifipass;
        public string type_time;
    }

}

