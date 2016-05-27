using Base.Devices.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroXBase.Devices.Features
{
    public enum MicroXDeviceFeatureEnum : byte
    {
        CYCLIC = 0x00,
        PUSH_TO_RUN,
        TIMER_RUN,
        STOP_ON_KEY_PRESS,
        STOP_ON_DISCONNECT,
        TEST_MODE,
        MEMORY_SIZE,
        SHOW_MINMAX,
        DEEP_SLEEP,
        LED_ON_ALARM,
    }

    public class MicroXFeature : DeviceFeature
    {
        #region Ready-To-Use DeviceFeatures
        public static MicroXFeature CYCLIC { get { return new MicroXFeature(MicroXDeviceFeatureEnum.CYCLIC); } }
        public static MicroXFeature PUSH_TO_RUN { get { return new MicroXFeature(MicroXDeviceFeatureEnum.PUSH_TO_RUN); } }
        public static MicroXFeature TIMER_RUN { get { return new MicroXFeature(MicroXDeviceFeatureEnum.TIMER_RUN); } }
        public static MicroXFeature STOP_ON_KEY_PRESS { get { return new MicroXFeature(MicroXDeviceFeatureEnum.STOP_ON_KEY_PRESS); } }
        public static MicroXFeature STOP_ON_DISCONNECT { get { return new MicroXFeature(MicroXDeviceFeatureEnum.STOP_ON_DISCONNECT); } }
        public static MicroXFeature TEST_MODE { get { return new MicroXFeature(MicroXDeviceFeatureEnum.TEST_MODE); } }
        public static MicroXFeature MEMORY_SIZE { get { return new MicroXFeature(MicroXDeviceFeatureEnum.MEMORY_SIZE); } }
        public static MicroXFeature SHOW_MINMAX { get { return new MicroXFeature(MicroXDeviceFeatureEnum.SHOW_MINMAX); } }
        public static MicroXFeature DEEP_SLEEP { get { return new MicroXFeature(MicroXDeviceFeatureEnum.DEEP_SLEEP); } }
        public static MicroXFeature LED_ON_ALARM { get { return new MicroXFeature(MicroXDeviceFeatureEnum.LED_ON_ALARM); } }
        #endregion
        public MicroXDeviceFeatureEnum Feature
        {
            get;
            protected set;
        }
        public override byte Code
        {
            get { return (byte)Feature; }
        }
        public override string Text
        {
            get { return Feature.ToString(); }
        }
        
        public MicroXFeature(MicroXDeviceFeatureEnum code)
        {
            this.Feature = code;
        }
    }
}
