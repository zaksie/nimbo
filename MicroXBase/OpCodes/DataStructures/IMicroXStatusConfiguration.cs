using Base.Devices.Components;
using Base.OpCodes.Helpers;
using Base.Sensors.Types;
using Base.Sensors.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroXBase.OpCodes.DataStructures
{
    public interface IMicroXStatusConfiguration : IMicroXConfiguration
    {
        byte DeviceType { get; set; }
        float FirmwareVersion { get; set; }
        byte PCBAssembly { get; set; }
        byte BatteryLevel { get; set; }
        bool Running { get; set; }
    }
}
