using Base.Devices;
using Base.Sensors.Alarms;
using Base.Sensors.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Devices.Management;
using System.Reflection;
using Base.Sensors.Management;
using MicroXBase.Devices.Types;
using MicroXBase.OpCodes.DataStructures;

namespace MicroXBase.Sensors.Management
{
    public class MicroXSensorManager<T> : SensorManager<T> where T : GenericSensor
    {
        #region Properties
        new public MicroXDevice ParentLogger
        {
            get {return base.ParentLogger as MicroXDevice;}
            protected set {base.ParentLogger = value;}
        }

        public CalibrationConfiguration Calibration
        {
            get;
            protected set;
        }
        #endregion
        #region Constructors
        public MicroXSensorManager(GenericLogger parent)
            :base(parent)
        {
        }

        protected override void Initialize(GenericLogger parent)
        {
            base.Initialize(parent);
            Calibration = new CalibrationConfiguration();
        }
        #endregion
        #region Methods
        protected override void ParentLogger_OnStatusReceived(object sender, DeviceEventArgs<GenericDevice> e)
        {
            foreach (var entry in Calibration)
                this[entry.Key].Calibration.Set(entry.Value);

            foreach (var sensor in this)
                sensor.Initialize();

            ParentLogger.Configuration.
        }
        #endregion
    }
}
