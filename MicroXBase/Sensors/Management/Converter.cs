using Base.Sensors.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroXBase.Sensors.Management
{

    public static class Converter
    {
        public static eSensorType[] ToSensorType(eSensorIndex index)
        {
            switch (index)
            {
                case eSensorIndex.Temperature:
                    return new eSensorType[] { eSensorType.DigitalTemperature, eSensorType.InternalNTC };
                case eSensorIndex.Humidity:
                    return new eSensorType[] { eSensorType.Humidity };
                case eSensorIndex.External1:
                    return new eSensorType[] 
                    { 
                        eSensorType.Current4_20mA,
                        eSensorType.Voltage0_10V,
                        eSensorType.ExternalNTC,
                        eSensorType.PT100,
                    };
                default:
                    return new eSensorType[] { };
            }
        }
    }
}
