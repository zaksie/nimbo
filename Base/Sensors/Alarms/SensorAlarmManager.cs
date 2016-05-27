using Base.Devices;
using Base.Sensors.Types;
using Base.Sensors.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.Sensors.Alarms
{
    public delegate void LoggerStatusChangedDelegate(GenericLogger sender, eStatus status);
    public enum eStatus
    {
        Normalized,
        Alarm
    }

    public class SensorAlarmManager
    {
        public event LoggerStatusChangedDelegate OnLoggerStatusChanged;
    
        private eStatus status = eStatus.Normalized;
        private ISensorManager ParentSensorManager;
        public eStatus Status 
        {
            get { return status; }
            private set
            {
                if (status != value)
                {
                    status = value;
                    notifyStatusChange();
                }

                status = value;
            }
        }

        public ushort Delay { get; set; }
        public ushort WarningDelay { get; set; }
        public ushort Duration { get; set; }
        public bool SoundAlarmOnWarning { get; set; }

        public SensorAlarmManager(ISensorManager i_Parent)
        {
            this.ParentSensorManager = i_Parent;
        }

        #region Events
        private void Alarm_SensorStatusChangedEvent(object sender, SensorAlarmStatusArgs e)
        {
            var sensorsInAlarm = (from sensor in ParentSensorManager.GetSensors()
                                  where sensor.Alarm.Status != SensorAlarm.eStatus.Normalized
                                  select sensor).ToArray();
            Status = sensorsInAlarm.Count() > 0 ? eStatus.Alarm : eStatus.Normalized;
        }

        protected void notifyStatusChange()
        {
            if (OnLoggerStatusChanged != null)
            {
                OnLoggerStatusChanged(ParentSensorManager.ParentLogger, Status);
            }
        }
        #endregion

        public void RegisterForStatusChange(SensorAlarm alarm)
        {
            alarm.OnSensorStatusChanged += Alarm_SensorStatusChangedEvent;
        }

        public void UnregisterForStatusChange(SensorAlarm alarm)
        {
            alarm.OnSensorStatusChanged -= Alarm_SensorStatusChangedEvent;
        }
    }
}
