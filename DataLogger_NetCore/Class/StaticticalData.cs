
//﻿using Amazon.Runtime.Internal.Transform;
//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;
//using MongoDB.Driver;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using MongoDB.Driver;

//namespace DataLogger_NetCore.Class
//{
//    public class StaticticalData
//    {
//        [BsonId]
//        public string device_id;
        
//        public Dictionary<string, Data> data { get; set; }
//       public void InsertDay(DateData datedata, Dictionary<string, string> data)
//        {
//            Dictionary<string, Dictionary<string, string>> list_data_min;
//            Dictionary<string, Dictionary<string, string>> list_data_max;
//            var statictical_docmument = Program.Statictical_Collection.Find(new BsonDocument()).ToList();

//            StaticticalData statictical_mongo = new StaticticalData();
//            StaticticalData statics = new StaticticalData();
//            Data statictical_data = new Data();
//            double oadate = datedata.datetime.ToOADate();
//            string date_now = datedata.datetime.ToString("dd/MM/yyyy");
//            // Kiểm tra xem trong Collection đã tồn tại device id chưa
//            if (statictical_docmument.FirstOrDefault(x => x.device_id == "d_"+device_id) == null)
//            {
//                statictical_mongo.device_id = "d_" + device_id;
//                statics.device_id = "s_" + device_id;
//                statictical_mongo.data = new Dictionary<string, Data>();
//                statictical_data.min = new Dictionary<string, Dictionary<string, string>>();
//                statictical_data.max = new Dictionary<string, Dictionary<string, string>>();
//                statics.data = new Dictionary<string, Data>();
//                foreach (var key in data.Keys)
//                {
//                    Dictionary<string, string> list_data = new Dictionary<string, string>();
//                    list_data.Add(oadate.ToString(), data[key]);
//                    statictical_data.min[key] = list_data;
//                    statictical_data.max[key] = list_data;

//                }
//                statictical_mongo.data.Add(date_now, statictical_data);
//                Program.Statictical_Collection.InsertOne(statictical_mongo);
//                if (statictical_docmument.FirstOrDefault(x => x.device_id == "s_"+device_id) == null){
//                    statics.device_id = "s_" + device_id;
//                    statics.data.Add("week", statictical_data);
//                    Program.Statictical_Collection.InsertOne(statics);
//                }
//            }
//            else
//            {
//                statictical_mongo = statictical_docmument.FirstOrDefault(x => x.device_id == "d_" + device_id);
//                statics = statictical_docmument.FirstOrDefault(x => x.device_id == "s_" + device_id);

//                // Vấn đề xuất hiện ở đây! Khi thêm full ngày mới thì nó sẽ thêm tiếp vào tuần mới!
//                if (!statictical_mongo.data.ContainsKey(date_now))
//                {
//                    if(statictical_mongo.data == null){
//                        statictical_mongo.data = new Dictionary<string, Data>();
//                    }
//                    statictical_data.min = new Dictionary<string, Dictionary<string, string>>();
//                    statictical_data.max = new Dictionary<string, Dictionary<string, string>>();
//                    foreach (var key in data.Keys)
//                    {
//                        Dictionary<string, string> list_data = new Dictionary<string, string>();
//                        list_data.Add(oadate.ToString(), data[key]);
//                        statictical_data.min[key] = list_data;
//                        statictical_data.max[key] = list_data;
//                        foreach(var key_time in statics.data["week"].min[key]){
//                            if(datedata.datetime.DayOfYear - DateTime.FromOADate(double.Parse(key_time.Key.ToString())).DayOfYear  >= 7 ){
//                                statics.data["week"].min[key.ToString()].Remove(key_time.Key.ToString());
//                                statics.data["week"].max[key.ToString()].Remove(key_time.Key.ToString());

//                            }
//                        }
//                        statics.data["week"].min[key].Add(oadate.ToString(), data[key]);
//                        statics.data["week"].max[key].Add(oadate.ToString(), data[key]);

//                    }
//                    statictical_mongo.data.Add(date_now, statictical_data);
//                }
//                else
//                {
//                    statictical_data = statictical_mongo.data[date_now];
//                    list_data_min = statictical_data.min;
//                    list_data_max = statictical_data.max;

