using Base.OpCodes.Management;
using MicroXBase.Devices.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroXBase.OpCodes.Management
{
    [Serializable]
    public abstract class MicroXOpCode : OpCode
    {
        public override byte[] AckHeader
        {
            get { return new byte[] { 0x28 }; }
        }
        
        public MicroXOpCode(MicroXDevice device)
            : base(device)
        {

        }

        new protected MicroXDevice ParentDevice
        {
            get { return base.ParentDevice as MicroXDevice; }
            set { base.ParentDevice = value; }
        }

        new protected MicroXOpCodeWrapper ParentOpCodeWrapper
        {
            get { return base.ParentOpCodeWrapper as MicroXOpCodeWrapper; }
        }
    }
}
