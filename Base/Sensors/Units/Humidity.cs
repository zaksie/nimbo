using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Base.Sensors.Units
{
    [Serializable]
    public class Humidity : IUnit, ISerializable
    {
        public Humidity()
        {

        }

        public string Name
        {
            get { return "% RH"; }
        }

        #region Serialization & Deserialization
        public Humidity(SerializationInfo info, StreamingContext context)
        {

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }

        #endregion
    }
}
