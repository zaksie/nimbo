using Base.Devices;
using Base.Sensors.Alarms;
using Base.Sensors.Types;
using Base.Sensors.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.Sensors.Management
{
    public delegate void SampleAddedDelegate(object sender, SampleAddedEventArgs e);

    public interface ISensorManager
    {
        event SampleAddedDelegate OnSampleAdded;
        event SampleAddedDelegate OnTimeStampAdded;

        SensorAlarmManager GetAlarmManager();
        bool FahrenheitMode
        {
            get;
        }
        GenericSensor[] GetSensors();
        GenericLogger ParentLogger
        { get; }

        void SampleAdded(GenericSensor genericSensor, Sample sample);
        void TimeStampAdded(GenericSensor genericSensor, Sample sample);

        PredefinedSensor Create(eSensorType Type);
    }
}
