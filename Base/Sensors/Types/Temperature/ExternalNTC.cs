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
    public abstract class ExternalNTC : GenericTemperatureSensor, ISerializable
    {
        public override eSensorType Type
        {
            get { return eSensorType.ExternalNTC; }
        }

        public override string Name
        {
            get { return "External NTC"; }
        }
        
        public override double Maximum
        {
            get { return 150; }
        }

        public override double Minimum
        {
            get { return -50; }
        }

        public ExternalNTC(ISensorManager i_Parent)
            :base(i_Parent)
        {
        }

        #region Serialization & Deserialization
        public ExternalNTC(SerializationInfo info, StreamingContext context)
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
