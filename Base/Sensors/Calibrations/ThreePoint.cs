using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuxiliaryLibrary.MathLib;
using Base.Sensors.Types;
using System.Runtime.Serialization;

namespace Base.Sensors.Calibrations
{
    [Serializable]
    public abstract class ThreePoint : CalibrationInformation, ISerializable
    {
        #region Fields
        protected abstract decimal DEFAULT_COEFF_C { get; }
        public override int Points { get { return 3; } }
        #endregion
        #region Constructors
        public ThreePoint()
        {

        }

        #endregion
        #region Properties
        public Decimal C
        {
            get;
            set;
        }

        public Single Cf
        {
            get { return Convert.ToSingle(C); }
        }

        #endregion
        #region Abstract Methods
        protected abstract decimal InverseFunction(decimal x, bool defaultCoefficients);
        #endregion
        #region Public Methods
        public override void Reset()
        {
            A = DEFAULT_COEFF_A;
            B = DEFAULT_COEFF_B;
            C = DEFAULT_COEFF_C;
        }

        public override void Set(CalibrationCoefficient coeff)
        {
            A = coeff.Am;
            B = coeff.Bm;
            C = coeff.Cm;
        }
        protected override void InitializeValues()
        {
            A = B = C = Gain = 0;
            Offset = 0;
        }
        #endregion
        #region Private Methods
        protected decimal[] Celsius2InvertedKelvin(decimal[] values)
        {
            List<decimal> results = new List<decimal>();
            foreach (var val in values)
            {
                results.Add(1 / (val + GenericSensor.KELVIN_0));
            }

            return results.ToArray();
        }

        protected decimal[] ExtractRValues(decimal[] refs)
        {
            List<decimal> results = new List<decimal>();
            foreach (var val in refs)
            {
                results.Add(Convert.ToDecimal(InverseFunction(val, true)));
            }
            return results.ToArray();
        }

        protected void RecalculateCoefficients(decimal[] Rs, double[] refs)
        {
            List<double[]> data = new List<double[]>();
            data.Add(GetFunctionVector(Rs[0]));
            data.Add(GetFunctionVector(Rs[1]));
            data.Add(GetFunctionVector(Rs[2]));

            Matrix m = new Matrix(data.ToArray());
            Vector v = new Vector(refs);
            Matrix solution = m.Solve(v);

            A = Convert.ToDecimal(solution[0, 0]);
            B = Convert.ToDecimal(solution[1, 0]);
            C = Convert.ToDecimal(solution[2, 0]);
        }

        private double[] GetFunctionVector(decimal r)
        {
            return new double[]{
                1, 
                Math.Log(Convert.ToDouble(r)), 
                Math.Pow(Math.Log(Convert.ToDouble(r)), 3)
            };
        }
        #endregion
        #region Serialization & Deserialization
        public ThreePoint(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {
            C = (Decimal)AuxiliaryLibrary.Tools.IO.GetSerializationValue<Decimal>(ref info, "C");
        }

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("C", C, typeof(Decimal));
        }

        #endregion
    }
}
