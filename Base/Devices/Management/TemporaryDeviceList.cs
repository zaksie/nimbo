using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Devices.Management
{
    /// <summary>
    /// This class hold a temporary list of devices till they are further differentiated into device classes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TemporaryDeviceList<T> : DeviceList<T> where T : GenericDevice
    {
        public new void Add(T item)
        {
            if (this.Contains(item))
                RemoveAll(x => x.Equals(item));
            base.Add(item);
        }

        public void Discard(T device)
        {
            if (Contains(device))
                Remove(device);
        }
    }
}
