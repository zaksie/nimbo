using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Base.Sensors.Units
{
    [Serializable]
    public class Fahrenheit : IUnit, ISerializable
    {
        public Fahrenheit()
        {

        }

        public string Name
        {
            get { return "F"; }
        }

        #region Serialization & Deserialization
        public Fahrenheit(SerializationInfo info, StreamingContext context)
        {

        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
        }

        #endregion
    }
}
