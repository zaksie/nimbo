using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Reflection;
using System.Drawing;
using Base.Sensors;
using Base.Devices;
using Base.Sensors.Types;
using System.Runtime.Serialization;
using Maintenance.Communication;

namespace Base.Devices.Management
{

    [Serializable]
    public abstract class GenericDeviceManager<T> : DeviceList<T>, IDeviceManager, ISerializable where T:GenericDevice
    {
        #region Events
        public event ReportGenericDeviceDelegate OnReportDevice;
        #endregion
        #region Properties
        public abstract DeviceID[] DeviceIDs { get; }
        #endregion
        #region Constructor
        public GenericDeviceManager()
        {

        }
        #endregion
        #region Methods
        protected void ReportDevice(object sender, GenericDevice device, DeviceStateEnum state)
        {
            if (OnReportDevice != null)
                OnReportDevice(sender, new DeviceEventArgs<GenericDevice> { Device = device, State = state });
        }
        #endregion
        #region Search
        public virtual PropertyInfo[] SearchableProperties
        {
            get
            {
                int Index = 0;
                PropertyInfo[] PropertyInfos = new PropertyInfo[SearchablePropertyNames.Count()];
                foreach (string PropertyName in SearchablePropertyNames)
                    PropertyInfos[Index++] = typeof(GenericDevice).GetProperty(PropertyName);
                return PropertyInfos;
            }
        }

        public virtual string[] SearchablePropertyNames
        {
            get { return new string[] { "Comment", "SerialNumber", "DeviceTypeName", "FirmwareVersion", "BatteryLevel" }; }
        }
        #endregion
        #region IDeviceManager Methods
        public abstract DeviceList<GenericDevice> GetDeviceList();
        public abstract void OnDeviceFound(string UID, uint VID, uint PID, IDeviceIO deviceIO);
        public abstract void OnDeviceRemoved(string UID);
        public abstract void Subscribe<E>(ReportDeviceDelegate<E> reportMethod) where E : GenericDevice;
        #endregion
        #region Serialization & Deserialization
        //Deserialization constructor.
        public GenericDeviceManager(SerializationInfo info, StreamingContext ctxt)
        {
        }
        //Serialization function.
        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
        }       
        #endregion
    }
}
