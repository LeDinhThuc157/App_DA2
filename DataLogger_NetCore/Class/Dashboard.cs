//using MongoDB.Bson.Serialization.Attributes;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace DataLogger_NetCore.Class
//{
//    public class DashBoard
//    {
//        [BsonId]
//        public string username { get; set; }
//        public DateTime time { get; set; }
//        public int Sum_Device_Off { get; set; }
//        public int Sum_Device_On { get; set; }
//        public Dictionary<string, int> Sum_Device_Area { get; set; }
//        public Dictionary<string, int> Sum_Device_Type { get; set; }
//        public void GetDashboard(string user_name, User user)
//        {
//            Dictionary<string, int> res = new Dictionary<string, int>();
//            Dictionary<string, int> countDeviceByArea = new Dictionary<string, int>();
//            Dictionary<string, int> countDeviceByType = new Dictionary<string, int>();

//            int coutOnlineDeivce = 0;
//            int countOfflineDevice = 0;
//            string key;
//            string keytype;
//            foreach (var device in Program.Devices)
//            {
//                if (user.listDevices.Contains(device.Value.Device_id))
//                {
//                    if (device.Value.Status)
//                    {
//                        coutOnlineDeivce++;
//                    }
//                    else
//                    {
//                        countOfflineDevice++;
//                    }
//                    if (device.Value.area == "")
//                    {
//                        key = "Other";
//                    }
//                    else
//                    {
//                        key = device.Value.area;
//                    }
//                    if (countDeviceByArea.ContainsKey(key))
//                    {
//                        countDeviceByArea[key] += 1;
//                    }
//                    else
//                    {
//                        countDeviceByArea.Add(key, 1);
//                    }
//                    keytype = device.Value.type.ToString();
//                    if (countDeviceByType.ContainsKey(keytype))
//                    {
//                        countDeviceByType[keytype] += 1;
//                    }
//                    else
//                    {
//                        countDeviceByType.Add(keytype, 1);
//                    }
//                }
                
//            }
//            username = user_name;
//            time = Program.getTime();
//            Sum_Device_On = coutOnlineDeivce;
//            Sum_Device_Off = countOfflineDevice;
//            Sum_Device_Area = countDeviceByArea;
//            Sum_Device_Type = countDeviceByType;
//        }

//    }
//}
