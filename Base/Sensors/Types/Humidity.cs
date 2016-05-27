using Base.Sensors.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Base.Sensors.Types
{
    [Serializable]
    public abstract class Humidity : PredefinedSensor, ISerializable
    {
        public Humidity(ISensorManager i_Parent)
            :base(i_Parent)
        {
            Calibration = new Base.Sensors.Calibrations.TwoPoint();
        }
  
        public override eSensorType Type
        {
            get { return eSensorType.Humidity; }
        }

        public override double Maximum
        {
            get { return 100; }
        }

        public override double Minimum
        {
            get { return 0; }
        }

        public override string Name
        {
            get { return "Internal RH"; }
        }

        public override Units.IUnit Unit
        {
            get { return new Base.Sensors.Units.Humidity(); }
        }

        public override bool USBRunnable
        {
            get { return true; }
        }
        #region Serialization & Deserialization
        public Humidity(SerializationInfo info, StreamingContext context)
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