//                    foreach (var key in data.Keys)
//                    {
//                        if(statics.data["week"].min.ContainsKey(key)){
//                            foreach(var key_time in statics.data["week"].min[key]){
//                                if(datedata.datetime.DayOfYear - DateTime.FromOADate(double.Parse(key_time.Key.ToString())).DayOfYear  >= 7 ){
//                                    statics.data["week"].min[key.ToString()].Remove(key_time.Key.ToString());
//                                    statics.data["week"].max[key.ToString()].Remove(key_time.Key.ToString());

//                                }
//                            }
//                            if (list_data_min[key].Count < 5)
//                            {
//                                list_data_min[key].Add(oadate.ToString(), data[key]);
//                                statics.data["week"].min[key].Add(oadate.ToString(), data[key]);

//                            }
//                            else
//                            {
//                                double maxV = double.MinValue;
//                                string k = "";
//                                foreach (var i in list_data_min[key].Keys)
//                                {
//                                    if (double.Parse(list_data_min[key][i]) > maxV)
//                                    {
//                                        maxV = double.Parse(list_data_min[key][i]);
//                                        k = i;
//                                    }
//                                }

//                                if (maxV > double.Parse(data[key]))
//                                {
//                                    list_data_min[key].Remove(k);
//                                    list_data_min[key].Add(oadate.ToString(), data[key]);
//                                    statics.data["week"].min[key].Remove(k);
//                                    statics.data["week"].min[key].Add(oadate.ToString(), data[key]);
//                                }
//                            }
//                            if (list_data_max[key].Count < 5)
//                            {
//                                list_data_max[key].Add(oadate.ToString(), data[key]);
//                                statics.data["week"].max[key].Add(oadate.ToString(), data[key]);

//                            }
//                            else
//                            {
//                                double minV = double.MaxValue;
//                                string k = "";
//                                foreach (var i in list_data_max[key].Keys)
//                                {
//                                    if (double.Parse(list_data_max[key][i]) < minV)
//                                    {
//                                        minV = double.Parse(list_data_max[key][i]);
//                                        k = i;
//                                    }
//                                }

//                                if (minV < double.Parse(data[key]))
//                                {
//                                    list_data_max[key].Remove(k);
//                                    list_data_max[key].Add(oadate.ToString(), data[key]);
//                                    statics.data["week"].max[key].Remove(k);
//                                    statics.data["week"].max[key].Add(oadate.ToString(), data[key]);

//                                }
//                            }
//                        }else{
//                                list_data_min.Add(key,new Dictionary<string, string>());
//                                list_data_min[key].Add(oadate.ToString(), data[key]);
//                                list_data_max.Add(key,new Dictionary<string, string>());
//                                list_data_max[key].Add(oadate.ToString(), data[key]);
//                                statics.data["week"].min.Add(key,new Dictionary<string, string>());
//                                statics.data["week"].max.Add(key,new Dictionary<string, string>());
//                                statics.data["week"].min[key].Add(oadate.ToString(), data[key]);
//                                statics.data["week"].max[key].Add(oadate.ToString(), data[key]);


//                        }               
//                    }
//                    statictical_data.max = list_data_max;
//                    statictical_data.min = list_data_min;
//                    statictical_mongo.data[date_now] = statictical_data;
                    
//                }
               
//                var filter = Builders<StaticticalData>.Filter.Eq("device_id", statictical_mongo.device_id);
//                Program.Statictical_Collection.ReplaceOne(filter, statictical_mongo);
//                var filter_s = Builders<StaticticalData>.Filter.Eq("device_id", statics.device_id);
//                Program.Statictical_Collection.ReplaceOne(filter_s, statics);

//            }
//        }
//        public void InsertMonth(DateData datedata, Dictionary<string, string> data,DateTime timestamp)
//        {
//            Dictionary<string, Dictionary<string, string>> list_data_min;
//            Dictionary<string, Dictionary<string, string>> list_data_max;
//            var statictical_docmument = Program.Statictical_Collection.Find(new BsonDocument()).ToList();

//            StaticticalData statictical_mongo = new StaticticalData();
//            Data statictical_data = new Data();
//            Data statictical_data_s = new Data();

//            StaticticalData statics = statictical_docmument.FirstOrDefault(x => x.device_id == "s_" + device_id);

