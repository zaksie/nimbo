using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Base.Sensors.Units
{
    [Serializable]
    public class Current : IUnit, ISerializable
    {
        public Current()
        {

        }

        public string Name
        {
            get { return "mA"; }
        }

        #region Serialization & Deserialization
        public Current(SerializationInfo info, StreamingContext context)
        {

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }

        #endregion
    }
}
