using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Devices.Components
{
    [Serializable]
    public abstract class GenericBattery : GenericComponent
    {
        public const int WAIT_FOR_EMPTY_QUEUE = 100;
        public const int LOW_BATTERY_LEVEL = 25;
        public const int MED_BATTERY_LEVEL = 50;
        public const int HIGH_BATTERY_LEVEL = 100;
        public const int NAN_BATTERY_LEVEL = 255;

        public const int MAX_BATTERY_PERCENTAGE = 100;

        #region Constructors
        public GenericBattery(GenericDevice device)
            : base(device)
        {
        }
        #endregion

        public abstract bool HasAlarm
        {
            get;
        }

        public abstract Byte BatteryLevel
        {
            get;
        }
    }
}
