using Base.Sensors;
using Base.Sensors.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroXBase.OpCodes.DataStructures
{
    public delegate void CalibrationAddedDelegate(eSensorIndex type, CalibrationCoefficient coeff);
    public class CalibrationConfiguration : Dictionary<eSensorIndex, CalibrationCoefficient>
    {
        public event CalibrationAddedDelegate OnCalibrationAdded;

        new public void Add(eSensorIndex index, CalibrationCoefficient coeff)
        {
            if (ContainsKey(index))
                Remove(index);

            base.Add(index, coeff);
            if (OnCalibrationAdded != null)
                OnCalibrationAdded(index, coeff);
        }
    }
}
