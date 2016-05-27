using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Base.Sensors.Samples
{
    [Serializable]
    public class TemperatureAndHumidity : ISerializable
    {
        public Sample Temperature { get; private set; }
        public Sample Humidity { get; private set; }

        public TemperatureAndHumidity(Sample i_Temperature, Sample i_Humidity)
        {
            if (i_Temperature.Date == i_Humidity.Date)
            {
                Temperature = i_Temperature;
                Humidity = i_Humidity;
            }
            else
            {
                throw new Exception("Temperature and Humidity DateTime are different");
            }
        }

        #region Serialization & Deserialization
        public TemperatureAndHumidity(SerializationInfo info, StreamingContext context)
        {
            Temperature = (Sample)AuxiliaryLibrary.Tools.IO.GetSerializationValue<Sample>(ref info, "Temperature");
            Humidity = (Sample)AuxiliaryLibrary.Tools.IO.GetSerializationValue<Sample>(ref info, "Humidity");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Temperature", Temperature);
            info.AddValue("Humidity", Humidity);            
        }

        #endregion
    }
}