//            double oadate = datedata.datetime.ToOADate();
//            string day = datedata.datetime.ToString("dd");
//            string date_now = datedata.datetime.ToString("MM/yyyy");
//            // Kiểm tra xem trong Collection đã tồn tại device id chưa
//            if (statictical_docmument.FirstOrDefault(x => x.device_id == "m_" + device_id) == null)
//            {
//                statictical_mongo.device_id = "m_" + device_id;
//                statictical_mongo.data = new Dictionary<string, Data>();
//                statictical_data.min = new Dictionary<string, Dictionary<string, string>>();
//                statictical_data.max = new Dictionary<string, Dictionary<string, string>>();
//                statictical_data_s.min = new Dictionary<string, Dictionary<string, string>>();
//                statictical_data_s.max = new Dictionary<string, Dictionary<string, string>>();
                
//                foreach (var key in data.Keys)
//                {
//                    Dictionary<string, string> list_data = new Dictionary<string, string>();
//                    Dictionary<string, string> list_data_s = new Dictionary<string, string>();

//                    list_data.Add(day, data[key]);
//                    list_data_s.Add(oadate.ToString(),data[key]);
//                    statictical_data.min[key] = list_data;
//                    statictical_data.max[key] = list_data;
//                    statictical_data_s.min[key] = list_data_s;
//                    statictical_data_s.max[key] = list_data_s;
//                }
//                statictical_mongo.data.Add(date_now, statictical_data);
//                Program.Statictical_Collection.InsertOne(statictical_mongo);
//                statics.data.Add("month", statictical_data_s);
//                statics.data.Add("year", statictical_data_s);
//            }
//            else
//            {
//                statictical_mongo = statictical_docmument.FirstOrDefault(x => x.device_id == "m_" + device_id);
//                if (!statictical_mongo.data.ContainsKey(date_now))
//                {
//                    if(statictical_mongo.data == null){
//                        statictical_mongo.data = new Dictionary<string, Data>();
//                    }
//                    statictical_data.min = new Dictionary<string, Dictionary<string, string>>();
//                    statictical_data.max = new Dictionary<string, Dictionary<string, string>>();
//                    foreach (var key in data.Keys)
//                    {
//                        Dictionary<string, string> list_data = new Dictionary<string, string>();
//                        list_data.Add(day, data[key]);
//                        statictical_data.min[key] = list_data;
//                        statictical_data.max[key] = list_data;
//                        foreach(var key_time in statics.data["month"].min[key]){
//                            if(datedata.datetime.DayOfYear - DateTime.FromOADate(double.Parse(key_time.Key.ToString())).DayOfYear  >= 30 ){
//                                statics.data["month"].min[key.ToString()].Remove(key_time.Key.ToString());
//                                statics.data["month"].max[key.ToString()].Remove(key_time.Key.ToString());

//                            }
//                        }
//                        foreach(var key_time in statics.data["year"].min[key]){
//                            if(datedata.datetime.DayOfYear - DateTime.FromOADate(double.Parse(key_time.Key.ToString())).DayOfYear  >= 365 ){
//                                statics.data["year"].min[key.ToString()].Remove(key_time.Key.ToString());
//                                statics.data["year"].max[key.ToString()].Remove(key_time.Key.ToString());

//                            }
//                        }
//                        statics.data["month"].min[key].Add(oadate.ToString(), data[key]);
//                        statics.data["month"].max[key].Add(oadate.ToString(), data[key]);
//                        statics.data["year"].min[key].Add(oadate.ToString(), data[key]);
//                        statics.data["year"].max[key].Add(oadate.ToString(), data[key]);

//                    }
//                    statictical_mongo.data.Add(date_now, statictical_data);
//                }
//                else
//                {
//                    statictical_data = statictical_mongo.data[date_now];
//                    list_data_min = statictical_data.min;
//                    list_data_max = statictical_data.max;

//                    foreach (var key in data.Keys)
//                    {
//                        if(statics.data["month"].min.ContainsKey(key)){
//                            foreach(var key_time in statics.data["month"].min[key]){
//                                if(datedata.datetime.DayOfYear - DateTime.FromOADate(double.Parse(key_time.Key.ToString())).DayOfYear  >= 30 ){
//                                    statics.data["month"].min[key.ToString()].Remove(key_time.Key.ToString());
//                                    statics.data["month"].max[key.ToString()].Remove(key_time.Key.ToString());

