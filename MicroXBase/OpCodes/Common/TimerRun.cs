using Base.Devices;
using Base.OpCodes.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MicroXBase.OpCodes.Management;
using MicroXBase.Devices.Types;
using MicroXBase.OpCodes.Helpers;

namespace MicroXBase.OpCodes.Common
{
    [Serializable]
    public abstract class TimerRun: MicroXOpCode
    {
        #region Constructors
        public TimerRun(MicroXDevice device)
            : base(device)
        {
        }
        #endregion
        #region Populate Methods
        protected override void PopulateReport()
        {
            PopulateOpCodeSend();
            PopulateTime();
        }

        protected virtual void PopulateTime()
        {
            if (ParentOpCodeWrapper.Setup.Configuration.TimerRunEnabled)
            {
                MicroXTime time = new MicroXTime();
                time.Set(ParentOpCodeWrapper.Setup.Configuration.TimerStart);
                OutReport.InsertBlock(time.GetBytes());
            }
        }
        #endregion
    }
}
