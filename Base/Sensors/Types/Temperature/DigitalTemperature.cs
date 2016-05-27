using Base.Sensors.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Sensors.Samples;
using Base.Sensors.Management;
using System.Runtime.Serialization;


namespace Base.Sensors.Types.Temperature
{
    [Serializable]
    public class DigitalTemperature : GenericTemperatureSensor, ISerializable
    {
        public DigitalTemperature(ISensorManager i_Parent)
            :base(i_Parent)
        {
        }

        public override eSensorType Type
        {
            get { return eSensorType.DigitalTemperature; }
        }

        public override double Maximum
        {
            get { return 80; }
        }

        public override double Minimum
        {
            get { return -40; }
        }

        public override string Name
        {
            get { return "Internal Digital Temperature"; }
        }

        public override bool USBRunnable
        {
            get { return true; }
        }

        public override long GetDigitalValue(double i_SensorValue)
        {
            return Convert.ToInt32((i_SensorValue + 39.63) / 0.01);
        }

        public override double GetSensorValue(long i_DigitalValue)
        {
            return -39.63 + 0.01 * i_DigitalValue;
        }

        #region Serialization & Deserialization
        public DigitalTemperature(SerializationInfo info, StreamingContext context)
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
