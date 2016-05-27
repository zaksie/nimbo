using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Devices.Features;

namespace MicroXBase.Devices.Features
{
    public class MicroXAvailableFunction : AvailableFunction
    {
        public MicroXAvailableFunction(MicroXDeviceFunctionEnum function)
            : base((byte)function)
        {
        }

        public MicroXDeviceFunctionEnum Function
        {
            get
            {
                if (Enum.IsDefined(typeof(MicroXDeviceFunctionEnum), Code))
                    return (MicroXDeviceFunctionEnum)Code;
                else
                    return MicroXDeviceFunctionEnum.NONE;
            }
        }
    }
}
