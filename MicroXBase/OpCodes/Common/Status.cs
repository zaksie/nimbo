using Base.Devices;
using Base.OpCodes.Helpers;
using Base.OpCodes.Management;
using MicroXBase.Devices.Types;
using MicroXBase.OpCodes.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MicroXBase.OpCodes.Management;
using AuxiliaryLibrary.MathLib;

namespace MicroXBase.OpCodes.Common
{
    [Serializable]
    public abstract class Status : MicroXOpCode
    {
        #region Constructors
        public Status(MicroXDevice device)
            : base(device)
        { }
        #endregion
        #region Parse Methods
        protected abstract void ParseSetupBooleans();
        protected abstract void ParseTime();
        protected abstract void ParseInterval();
        protected abstract void ParseAlarms();
        protected abstract void ParseLCDConfiguration();
        protected abstract Alarm ParseAlarm();
        protected abstract IntegerFraction ParseAlarmEnd();
        protected abstract void ParseExternalSensorStatus();
        protected abstract void ParseFirmwareVersion();
        protected abstract void ParseBatteryLevel();
        protected abstract void ParseTimerStatus();
        protected abstract void ParseAlarmInfo();
        protected abstract void PopulateAssembly();
        protected abstract void ParseDeviceType();
        #endregion
    }
}
