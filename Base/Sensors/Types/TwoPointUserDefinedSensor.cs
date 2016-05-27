using Base.Sensors.Calibrations;
using Base.Sensors.Management;
using Base.Sensors.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Sensors.Types
{
    [Serializable]
    public class TwoPointUserDefinedSensor : GenericSensor, ISensor
    {
        #region Fields
        private eSensorType type;
        protected PredefinedSensor BaseSensor;
        protected TwoPoint Transformer = new TwoPoint();
        #endregion
        #region Properties
        public eSensorType Type
        {
            get { return type; }
            set
            {
                if (type != value)
                {
                    type = value;
                    reloadBaseSensor();
                }
            }
        }

        public string Name
        {
            get;
            set;
        }

        public virtual IUnit Unit
        {
            get;
            set;
        }
        public override double Maximum
        {
            get { return TransformFromBase(BaseSensor.Maximum); }
        }

        public override double Minimum
        {
            get { return TransformFromBase(BaseSensor.Minimum); }
        }

        public override bool USBRunnable
        {
            get { return BaseSensor.USBRunnable; }
        }

        public float Offset { get { return Convert.ToSingle(Transformer.Offset); } }
        public float Gain { get { return Convert.ToSingle(Transformer.Gain); } }
        #endregion
        #region Constructors
        public TwoPointUserDefinedSensor(ISensorManager parent)
            :base(parent)
        {

        }
        #endregion
        #region Methods
        public override long GetDigitalValue(double i_SensorValue)
        {
            return BaseSensor.GetDigitalValue(TransformToBase(i_SensorValue));
        }
        public override double GetSensorValue(long i_DigitalValue)
        {
            return TransformFromBase(BaseSensor.GetSensorValue(i_DigitalValue));
        }

        private void reloadBaseSensor()
        {
            if(ParentSensorManager != null)
                BaseSensor = ParentSensorManager.Create(Type);
        }

        public double TransformFromBase(double baseSensorValue)
        {
            return Transformer.Calibrate(baseSensorValue);
        }

        public double TransformToBase(double definedSensorValue)
        {
            return Transformer.Decalibrate(definedSensorValue);
        }

        public void Set(Tuple<decimal, decimal> definedValues, Tuple<decimal, decimal> originalValues)
        {
            Transformer.Set(new decimal[] { definedValues.Item1, definedValues.Item2 },
                new decimal[] { originalValues.Item1, originalValues.Item2 });
        }
        #endregion
        #region Override Methods
        public override string ToString()
        {
            return Name + " (" + Unit + ")";
        }

        #endregion
    }
}
