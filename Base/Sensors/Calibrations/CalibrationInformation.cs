using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuxiliaryLibrary.MathLib;
using System.Runtime.Serialization;

namespace Base.Sensors
{
    [Serializable]
    public abstract class CalibrationInformation : ISerializable
    {
        #region Fields
        protected abstract decimal DEFAULT_COEFF_A
        { get; }
        protected abstract decimal DEFAULT_COEFF_B
        { get; }
        public abstract int Points { get; }
        #endregion
        #region Properties
        public Decimal A
        {
            get;
            set;
        }
        public Decimal B
        {
            get;
            set;
        }

        public Single Af
        {
            get { return Convert.ToSingle(A); }
        }
        public Single Bf
        {
            get { return Convert.ToSingle(B); }
        }

        public float GainFactor
        {
            get { return 1000f; }
        }

        public decimal Gain
        {
            get { return A; }
            set { A = value; }
        }

        public decimal Offset
        {
            get { return B; }
            set { B = value; }
        }
        public abstract bool Valid
        {
            get;
        }
        #endregion
        #region Constructors
        public CalibrationInformation()
        {
            InitializeValues();
        }

        protected virtual void InitializeValues()
        {
            A = B = Gain = 0;
            Offset = 0;
        }
        #endregion
        #region Abstract Methods
        public abstract double Calibrate(double SensorValue);
        public abstract double Decalibrate(double SensorValue);
        public abstract void Set(decimal[] refs, decimal[] values);
        public abstract void Reset();
        public abstract void Set(CalibrationCoefficient coeff);
        #endregion
        #region Serialization & Deserialization
        public CalibrationInformation(SerializationInfo info, StreamingContext ctxt)
        {
            A = (Decimal)AuxiliaryLibrary.Tools.IO.GetSerializationValue<Decimal>(ref info, "A");
            B = (Decimal)AuxiliaryLibrary.Tools.IO.GetSerializationValue<Decimal>(ref info, "B");
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("A", A);
            info.AddValue("B", B);
        }

        #endregion
    }
}
