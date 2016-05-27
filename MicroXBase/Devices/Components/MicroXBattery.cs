using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Devices.Components;
using MicroXBase.Devices.Types;

namespace MicroXBase.Devices.Components
{
    public class MicroXBattery : GenericBattery
    {
        #region Properties
        new protected MicroXDevice ParentDevice
        {
            get { return base.ParentDevice as MicroXDevice; }
            set { base.ParentDevice = value; }
        }
        public override byte BatteryLevel
        {
            get { return ParentDevice.Configuration.BatteryLevel; }
        }

        public override bool HasAlarm
        {
            get { return false; }
        }

        #endregion
        #region Constructors
        public MicroXBattery(MicroXDevice device)
            :base(device)
        {

        }
        #endregion
    }
}
