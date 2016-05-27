using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Sensors.Management;
using System.Runtime.Serialization;

namespace Base.Sensors.Types
{
    [Serializable]
    public abstract class Current4_20mA : PredefinedSensor, ISerializable
    {
        public Current4_20mA(ISensorManager i_Parent)
            :base(i_Parent)
        {
        }

        public override double Maximum
        {
            get { return 20; }
        }

        public override double Minimum
        {
            get { return 4; }
        }

        public override Units.IUnit Unit
        {
            get { return new Base.Sensors.Units.Current(); }
        }

        public override eSensorType Type
        {
            get { return eSensorType.Current4_20mA; }
        }

        public override string Name
        {
            get { return "Current 4 - 20 mA"; }
        }
        #region Serialization & Deserialization
        public Current4_20mA(SerializationInfo info, StreamingContext context)
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
