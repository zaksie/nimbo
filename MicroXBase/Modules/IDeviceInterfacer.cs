using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Base.Devices;
using Base.Devices.Management;
using MicroXBase.Devices.Types;
using Maintenance.Communication;

namespace MicroXBase.Modules
{
    public interface IDeviceInterfacer
    {
        DeviceID[] DeviceIDs { get; }
        Type DeviceType { get; }

        bool CanCreate(uint vid, uint pid);
        void CreateFromAndReport(string uid, uint vid, uint pid, IDeviceIO deviceIO);

        bool CanCreateFrom(MicroXDevice genericDevice);
        void CreateFromAndReport(MicroXDevice genericDevice);

        void Subscribe<E>(ReportDeviceDelegate<E> reportMethod) where E : GenericDevice;
    }
}
