using DataLogger_NetCore.Class;
using Microsoft.AspNetCore.Cors;
using Nancy;
using Nancy.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;
using MongoDB.Driver;


namespace DataLogger_NetCore.API
{
    [EnableCors("AllowAnyOrigin")]
    public class DataModule : NancyModule
    {
        public DataModule() : base("/api/DataAPI")
        {

            #region API GET: /ping :get system state
            //Get /api/DataAPI/ping
            Get("/ping", args =>
            {
                dynamic rs = new ExpandoObject();
                rs.status = "Running";
                rs["version"] = Constants.api_version;
                rs["whatnews"] = Constants.whatnews;
                return Response.AsJson((object)rs, HttpStatusCode.OK);
                
            });
            #endregion

            #region API POST: api/DataAPI/GetDevice : Get Data Device
            //POST api/DataAPI/GetDevice
            Post("/GetDevice", args =>
            {
                try
                {
                    Device_respond device = AccessDevice.DV_res();
                    if (device != null)
                    {
                        return Response.AsJson(device, HttpStatusCode.OK);
                    }
                    else
                    {
                        return Response.AsJson("Device doesn't exist!", HttpStatusCode.ExpectationFailed);
                    }
                }
                catch (Exception Ex)
                {
                    Program.saveLog("API: api/DataDevice/GetData -> " + Ex.Message);
                    return Response.AsJson(Ex.Message, HttpStatusCode.ExpectationFailed);
                }
            });
            #endregion

            #region API POST: /ReadDeviceDataByDay :Load device's data in a day
            //POST api/DataAPI/ReadDeviceDataByDay
            Post("/ReadDeviceDataByDay", args =>
            {
                var request = JsonConvert.DeserializeObject<HttpRequest>(Request.Body.AsString());
                try
                {

                    var listInput = Program.Device.inputCode;
                    Dictionary<string, Dictionary<string, string>> res = new Dictionary<string, Dictionary<string, string>>();
                    
                    DateTime fromDate = new DateTime(request.year, request.month, request.day, 0, 0, 0);
                    DateTime toDate = new DateTime(request.year, request.month, request.day, 23, 59, 59);
                    var devicedata = DeviceData.getDevice(request.device_id);
                    var data_recv = devicedata.searchData(fromDate, toDate);
                    foreach (var item in listInput) res[item] = new Dictionary<string, string>();
                    foreach (var datedata in data_recv)
                    {
                        string oaDateTime = datedata.datetime.ToOADate().ToString();
                        foreach (var numInput in res.Keys)
                            if (datedata.codedata.ContainsKey(numInput))
                                res[numInput][oaDateTime] = datedata.codedata[numInput];
                    }
                    //foreach (var numInput in res.Keys)
                    //    res[numInput] = AccessData.DataCal(res[numInput], listInput, numInput);
                    return JsonConvert.SerializeObject(res);// Response.AsJson(res, HttpStatusCode.OK);

                }
                catch (Exception Ex)
                {
                    Program.saveLog("API: api/DataAPI/ReadDeviceDataByDay -> " + Ex.Message);
                    return Response.AsJson(Ex.Message, HttpStatusCode.ExpectationFailed);
                }
            });
            #endregion

        }
    }
}
