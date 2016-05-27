using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base;
using Base.Sensors;
using AuxiliaryLibrary;
using AuxiliaryLibrary.Tools;
using AuxiliaryLibrary.MathLib;
using Base.Devices;
using Base.OpCodes.Management;
using Base.Sensors.Management;
using Base.Sensors.Types;
using Base.Devices.Components;
using MicroXBase.OpCodes.Management;
using MicroXBase.Devices.Types;
using MicroXBase.OpCodes.Helpers;
using Base.OpCodes.Helpers;
using MicroXBase.OpCodes.DataStructures;

namespace MicroXBase.OpCodes.Common
{
    [Serializable]
    public abstract class Setup : MicroXOpCode
    {
        #region Properties
        public IMicroXConfiguration Configuration;
        #endregion
        #region Constructors
        public Setup(MicroXDevice device)
            : base(device)
        { }
        #endregion
        #region Invoke Methods
        new public IAsyncResult BeginInvoke(AsyncCallback callback)
        {
            if (Configuration == null)
                Finished(Result.ILLEGAL_CALL);

            return base.BeginInvoke(callback);
        }
        new public Result Invoke()
        {
            if (Configuration == null)
                return Result.ILLEGAL_CALL;
            else
                return base.Invoke();
        }
        public IAsyncResult BeginInvoke(IMicroXConfiguration configuration, AsyncCallback callback)
        {
            Configuration = configuration;
            return BeginInvoke(callback);
        }

        public Result Invoke(IMicroXConfiguration configuration)
        {
            Configuration = configuration;
            return Invoke();
        }
        #endregion
        #region Populate Methods
        protected abstract void PopulateSetupBooleans();
        protected abstract void PopulateTime();
        protected abstract void PopulateInterval();
        protected abstract void PopulateAlarms();
        protected abstract void PopulateLCDConfiguration();
        protected abstract void PopulateAlarm(Alarm AlarmValue);
        protected abstract void PopulateExternalSensorStatus();
        protected abstract void PopulateAlarmInfo();
        #endregion

    }
}
