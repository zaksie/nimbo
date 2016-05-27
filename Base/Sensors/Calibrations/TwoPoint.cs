using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Base.Sensors.Calibrations
{
    [Serializable]
    public class TwoPoint : CalibrationInformation, ISerializable
    {
        #region Fields
        public override int Points { get { return 2; } }
        #endregion
        #region Constructors
        public TwoPoint()
        {

        }
        #endregion
        #region Properties
        protected override decimal DEFAULT_COEFF_A
        {
            get { return 1; }
        }

        protected override decimal DEFAULT_COEFF_B
        {
            get { return 0; }
        }

        public override bool Valid
        {
            get { return Gain != 0; }
        }
        #endregion
        #region Methods
        public override void Reset()
        {
            A = DEFAULT_COEFF_A;
            B = DEFAULT_COEFF_B;
        }
        public override void Set(CalibrationCoefficient coeff)
        {
            A = coeff.Am;
            B = coeff.Bm;
        }
        public override void Set(decimal[] refs, decimal[] values)
        {
            decimal gain = (refs[0] - refs[1]) / (values[0] - values[1]);
            decimal offset = gain * (-values[0]) + refs[0];

            A = gain;
            B = offset;
        }

        public override double Calibrate(double SensorValue)
        {
            return (double)((decimal)SensorValue * Gain + Offset);
        }

        public override double Decalibrate(double SensorValue)
        {
            return (double)(((decimal)SensorValue - Offset) / Gain);
        }

        #endregion
        #region Serialization & Deserialization
        public TwoPoint(SerializationInfo info, StreamingContext context)
            :base(info, context)
        {

        }

        public new void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            
        }
        #endregion
    }
}
