using Base.Sensors.Management;
using Base.Sensors.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Base.Sensors.Types.Temperature
{
    [Serializable]
    public abstract class GenericTemperatureSensor : PredefinedSensor, ISerializable
    {
        public GenericTemperatureSensor(ISensorManager i_Parent)
            :base(i_Parent)
        {
        }

        public override Units.IUnit Unit
        {
            get 
            {
                if (ParentSensorManager == null || !ParentSensorManager.FahrenheitMode)
                {
                    return new Celcius();
                }
                else
                {
                    return new Fahrenheit();
                }
            }
        }

        #region Serialization & Deserialization
        public GenericTemperatureSensor(SerializationInfo info, StreamingContext context)
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
