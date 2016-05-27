using Base.Sensors;
using Base.Sensors.Management;
using Base.Sensors.Samples;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroXBase.Sensors.Generic
{
    public abstract class GenericNTC_V1 : Base.Sensors.Types.Temperature.ExternalNTC
    {
        #region Fields
        public abstract UInt16[] ValueTable { get; }
        public abstract double[] MultiplicationTable { get; }
        public abstract ushort Zero { get; }
        public override bool USBRunnable
        {
            get { return true; }
        }
        #endregion
        #region Constructors
        public GenericNTC_V1(ISensorManager i_Parent)
            : base(i_Parent)
        {
        }

        #endregion    
        #region Methods
        protected double GetSensorValueUsingTables(long i_DigitalValue)
        {
            int i;
            long diff;
            uint A, B;
            long fraction = 0, integer = 0;

            if (i_DigitalValue < ValueTable[0])
            {
                i_DigitalValue = ValueTable[0];
            }
            else if (i_DigitalValue > ValueTable[ValueTable.Length - 1])
            {
                i_DigitalValue = ValueTable[ValueTable.Length - 1];
            }

            for (i = 0; i < ValueTable.Length; i++)
            {
                B = ValueTable[i];

                if (i_DigitalValue < B)
                {
                    A = ValueTable[i - 1];
                    diff = B - A;

                    if (i <= Math.Abs(Minimum))
                    {
                        integer = Convert.ToInt16(i - Math.Abs(Minimum) - 1);
                        if (diff > B - i_DigitalValue)
                        {
                            A = GetNTCDiffTableValue(diff);
                            diff = B - i_DigitalValue;
                            fraction = Convert.ToInt16(diff * A);
                            fraction /= 10;
                            if (fraction > 999)
                                fraction = 999;
                            fraction = 1000 - fraction;
                        }
                        else
                        {
                            fraction = 0;
                        }
                    }
                    else
                    {
                        integer = Convert.ToInt16(i - Math.Abs(Minimum) - 1);
                        B = GetNTCDiffTableValue(diff);
                        diff = i_DigitalValue - A;
                        fraction = Convert.ToInt16(diff * B);
                        fraction /= 10;
                        if (fraction > 999) fraction = 999;

                    }
                    break;
                }
            }

            float fFraction = ((int)(fraction / 10)) / 100f;
            float Value = integer;
            Value += fFraction;

            return Value;

        }

        protected UInt16 GetDigitalValueUsingTables(double i_SensorValue)
        {
            UInt16 result;
            double processedSensorValue = i_SensorValue * 100;
            short arrayPosition = Convert.ToInt16((processedSensorValue + 4000) / 100f);
            if (processedSensorValue == 0)
                result = 0;
            else if (processedSensorValue < 0)
            {

                double delta = (-processedSensorValue) % 100 * 100 / MultiplicationTable[arrayPosition];
                int shift = delta > 0 ? 1 : 0;
                result = Convert.ToUInt16(ValueTable[arrayPosition + shift] - delta);
            }
            else
            {
                double delta = processedSensorValue % 100;
                delta = delta * (100f / MultiplicationTable[arrayPosition]);
                result = Convert.ToUInt16(ValueTable[arrayPosition] + delta);
            }

            return result;
        }

        private uint GetNTCDiffTableValue(long Diff)
        {
            if (Diff < 0)
                throw new Exception("Diff cannot be 0");
            else if (Diff == 0)
                return 0;
            else
                return (uint)((float)(1f / Diff) * 10000);
        }

        #endregion
        #region Implemented Methods
        public override long GetDigitalValue(double i_SensorValue)
        {
            return GetDigitalValueUsingTables(i_SensorValue);
        }

        public override double GetSensorValue(long i_DigitalValue)
        {
            return GetSensorValueUsingTables(i_DigitalValue);
        }

        #endregion
    }
}
