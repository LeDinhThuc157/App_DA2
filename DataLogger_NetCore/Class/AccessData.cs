using OneSignalApi.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataLogger_NetCore.Class
{
    public class AccessDevice
    {
        public static Device_respond DV_res()
        {
            Device_respond dv = new Device_respond();
            var dev = Program.Device;
            dv.Device_id = dev.Device_id;
            dv.Device_name = dev.Device_name;
            dv.lastData = dev.lastData;
            dv.lastEdit = dev.lastEdit;
            dv.lastReceived = dev.lastTimeSystem;
            dv.lastTimeSystem = dev.lastTimeSystem;
            dv.Status = dev.Status;
            dv.version_running = dev.version_running;
            dv.inputCode = dev.inputCode;
            return dv;
        }

    }

}
