using Base.Sensors.Management;
using Base.Sensors.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Base.Sensors.Types
{
    [Serializable]
    public abstract class Voltage0_10V : PredefinedSensor, ISerializable
    {
        public Voltage0_10V(ISensorManager i_Parent)
            :base(i_Parent)
        {
        }

        public override double Maximum
        {
            get { return 10; }
        }

        public override double Minimum
        {
            get { return 0; }
        }

        public override Units.IUnit Unit
        {
            get { return new Base.Sensors.Units.Voltage(); }
        }
        public override eSensorType Type
        {
            get { return eSensorType.Voltage0_10V; }
        }

        public override string Name
        {
            get { return "Voltage 0 - 10 V"; }
        }
        #region Serialization & Deserialization
        public Voltage0_10V(SerializationInfo info, StreamingContext context)
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