//                                }
//                            }
//                            foreach(var key_time in statics.data["year"].min[key]){
//                                if(datedata.datetime.DayOfYear - DateTime.FromOADate(double.Parse(key_time.Key.ToString())).DayOfYear  >= 365 ){
//                                    statics.data["year"].min[key.ToString()].Remove(key_time.Key.ToString());
//                                    statics.data["year"].max[key.ToString()].Remove(key_time.Key.ToString());

//                                }
//                            }
//                            if(list_data_min[key].ContainsKey(day)){
//                                if (double.Parse(list_data_min[key][day]) > double.Parse(data[key]))
//                                {
//                                    // list_data_min[key].Remove(day);
//                                    // list_data_min[key].Add(day, data[key]);
//                                    list_data_min[key][day] = data[key];
//                                    foreach(var key_time in statics.data["month"].min[key]){
//                                        if(DateTime.FromOADate(double.Parse(key_time.Key)).Day == int.Parse(day)){
//                                            statics.data["month"].min[key].Remove(key_time.Key.ToString());
//                                            statics.data["month"].min[key].Add(oadate.ToString(), data[key]);
//                                            statics.data["year"].min[key].Remove(key_time.Key.ToString());;
//                                            statics.data["year"].min[key].Add(oadate.ToString(), data[key]);
//                                            break;
//                                        }
//                                    }
                                   
                
//                                }
//                                if (double.Parse(list_data_max[key][day]) < double.Parse(data[key]))
//                                {
//                                    // list_data_max[key].Remove(day);
//                                    // list_data_max[key].Add(day, data[key]);
//                                    list_data_max[key][day] = data[key];

//                                    // statics.data["month"].max[key].Clear();
//                                    // statics.data["month"].max[key].Add(oadate.ToString(), data[key]);
//                                    // statics.data["year"].max[key].Clear();
//                                    // statics.data["year"].max[key].Add(oadate.ToString(), data[key]);
//                                     foreach(var key_time in statics.data["month"].max[key]){
//                                        if(DateTime.FromOADate(double.Parse(key_time.Key)).Day == int.Parse(day)){      
//                                            statics.data["month"].max[key].Remove(key_time.Key.ToString());
//                                            statics.data["month"].max[key].Add(oadate.ToString(), data[key]);
//                                            statics.data["year"].max[key].Remove(key_time.Key.ToString());
//                                            statics.data["year"].max[key].Add(oadate.ToString(), data[key]);
//                                            break;
//                                        }
//                                    }
//                                }
//                            }
//                            else{
//                                list_data_min[key].Add(day, data[key]);
//                                list_data_max[key].Add(day, data[key]);
//                                statics.data["month"].min[key].Add(oadate.ToString(), data[key]);
//                                statics.data["year"].min[key].Add(oadate.ToString(), data[key]);
//                                statics.data["month"].max[key].Add(oadate.ToString(), data[key]);
//                                statics.data["year"].max[key].Add(oadate.ToString(), data[key]);

//                            }
//                        }
//                        else{
//                                list_data_min.Add(key,new Dictionary<string, string>());
//                                list_data_min[key].Add(day, data[key]);
//                                list_data_max.Add(key,new Dictionary<string, string>());
//                                list_data_max[key].Add(day, data[key]);
//                                statics.data["month"].min.Add(key,new Dictionary<string, string>());
//                                statics.data["month"].max.Add(key,new Dictionary<string, string>());
//                                statics.data["year"].min.Add(key,new Dictionary<string, string>());
//                                statics.data["year"].max.Add(key,new Dictionary<string, string>());
//                                statics.data["month"].min[key].Add(oadate.ToString(), data[key]);
//                                statics.data["month"].max[key].Add(oadate.ToString(), data[key]);
//                                statics.data["year"].min[key].Add(oadate.ToString(), data[key]);
//                                statics.data["year"].max[key].Add(oadate.ToString(), data[key]);
//                        }
                        
                    
//                    }
//                    statictical_data.max = list_data_max;
//                    statictical_data.min = list_data_min;
//                    statictical_mongo.data[date_now] = statictical_data;
                    
