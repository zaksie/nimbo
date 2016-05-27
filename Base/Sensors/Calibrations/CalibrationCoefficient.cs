using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.Sensors
{
    [Serializable]
    public class CalibrationCoefficient
    {
        public float A, B, C;

        public Decimal Am { get { return Convert.ToDecimal(A); } }
        public Decimal Bm { get { return Convert.ToDecimal(B); } }
        public Decimal Cm { get { return Convert.ToDecimal(C); } }

        public override bool Equals(object obj)
        {
            CalibrationCoefficient other = obj as CalibrationCoefficient;
            if (other == null)
                return false;

            return A == other.A
                && B == other.B
                && C == other.C;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
