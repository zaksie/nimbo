using Base.Sensors.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

namespace Base.Sensors.Alarms
{
    [Serializable]
    public class SensorAlarm : ISerializable, IDisposable
    {
        #region Fields
        public enum eStatus
        {
            Normalized,
            HighAlarm,
            HighWarning,
            LowAlarm,
            LowWarning,
        }

        public delegate void SensorStatusChangedDelegate(GenericSensor sender, SensorAlarmStatusArgs e);
        public event SensorStatusChangedDelegate OnSensorStatusChanged;

        private GenericSensor m_Parent;
        private eStatus m_Status;
        private Timer m_DelayTimer;
        protected eStatus m_PrevStatus;

        #endregion
        #region Properties
        public bool Enabled { get; set; }
        protected SensorAlarmManager AlarmManager { get { return m_Parent.ParentSensorManager.GetAlarmManager(); } }

        public double LowAlarm
        {
            get;
            private set;
        }

        public double HighAlarm
        {
            get;
            private set;
        }

        public eStatus Status 
        { 
            get {return m_Status;}
            private set
            {
                m_PrevStatus = m_Status;
                m_Status = value;
            }
        }

        #endregion
        #region Constructors
        public SensorAlarm(GenericSensor i_Parent)
        {
            Initialize(i_Parent);
        }

        private void Initialize(GenericSensor i_Parent)
        {
            this.m_Parent = i_Parent;
            LowAlarm = double.MinValue;
            HighAlarm = double.MaxValue;
            Status = default(eStatus);
            m_DelayTimer = new Timer(new TimerCallback(notifyStatusChange));
        }

        #endregion
        #region Methods
        public eStatus CheckSampleAndNotifyOnAlarm(double i_Value, bool i_NotifyAlarm)
        {
            Status = checkLowHighAlarms(i_Value);

            if (i_NotifyAlarm && statusChanged())
            {
                DelayNotificationOfStatusChange();
            }

            return Status;
        }

        protected bool statusChanged()
        {
            return m_Status != m_PrevStatus;
        }

        protected void DelayNotificationOfStatusChange()
        {
            if (AlarmManager.Delay > 0)
	        {
                m_DelayTimer.Change(AlarmManager.Delay, AlarmManager.Delay);
	        }
            else
            {
                notifyStatusChange(null);
            }
        }

        private void notifyStatusChange(object sender)
        {
            if (sender is System.Threading.Timer)
            {
                m_DelayTimer.Change(Timeout.Infinite, Timeout.Infinite);                
            }

            if (OnSensorStatusChanged != null)
            {
                OnSensorStatusChanged(m_Parent, generateAlarmStatusArgs());
            }
        }

        private SensorAlarmStatusArgs generateAlarmStatusArgs()
        {
            return new SensorAlarmStatusArgs(Status, m_PrevStatus);
        }

        #endregion
        #region Virtual Methods
        public virtual void Set(double? i_LowAlarm, double? i_HighAlarm)
        {
            if (i_LowAlarm == null)
            {
                LowAlarm = double.MinValue;
            }
            else
            {
                if (i_LowAlarm < m_Parent.Minimum)
                {
                    throw new ArgumentException("Low alarm is smaller than sensor minimum", "i_LowAlarm");
                }

                LowAlarm = i_LowAlarm.Value;
            }

            if (i_HighAlarm == null)
            {
                HighAlarm = double.MaxValue;
            }
            else
            {
                if (i_HighAlarm > m_Parent.Maximum)
                {
                    throw new ArgumentException("High alarm is bigger than sensor minimum", "i_HighAlarm");
                }

                HighAlarm = i_HighAlarm.Value;
            }

            if (i_LowAlarm == null && i_HighAlarm == null)
            {
                Enabled = false;
            }
            else
            {
                if (LowAlarm >= HighAlarm)
                {
                    LowAlarm = double.MinValue;
                    HighAlarm = double.MaxValue;
                    Enabled = false;

                    throw new ArgumentException("Low alarm is bigger than high alarm");
                }
                else
                {
                    Enabled = true;                    
                }
            }
        }

        protected virtual eStatus checkLowHighAlarms(double i_Value)
        {
            eStatus statusToReturn = eStatus.Normalized;

            if (i_Value < LowAlarm)
            {
                statusToReturn = eStatus.LowAlarm;
            }
            else if (i_Value > HighAlarm)
            {
                statusToReturn = eStatus.HighAlarm;
            }

            return statusToReturn;
        }

        #endregion
        #region Serialization & Deserialization
        public SensorAlarm(SerializationInfo info, StreamingContext ctxt)
        {
            m_Status = (eStatus)AuxiliaryLibrary.Tools.IO.GetSerializationValue<eStatus>(ref info, "m_Status");
            m_PrevStatus = (eStatus)AuxiliaryLibrary.Tools.IO.GetSerializationValue<eStatus>(ref info, "m_PrevStatus");
            Enabled = (bool)AuxiliaryLibrary.Tools.IO.GetSerializationValue<bool>(ref info, "Enabled");
            LowAlarm = (double)AuxiliaryLibrary.Tools.IO.GetSerializationValue<double>(ref info, "LowAlarm");
            HighAlarm = (double)AuxiliaryLibrary.Tools.IO.GetSerializationValue<double>(ref info, "HighAlarm");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("m_Status", m_Status);
            info.AddValue("m_PrevStatus", m_PrevStatus);
            info.AddValue("Enabled", Enabled);
            info.AddValue("LowAlarm", LowAlarm);
            info.AddValue("HighAlarm", HighAlarm);
        }
        #endregion
        #region IDisposable
        public void Dispose()
        {
        }
        #endregion
    }
}