//                }

//                var filter = Builders<StaticticalData>.Filter.Eq("device_id", statictical_mongo.device_id);
//                Program.Statictical_Collection.ReplaceOne(filter, statictical_mongo);
//            }
//            var filter_s = Builders<StaticticalData>.Filter.Eq("device_id", statics.device_id);
//            Program.Statictical_Collection.ReplaceOne(filter_s, statics);
//        }
//        public void InsertYear(DateData datedata, Dictionary<string, string> data)
//        {
//            Dictionary<string, Dictionary<string, string>> list_data_min;
//            Dictionary<string, Dictionary<string, string>> list_data_max;
//            var statictical_docmument = Program.Statictical_Collection.Find(new BsonDocument()).ToList();

//            StaticticalData statictical_mongo = new StaticticalData();
//            Data statictical_data = new Data();
//            double oadate = datedata.datetime.ToOADate();
//            string date_now = datedata.datetime.ToString("yyyy");
//            // Kiểm tra xem trong Collection đã tồn tại device id chưa
//            if (statictical_docmument.FirstOrDefault(x => x.device_id == "y_" + device_id) == null)
//            {
//                statictical_mongo.device_id = "y_" + device_id;
//                statictical_mongo.data = new Dictionary<string, Data>();
//                statictical_data.min = new Dictionary<string, Dictionary<string, string>>();
//                statictical_data.max = new Dictionary<string, Dictionary<string, string>>();
//                foreach (var key in data.Keys)
//                {
//                    Dictionary<string, string> list_data = new Dictionary<string, string>();
//                    list_data.Add(oadate.ToString(), data[key]);
//                    statictical_data.min[key] = list_data;
//                    statictical_data.max[key] = list_data;

//                }
//                statictical_mongo.data.Add(date_now, statictical_data);
//                Program.Statictical_Collection.InsertOne(statictical_mongo);
//            }
//            else
//            {
//                statictical_mongo = statictical_docmument.FirstOrDefault(x => x.device_id == "y_" + device_id);
//                if (!statictical_mongo.data.ContainsKey(date_now))
//                {
//                    statictical_mongo.data = new Dictionary<string, Data>();
//                    statictical_data.min = new Dictionary<string, Dictionary<string, string>>();
//                    statictical_data.max = new Dictionary<string, Dictionary<string, string>>();
//                    foreach (var key in data.Keys)
//                    {
//                        Dictionary<string, string> list_data = new Dictionary<string, string>();
//                        list_data.Add(oadate.ToString(), data[key]);
//                        statictical_data.min[key] = list_data;
//                        statictical_data.max[key] = list_data;

//                    }
//                    statictical_mongo.data.Add(date_now, statictical_data);
//                }
//                else
//                {
//                    statictical_data = statictical_mongo.data[date_now];
//                    list_data_min = statictical_data.min;
//                    list_data_max = statictical_data.max;

//                    foreach (var key in list_data_min.Keys)
//                    {
//                        foreach (var i in list_data_min[key].Keys)
//                        {
//                            if (double.Parse(list_data_min[key][i]) > double.Parse(data[key]))
//                            {
//                                list_data_min[key].Remove(i);
//                                list_data_min[key].Add(oadate.ToString(), data[key]);
//                            }
//                        }
//                    }

//                    foreach (var key in list_data_max.Keys)
//                    {
//                        foreach (var i in list_data_max[key].Keys)
//                        {
//                            if (double.Parse(list_data_max[key][i]) < double.Parse(data[key]))
//                            {
//                                list_data_max[key].Remove(i);
//                                list_data_max[key].Add(oadate.ToString(), data[key]);
//                            }
//                        }
//                    }
//                    statictical_data.max = list_data_max;
//                    statictical_data.min = list_data_min;
//                    statictical_mongo.data[date_now] = statictical_data;
//                }

//                var filter = Builders<StaticticalData>.Filter.Eq("device_id", statictical_mongo.device_id);
//                Program.Statictical_Collection.ReplaceOne(filter, statictical_mongo);
//            }
//        }

    
        
//    }
//    public class Data
//    {
//        public Dictionary<string, Dictionary<string, string>> min { get; set; }
//        public Dictionary<string, Dictionary<string, string>> max { get; set; }
//    }

    
//}
