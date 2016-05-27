using Base.Sensors.Alarms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Base.Sensors.Samples
{
    [Serializable]
    public class Sample : ISerializable
    {
        public double Value { get; set; }
        public DateTime Date { get; set; }
        public string Comment { get; set; }
        public SensorAlarm.eStatus AlarmStatus { get; set; }

        public Sample(DateTime i_Date, double i_Value, SensorAlarm.eStatus i_AlarmStatus, string i_Comment)
        {
            Date = i_Date;
            Value = i_Value;
            Comment = i_Comment;
            AlarmStatus = i_AlarmStatus;
        }

        #region Override Methods
        public override bool Equals(object obj)
        {
            Sample sample = obj as Sample;
            if (obj == null)
            {
                return false;
            }

            if (!sample.AlarmStatus.Equals(this.AlarmStatus) || !sample.Comment.Equals(this.Comment) || !sample.Date.Equals(this.Date) || !sample.Value.Equals(this.Value))
            {
                return false;
            }

            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
        #region Serialization & Deserialization
        public Sample(SerializationInfo info, StreamingContext ctxt)
        {
            Value = (double)AuxiliaryLibrary.Tools.IO.GetSerializationValue<double>(ref info, "Value");
            Date = (DateTime)AuxiliaryLibrary.Tools.IO.GetSerializationValue<DateTime>(ref info, "Date");
            Comment = (string)AuxiliaryLibrary.Tools.IO.GetSerializationValue<string>(ref info, "Comment");
            AlarmStatus = (SensorAlarm.eStatus)AuxiliaryLibrary.Tools.IO.GetSerializationValue<SensorAlarm.eStatus>(ref info, "AlarmStatus");
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Value", Value);
            info.AddValue("Date", Date);
            info.AddValue("Comment", Comment);
            info.AddValue("AlarmStatus", AlarmStatus);
        }
        #endregion
    }
}
