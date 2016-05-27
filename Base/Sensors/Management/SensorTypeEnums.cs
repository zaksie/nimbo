using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Sensors.Management
{
    public enum eSensorType : byte
    {
        None = 0,
        InternalNTC,
        DigitalTemperature,
        Humidity,
        DewPoint,
        Current4_20mA,
        Voltage0_10V,
        ExternalNTC,
        PT100,
    }

    public enum eSensorIndex : byte
    {
        Temperature = 0,
        Humidity = 1,
        External1 = 2,
        External2 = 3,
        External3 = 4,
        External4 = 5,
    }
}
