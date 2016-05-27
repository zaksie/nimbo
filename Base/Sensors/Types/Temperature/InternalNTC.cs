using Base.Sensors.Management;
using Base.Sensors.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Base.Sensors.Types.Temperature
{
    [Serializable]
    public abstract class InternalNTC : GenericTemperatureSensor, ISerializable
    {
        public override eSensorType Type
        {
            get { return eSensorType.InternalNTC; }
        }

        public override string Name
        {
            get { return "Internal NTC"; }
        }

        public InternalNTC(ISensorManager i_Parent)
            :base(i_Parent)
        {
        }

        #region Serialization & Deserialization
        public InternalNTC(SerializationInfo info, StreamingContext context)
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
