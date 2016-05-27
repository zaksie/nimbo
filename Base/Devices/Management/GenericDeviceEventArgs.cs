using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.Devices.Management
{
    public enum DeviceStateEnum { FOUND, REMOVED, NONE, RECONNECTED };

    public class DeviceEventArgs<T> : EventArgs where T : GenericDevice
    {
        public T Device { get; set; }
        public Type Type { get { return Device.GetType(); } }
        public DeviceStateEnum State { get; set; }
        public string SerialNumber { get { return Device.Status.SerialNumber; } }

        public DeviceEventArgs()
        {
        }
    }
}

