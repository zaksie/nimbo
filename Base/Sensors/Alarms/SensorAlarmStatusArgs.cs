using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.Sensors.Alarms
{
    public class SensorAlarmStatusArgs : EventArgs
    {
        public SensorAlarm.eStatus NewStatus { get; set; }
        public SensorAlarm.eStatus PrevStatus { get; set; }

        public SensorAlarmStatusArgs(SensorAlarm.eStatus i_NewStatus, SensorAlarm.eStatus i_PrevStatus)
        {
            this.NewStatus = i_NewStatus;
            this.PrevStatus = i_PrevStatus;
        }
    }
}
