using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Base.Sensors.Units
{
    [Serializable]
    public class Voltage : IUnit, ISerializable
    {
        public Voltage()
        {

        }

        public string Name
        {
            get { return "V"; }
        }

        #region Serialization & Deserialization
        public Voltage(SerializationInfo info, StreamingContext context)
        {

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }

        #endregion
    }
}
