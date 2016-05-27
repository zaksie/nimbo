using Base.Sensors.Management;
using Base.Sensors.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Sensors.Types
{
    interface ISensor
    {
        eSensorType Type { get; }
        string Name { get; }
        IUnit Unit { get; }
    }
}
