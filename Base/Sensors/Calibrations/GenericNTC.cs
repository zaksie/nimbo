using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuxiliaryLibrary.MathLib;
using Base.Sensors.Types;

namespace Base.Sensors.Calibrations.NTC
{
    [Serializable]
    public abstract class GenericNTC : ThreePoint
    {
        #region Properties
        public override bool Valid
        {
            get { return A != 0; }
        }
        #endregion
        #region Methods
        public override void Set(decimal[] refs, decimal[] values)
        {
            decimal[] Rs = ExtractRValues(values);
            double[] invertedKelvin = Array.ConvertAll(Celsius2InvertedKelvin(refs), x => (double)x);
            RecalculateCoefficients(Rs, invertedKelvin);
        }
        
        protected override decimal InverseFunction(decimal celsiusTemperature, bool defaultCoefficients)
        {
            decimal a = defaultCoefficients ? DEFAULT_COEFF_A : A;
            decimal b = defaultCoefficients ? DEFAULT_COEFF_B : B;
            decimal c = defaultCoefficients ? DEFAULT_COEFF_C : C;
            
            decimal temperature = GenericSensor.KELVIN_0 + celsiusTemperature;
            
            //The following was constructed using the Wikipedia description 
            //of the inverse of the Steinhart-Hart equation
            decimal y = (a - 1 / temperature) / c;
            decimal x = MathEx.Sqrt(MathEx.Pow(b / (3 * c), 3) + MathEx.Pow(y / 2m, 2));
            decimal result = MathEx.Exp(MathEx.Pow(x - y / 2m, 1 / 3m) - MathEx.Pow(x + y / 2m, 1 / 3m));

            return result;
        }
        #endregion
    }
}
