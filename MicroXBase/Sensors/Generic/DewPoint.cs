using Base.Sensors;
using Base.Sensors.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroXBase.Sensors.Generic
{
    public class DewPoint : Base.Sensors.Types.Temperature.DewPoint
    {
        #region Constructors
        public DewPoint(ISensorManager i_Parent)
            :base(i_Parent)
        {
        }

        #endregion
    }
}
