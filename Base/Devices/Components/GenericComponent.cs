using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Devices.Components
{
    [Serializable]
    public abstract class GenericComponent
    {
        protected GenericDevice ParentDevice { get; set; }

        public GenericComponent(GenericDevice device)
        {
            Initialize(device);
        }

        protected void Initialize(GenericDevice device)
        {
            this.ParentDevice = device;
        }
    }
}
