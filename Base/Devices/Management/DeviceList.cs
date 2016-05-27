using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ComponentModel;
using Base.Sensors;
using Base.Devices;
using Base.Devices.Features;
using Base.Devices.Management;
using Maintenance;

namespace Base.Devices.Management
{

    [Serializable]
    public class DeviceList<T> : List<T>, ISearchableDeviceList where T : GenericDevice, ISerializable
    {
        #region Fields and Delegates
        public event ReportDeviceDelegate<T> OnDeviceAddedToListEvent = null;
        #endregion
        #region Properties
        #endregion
        #region Constructors
        public DeviceList()
        { }
        public DeviceList(IEnumerable<T> devices)
            :base(devices)
        {
        }
        public DeviceList(SerializationInfo info, StreamingContext ctxt)
        { }
        #endregion
        #region Search Methods
        public E[] FindDevices<E>(string SerialNumber) where E : GenericDevice, ISerializable
        {
            return (from device in this
                        where device.Status.SerialNumber == SerialNumber && device is E
                        select device as E).ToArray();
        }
        #endregion
        #region Add/Remove/Merge/etc. Methods
        public new void Add(T t)
        {
            if (!this.Contains(t))
            {
                if (OnDeviceAddedToListEvent != null)
                    OnDeviceAddedToListEvent(this, new DeviceEventArgs<T>{Device = t as T});

                base.Add(t);
            }
        }
        public void AddRange(IEnumerable<GenericDevice> collection)
        {
            base.AddRange(collection.OfType<T>());
        }

        /// <summary>
        /// This checks where Device of type GenericDevice existing by matching it against other devices using its Type and Serial Number
        /// </summary>
        /// <param name="Device">Device to look for</param>
        /// <returns></returns>
        public bool Contains(GenericDevice device)
        {
            try
            {
                return Find(x => x.Status.SerialNumber == device.Status.SerialNumber) != null;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region Methods
        public void Replace(T ExistingDevice, T DeviceInfo)
        {
            if (this.Contains(ExistingDevice))
            {
                this.Remove(ExistingDevice);
                this.Add(DeviceInfo);
            }
        }
        public bool Contains(string UID)
        {
            try
            {
                T t = this[UID];
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        #region Operator overload
        public T this[string UID]
        {
            get
            {
                return (from device in this
                        where device.UID == UID
                        select device).ToArray()[0];
            }
        }
        #endregion
    }
}
