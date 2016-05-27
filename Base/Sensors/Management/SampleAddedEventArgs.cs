using Base.Devices;
using Base.Sensors.Types;
using Base.Sensors.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.Sensors.Management
{
    public class SampleAddedEventArgs : EventArgs
    {
        public GenericLogger Logger { get; set; }
        public GenericSensor Sensor { get; set; }
        public Sample Sample { get; set; }
    }
}
