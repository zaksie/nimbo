using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Base.Sensors.Units
{
    [Serializable]
    public class Custom : IUnit, ISerializable
    {
        public Custom()
        {

        }

        public string Name
        {
            get;
            set;
        }

        #region Serialization & Deserialization
        public Custom(SerializationInfo info, StreamingContext context)
        {
            Name = (string)AuxiliaryLibrary.Tools.IO.GetSerializationValue<string>(ref info, "Name");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Name", Name, typeof(string));
        }

        #endregion
    }
}
