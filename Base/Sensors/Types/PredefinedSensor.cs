using Base.Sensors.Management;
using Base.Sensors.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Base.Sensors.Types
{
    [Serializable]
    public abstract class PredefinedSensor : GenericSensor, ISensor, ISerializable
    {
        #region Properties
        public abstract eSensorType Type { get; }
        public abstract string Name { get; }
        public abstract IUnit Unit { get; }
        #endregion
        #region Constructors
        public PredefinedSensor(ISensorManager parent)
            : base(parent)
        {
        }
        #endregion
        #region Serialization & Deserialization
        public PredefinedSensor(SerializationInfo info, StreamingContext ctxt) 
            : base(info, ctxt)
        {

        }

        public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
        }

        #endregion
        #region Override Methods
        public override string ToString()
        {
            return Name + " (" + Unit + ")";
        }

        #endregion
    }
}
