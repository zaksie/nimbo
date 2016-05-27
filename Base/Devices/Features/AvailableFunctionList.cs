using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Devices.Features
{
    public class AvailableFunctionList<T> : List<T> where T : AvailableFunction
    {
        public void Remove(byte code)
        {
            this.RemoveAll((x) => x.Code == code);
        }

        public AvailableFunction this[byte code]
        {
            get
            {
                var availableDeviceFunction = (from item in this
                                               where item.Code == code
                                               select item).ToArray();

                if (availableDeviceFunction.Length > 0)
                    return availableDeviceFunction[0];
                else
                    return null;
            }
        }

        public bool Contains(byte code)
        {
            return this[code] != null;
        }
    }

}
