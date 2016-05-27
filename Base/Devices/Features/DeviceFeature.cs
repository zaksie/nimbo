using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.Devices.Features
{
    public abstract class DeviceFeature
    {
        public abstract byte Code { get; }
        public abstract string Text { get; }
    }
}
