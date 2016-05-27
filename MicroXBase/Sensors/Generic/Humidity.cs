using Base.Sensors;
using Base.Sensors.Management;
using Base.Sensors.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroXBase.Sensors.Generic
{
    public class Humidity : Base.Sensors.Types.Humidity
    {
        #region Constructors
        public Humidity(ISensorManager i_Parent)
            : base(i_Parent)
        {
        }

        #endregion
        #region Implemented Methods
        public override long GetDigitalValue(double i_SensorValue)
        {
            long digitalValue;

            if (i_SensorValue < 57.769532f)
            {
                digitalValue = Convert.ToInt32((i_SensorValue * 4096 + 8192) / 143);
            }
            else
            {
                digitalValue = Convert.ToInt32((i_SensorValue * 4096 - 46288) / 111);
            }

            return digitalValue;
        }

        public override double GetSensorValue(long i_DigitalValue)
        {
            double sensorValue;

            if (i_DigitalValue <= 1712)
            {
                sensorValue = (143 * i_DigitalValue - 8192) / 4096.0;
            }
            else
            {
                sensorValue = (111 * i_DigitalValue + 46288) / 4096.0;
            }

            return sensorValue;
        }

        #endregion
    }
}
