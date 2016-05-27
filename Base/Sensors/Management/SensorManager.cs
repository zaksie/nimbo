using Base.Devices;
using Base.Sensors.Alarms;
using Base.Sensors.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Devices.Management;
using System.Reflection;

namespace Base.Sensors.Management
{
    [Serializable]
    public abstract class SensorManager<T> : List<T>, ISensorManager where T : GenericSensor
    {
        #region Events
        public event SampleAddedDelegate OnSampleAdded;
        public event SampleAddedDelegate OnTimeStampAdded;
        #endregion
        #region Fields
        public SensorAlarmManager AlarmManager { get; protected set; }
        #endregion
        #region Properties
        public bool FahrenheitMode
        {
            get { return ParentLogger.Status.FahrenheitMode; }
        }
        public GenericLogger ParentLogger
        {
            get;
            protected set;
        }
        public bool HasData
        {
            get
            {
                foreach (GenericSensor sensor in this)
                {
                    if (!sensor.IsEmpty)
                    {
                        return true;
                    }
                }

                return false;
            }
        }
        public IEnumerable<GenericSensor> ActiveSensors
        {
            get
            {
                return from sensor in this
                       where sensor.Enabled
                       select sensor;
            }
        }

        #endregion
        #region Constructors
        public SensorManager(GenericLogger parent)
        {
            Initialize(parent);
        }

        protected virtual void Initialize(GenericLogger parent)
        {
            ParentLogger = parent;
            AlarmManager = new SensorAlarmManager(this);
            ParentLogger.OnStatusReceived += ParentLogger_OnStatusReceived;
        }

        #endregion
        #region New Methods
        protected new void Add(T item)
        {
            base.Add(item);
            AlarmManager.RegisterForStatusChange(item.Alarm);
        }

        protected new void Remove(T item)
        {
            base.Remove(item);
            AlarmManager.UnregisterForStatusChange(item.Alarm);
        }
        
        #endregion
        #region Methods
        protected abstract void ParentLogger_OnStatusReceived(object sender, DeviceEventArgs<GenericDevice> e);
    
        public void SampleAdded(GenericSensor genericSensor, Samples.Sample sample)
        {
            if (OnSampleAdded != null)
                OnSampleAdded(ParentLogger, new SampleAddedEventArgs { Logger = ParentLogger, Sensor = genericSensor, Sample = sample });
        }

        public void TimeStampAdded(GenericSensor genericSensor, Samples.Sample sample)
        {
            if (OnTimeStampAdded != null)
                OnTimeStampAdded(ParentLogger, new SampleAddedEventArgs { Logger = ParentLogger, Sensor = genericSensor, Sample = sample });
        }

        public void InitializeSensors()
        {
            this.AddRange(CreateFixedSensors());
        }

        public GenericSensor[] GetSensors()
        {
            return this.ToArray();
        }

        public T this[eSensorType type]
        {
            get
            {
                return this.Find(x => (x as ISensor).Type == type);
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public T this[eSensorIndex index]
        {
            get
            {
                return this.Find(x => x.Index == index);
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public SensorAlarmManager GetAlarmManager()
        {
            return AlarmManager;
        }


        public IEnumerable<T> CreateFixedSensors()
        {
            foreach (var sensorType in ParentLogger.AvailableFixedSensors)
                yield return Create(sensorType) as T;
        }

        public IEnumerable<T> CreateDetachableSensors()
        {
            foreach (var sensorType in ParentLogger.AvailableDetachableSensors)
                yield return Create(sensorType) as T;
        }

        public PredefinedSensor Create(eSensorType sensorType)
        {
            Type t = GetSensorClassType(sensorType);
            return Activator.CreateInstance(t, this) as PredefinedSensor;
        }
        
        protected virtual Type GetSensorClassType(eSensorType sensorType)
        {
            Assembly asm = ParentLogger.GetType().Assembly;
            string fullname = ParentLogger.GetType().Namespace;
            return asm.GetType(fullname.Substring(0, fullname.IndexOf('.') + 1) + STRUCTURE.SENSORS + "." + sensorType.ToString());
        }
        #endregion
    }
}
