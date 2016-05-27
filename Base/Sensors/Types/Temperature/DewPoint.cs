using Base.Sensors.Management;
using Base.Sensors.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Base.Sensors.Types.Temperature
{
    [Serializable]
    public abstract class DewPoint : GenericTemperatureSensor, ISerializable
    {
        public DewPoint(ISensorManager i_Parent)
            :base(i_Parent)
        {
        }

        public override eSensorType Type
        {
            get { return eSensorType.DewPoint; }
        }

        public override double Maximum
        {
            get { return 150; }
        }

        public override double Minimum
        {
            get { return -50; }
        }

        public override string Name
        {
            get { return "Dew Point"; }
        }

        public override bool USBRunnable
        {
            get { return true; }
        }

        public override long GetDigitalValue(double i_SensorValue)
        {
            throw new NotSupportedException();
        }

        public override double GetSensorValue(long i_DigitalValue)
        {
            throw new NotSupportedException("Please use override method, 10x");
        }

        public double GetSensorValue(TemperatureAndHumidity i_TemperatureAndHumidity)
        {
            return (1.0 / ((1.0 / (i_TemperatureAndHumidity.Temperature.Value + 273.0)) - (Math.Log(i_TemperatureAndHumidity.Humidity.Value / 100) / 5420.0)) - 273.0);
        }

        #region Serialization & Deserialization
        public DewPoint(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {

        }

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        #endregion
    }
}
