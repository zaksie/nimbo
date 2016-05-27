using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Base.Sensors.Alarms;
using Base.Sensors.Samples;
using Base.Sensors.Units;
using Base.Sensors.Management;

namespace Base.Sensors.Types
{
    [Serializable]
    public abstract class GenericSensor : ISerializable, IDisposable
    {
        #region Const
        public static readonly decimal KELVIN_0 = 273.15m;

        #endregion
        #region Abstract Members
        public abstract double Maximum { get; }
        public abstract double Minimum { get; }
        public abstract bool USBRunnable { get; }

        #endregion
        #region Properties
        public SampleList Samples { get; protected set; }
        public SampleList TimeStamps { get; protected set; }

        public IReadOnlyList<Sample> ReadOnlySamples { get { return Samples.AsReadOnly(); } }
        public IReadOnlyList<Sample> ReadOnlyTimeStamps { get { return TimeStamps.AsReadOnly(); } }

        public CalibrationInformation Calibration { get; protected set; }
        public SensorAlarm Alarm { get; protected set; }
        public bool Enabled { get; set; }
        public string Text { get; set; }
        public ISensorManager ParentSensorManager { get; protected set; }
        public eSensorIndex Index { get; set; }
        
        public bool IsEmpty
        {
            get { return Samples.Count == 0 && TimeStamps.Count == 0; }
        }

        public Sample LastSample
        {
            get
            {
                if (Samples.Count != 0)
                {
                    return Samples[Samples.Count - 1];
                }
                else
                {
                    return null;
                }
            }
        }

        public Sample LastTimeStamp
        {
            get
            {
                if (TimeStamps.Count != 0)
	            {
                    return TimeStamps[TimeStamps.Count - 1];
	            }

                else
                {
                    return null;
                }
            }
        }
        #endregion
        #region Constructors
        public GenericSensor(ISensorManager i_Parent)
        {
            Initialize(i_Parent);
        }

        protected virtual void Initialize(ISensorManager i_Parent)
        {
            if (i_Parent == null)
                return;

            Samples = new SampleList(this);
            TimeStamps = new SampleList(this);
            Alarm = new SensorAlarm(this);
            
            InitializeParent(i_Parent);
            InitializeCalibration();

            Samples.OnSampleAdded += ParentSensorManager.SampleAdded;
            TimeStamps.OnSampleAdded += ParentSensorManager.TimeStampAdded;
        }

        protected virtual void InitializeCalibration()
        {
            Calibration = new Base.Sensors.Calibrations.TwoPoint();
        }
        public void InitializeParent(ISensorManager i_Parent)
        {
            ParentSensorManager = i_Parent;            
        }

        #endregion
        #region Abstract Methods
        public abstract long GetDigitalValue(double i_SensorValue);
        public abstract double GetSensorValue(long i_DigitalValue);
        #endregion
        #region Methods
        internal Sample GenerateSample(long i_Value, DateTime i_Date, string i_Comment, bool i_NotifayAlarm)
        {
            double sensorValue = GetSensorValue(i_Value);
            SensorAlarm.eStatus alarmStatus = Alarm.CheckSampleAndNotifyOnAlarm(sensorValue, i_NotifayAlarm && Alarm.Enabled);

            return new Sample(i_Date, sensorValue, alarmStatus, i_Comment);
        }

        public void ClearData()
        {
            Samples.Clear();
            TimeStamps.Clear();
        }

        #endregion
        #region Serialization & Deserialization
        public GenericSensor(SerializationInfo info, StreamingContext ctxt) 
        {
            Samples = (SampleList)AuxiliaryLibrary.Tools.IO.GetSerializationValue<SampleList>(ref info, "Samples");
            TimeStamps = (SampleList)AuxiliaryLibrary.Tools.IO.GetSerializationValue<SampleList>(ref info, "TimeStamps");
            Calibration = (CalibrationInformation)AuxiliaryLibrary.Tools.IO.GetSerializationValue<CalibrationInformation>(ref info, "Calibration");
            Alarm = (SensorAlarm)AuxiliaryLibrary.Tools.IO.GetSerializationValue<SensorAlarm>(ref info, "Alarm");
            Text = (string)AuxiliaryLibrary.Tools.IO.GetSerializationValue<string>(ref info, "Text");
            Enabled = (bool)AuxiliaryLibrary.Tools.IO.GetSerializationValue<bool>(ref info, "Enabled");
            //Unit = (GenericUnit)AuxiliaryLibrary.Tools.IO.GetSerializationValue<GenericUnit>(ref info, "Unit");

        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("Samples", Samples);
            info.AddValue("TimeStamps", TimeStamps);
            info.AddValue("Calibration", Calibration);
            info.AddValue("Alarm", Alarm);
            info.AddValue("Text", Text);
            info.AddValue("Enabled", Enabled);
            //info.AddValue("Unit", Unit);
        }

        #endregion
        #region IDisposable
        public void Dispose()
        {
            Samples.Dispose();
            TimeStamps.Dispose();
            Alarm.Dispose();
        }
        #endregion
    }
}
