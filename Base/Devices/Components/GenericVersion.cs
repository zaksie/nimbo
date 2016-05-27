using AuxiliaryLibrary.MathLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Devices.Components
{
    public abstract class GenericVersion : GenericComponent
    {
        #region Properties
        public abstract byte PCBAssembly           {get;}
        public abstract byte PCBVersion                {get;}
        public abstract byte FirmwareRevision   {get;}
        public abstract float Firmware { get; }
        public Version FirmwareVersion
        {
            get
            {
                IntegerFraction intFrac = IntegerFraction.ConvertFromDouble(Firmware);
                return new Version(intFrac.intInteger, intFrac.intFraction);
            }
        }
        public virtual Version FullVersion
        {
            get
            {
                return new Version(PCBVersion.ToString() + "." + Firmware.ToString("f2") + "." + FirmwareRevision);
            }
        }
        #endregion
        #region Constructors
        public GenericVersion(GenericDevice device)
            : base(device)
        {
        }
        #endregion
    }
}
