using Base.Sensors.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Base.Sensors.Alarms
{
    [Serializable]
    class SensorAlarmWarning : SensorAlarm, ISerializable
    {
        #region Fields
        public double LowWarning
        {
            get;
            private set;
        }

        public double HighWarning
        {
            get;
            private set;
        }

        #endregion
        #region Constructors
        public SensorAlarmWarning(GenericSensor m_Parent)
            :base(m_Parent)
        {
        }
        #endregion
        #region Methods
        public void Set(double? i_LowAlarm, double? i_LowWarning, double? i_HighAlarm, double? i_HighWarning)
        {
            base.Set(i_LowAlarm, i_HighAlarm);

            if (i_LowWarning == null)
            {
                LowWarning = double.MinValue;
            }
            else
            {
                LowWarning = i_LowWarning.Value;
            }

            if (i_HighWarning == null)
            {
                HighWarning = double.MinValue;
            }
            else
            {
                HighWarning = i_HighWarning.Value;
            }
        }

        #endregion
        #region Override Methods
        protected override eStatus checkLowHighAlarms(double i_Value)
        {
            var baseResult = base.checkLowHighAlarms(i_Value);
            if (baseResult == eStatus.Normalized)
            {
                if (i_Value < LowWarning)
                {
                    return eStatus.LowWarning;
                }
                else if (i_Value > HighWarning)
                {
                    return eStatus.HighWarning;
                }

                return baseResult;
            }
            
            return baseResult;
        }

        // Will not support set without alarm warnings
        public override void Set(double? i_LowAlarm, double? i_HighAlarm)
        {
            throw new NotSupportedException("Please use the overloaded Set, 10x");
        }

        #endregion
        #region Serialization & Deserialization
        public SensorAlarmWarning(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            LowWarning = (double)AuxiliaryLibrary.Tools.IO.GetSerializationValue<double>(ref info, "LowWarning");
            HighWarning = (double)AuxiliaryLibrary.Tools.IO.GetSerializationValue<double>(ref info, "HighWarning");
        }

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);

            info.AddValue("LowWarning", LowWarning);
            info.AddValue("HighWarning", HighWarning);            
        }

        #endregion
    }
}
