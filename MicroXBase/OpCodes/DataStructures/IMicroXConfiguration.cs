using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.OpCodes.Helpers;

namespace MicroXBase.OpCodes.DataStructures
{
    public interface IMicroXConfiguration : Base.OpCodes.Helpers.IConfiguration
    {
        Alarm InternalTemperatureAlarm { get; set; }

        string Comment { get; set; }
        string SerialNumber { get; set; }

        DateTime TimerStart { get; set; }
        DateTime CurrentTime { get; set; }
        bool TimerRunEnabled { get; set; }
        bool CyclicMode { get; set; }
        bool PushToRunMode { get; set; }
        bool FahrenheitMode { get; set; }
        bool SWConfigurationState { get; set; }

        bool InternalTemperatureSensorActive { get; set; }

        bool BoomerangActive { get; set; }
        ushort Interval { get; set; }
        ushort AlarmDelay { get; set; }

        bool TestMode { get; set; }
    }
}
