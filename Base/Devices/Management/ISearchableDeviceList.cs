using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
using Base.Sensors;
using Base.Devices;

namespace Base.Devices
{
    public interface ISearchableDeviceList
    {
        T[] FindDevices<T>(string SerialNumber) where T : GenericDevice, ISerializable;
    }
}
