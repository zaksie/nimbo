using Base.Devices;
using Base.Devices.Management;
using Base.OpCodes.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.OpCodes.Management
{
    public class OpCodeInvokerArgs : EventArgs
    {
        public Result Result
        {
            get;
            set;
        }

        public object[] Parameters
        {
            get;
            set;
        }

        public GenericDevice Device { get { return OpCode.ParentDevice; } }
        public OpCode OpCode { get; set; }
    }
}
