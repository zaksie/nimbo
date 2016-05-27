using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Base.Sensors.Units
{
    [Serializable]
    public class Celcius : IUnit, ISerializable
    {
        public Celcius()
        {

        }

        public string Name
        {
            get { return "C"; }
        }

        #region Serialization & Deserialization
        public Celcius(SerializationInfo info, StreamingContext context)
        {

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }

        #endregion
    }
}
