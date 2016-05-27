using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Devices.Management;
using MicroXBase.Devices.Types;

namespace MicroXBase.Modules
{
    public abstract class GenericSubDeviceInterfacer<T> : GenericDeviceInterfacer<T>, ISubDeviceInterfacer where T : MicroXDevice
    {
        public GenericSubDeviceInterfacer()
            :base()
        {

        }
        public override bool CanCreate(uint vid, uint pid)
        {
            return false;
        }

        public override void CreateFromAndReport(string uid, uint vid, uint pid, Maintenance.Communication.IDeviceIO deviceIO)
        {
            throw new NotSupportedException();
        }
    }
}
