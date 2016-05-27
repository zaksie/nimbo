using Base.Devices.Components;
using Base.Devices.Management;
using Base.OpCodes.Helpers;
using Base.OpCodes.Management;
using Base.Sensors.Types;
using Base.Sensors.Management;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;

namespace Base.Devices
{
    [Serializable()]
    public abstract class GenericLogger : GenericDevice, ISerializable
    {
        #region Static
        public static TimeSpan GetLoggingDuration(UInt32 SamplingInterval, int ActiveSensors, int SampleCapacity)
        {
            long SecondsFactor = Convert.ToInt64(Math.Pow(10, 7));
            return new TimeSpan(SecondsFactor * (long)((SamplingInterval * SampleCapacity) / (float)ActiveSensors));
        }
        #endregion
        #region Enums
        public enum eVersion
        {
            V1,
            V2
        }
        #endregion
        #region Fields
        public DateTime DownloadStartTime = DateTime.MinValue;
        public SensorManager<GenericSensor> Sensors { get; protected set; }
        #endregion
        #region Properties
        public abstract eSensorType[] AvailableFixedSensors { get; }
        public abstract eSensorType[] AvailableDetachableSensors { get; }
        protected abstract string SensorTypeLocation { get; }
        public abstract bool IsDownloading { get; }
       
        public virtual UInt16 MinimumInterval
        {
            get { return 1; }
        }
        #endregion
        #region Constructors
        public GenericLogger(IDeviceManager parent)
            : base(parent)
        {
        }

        public override void Initialize()
        {
            Sensors = new SensorManager<GenericSensor>(this);
        }
        protected override void Initialize(IDeviceManager parent)
        {
            base.Initialize(parent);
            Initialize();
        }
        #endregion
        #region Device Functionalities
        public abstract Result RestoreDefaultCalibration();
        public abstract Result ResetCalibration();
        public abstract Result Download();
        public abstract Result CancelDownload();

        #endregion
        #region Serialization & Deserialization
        //Deserialization constructor.
        public GenericLogger(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {

        }

        //Serialization function.
        public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);

        }
        #endregion
    }
}
