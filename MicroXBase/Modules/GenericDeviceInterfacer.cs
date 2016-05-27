using Base.Devices;
using Base.Devices.Management;
using MicroXBase.Devices.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Maintenance.Communication;
using System.ComponentModel.Composition;
using Maintenance;

namespace MicroXBase.Modules
{
    public abstract class GenericDeviceInterfacer<T> : IDeviceInterfacer where T : MicroXDevice
    {
        #region Events
        public event ReportDeviceDelegate<T> OnDeviceConnected;
        public event ReportDeviceDelegate<T> OnDeviceRemoved;
        #endregion
        #region Properties
        public Type DeviceType { get { return typeof(T); } }
        public abstract DeviceID[] DeviceIDs { get; }
 
        protected abstract Version MinimumRequiredFirmwareVersion { get; }
        protected abstract byte Type { get; }
        protected abstract byte Bits { get; }

        [Import]
        protected IDeviceManager ParentDeviceManager;

        #endregion
        #region Constructors
        public GenericDeviceInterfacer()
        {
        }
        #endregion
        #region Report Methods
        public void Subscribe<E>(ReportDeviceDelegate<E> reportMethod) where E : GenericDevice
        {
            var handler = reportMethod as ReportDeviceDelegate<T>;
            if (handler != null)
                OnDeviceConnected += handler;
        }
        #endregion
        #region CreateNew Methods
        public virtual bool CanCreate(uint vid, uint pid)
        {
            return DeviceIDs.Any(x => x.VID == vid && x.PID == pid);
        }

        public abstract void CreateFromAndReport(string uid, uint vid, uint pid, IDeviceIO deviceIO);

        public virtual bool CanCreateFrom(MicroXDevice device)
        {
            return device.Configuration.DeviceType == Type
                && device.Version.FirmwareVersion >= MinimumRequiredFirmwareVersion;
        }

        public virtual void CreateFromAndReport(MicroXDevice genericDevice)
        {
            if (!CanCreateFrom(genericDevice))
                throw new ArgumentException("Device cannot be created using this interfacer", "device");

            Type type = lookUpCorrectType(genericDevice);
            T device = (T)Activator.CreateInstance(type, ParentDeviceManager);
            InitializeDevice(device, genericDevice);
            ReportDevice(device, DeviceStateEnum.FOUND);
        }
        #endregion
        #region AttachToExisting Methods
        public bool AttachToExistingDevice(MicroXDevice microxDevice)
        {
            T existingDevice = checkForExistingDevice(microxDevice);

            if (existingDevice != null)
            {
                existingDevice.Initialize();
                InitializeDevice(existingDevice, microxDevice);
                ReportDevice(existingDevice, DeviceStateEnum.RECONNECTED);
                return true;
            }
                
            return false;
        }

        private T checkForExistingDevice(MicroXDevice newDevice)
        {
            if (newDevice != null)
            {
                var existingDevices = ParentDeviceManager.FindDevices<T>(newDevice.Status.SerialNumber);

                foreach (T existingDevice in existingDevices)
                    if (existingDevice.IsOfSameType(newDevice))
                        if (existingDevice.Offline)
                            return existingDevice;
                        else
                            throwOnMultipleSerialNumbers();
            }

            return null;
        }
        #endregion
        #region Protected Methods
        protected void ReportDevice(MicroXDevice device, DeviceStateEnum state)
        {
            if (!(device is T))
                return;

            var e = new DeviceEventArgs<T>
            {
                Device = device as T,
                State = state
            };

            switch (state)
            {
                case DeviceStateEnum.RECONNECTED:
                case DeviceStateEnum.FOUND:
                    if (OnDeviceConnected != null)
                        OnDeviceConnected(this, e);
                    break;
                case DeviceStateEnum.REMOVED:
                    if (OnDeviceRemoved != null)
                        OnDeviceRemoved(this, e);
                    break;
                default:
                    break;
            }
        }
        #endregion
        #region Private Methods
        private static void throwOnMultipleSerialNumbers()
        {
            Exception ex = new Exception("A device by that serial number already exists");
            Log.Write(ex);
            throw ex;
        }
        private void killOldDevice(MicroXDevice device)
        {
            device.Dispose();
        }
        private void hookUpDevice(MicroXDevice newDevice, MicroXDevice genericDevice)
        {
            newDevice.Offline = false;
            newDevice.InitializeInterfacer(genericDevice.UID, genericDevice.OpCodes.DeviceIO);
            newDevice.GetStatus();
        }
        private void InitializeDevice(
            MicroXDevice newDevice,
            MicroXDevice genericDevice)
        {
            hookUpDevice(newDevice, genericDevice);
            killOldDevice(genericDevice);
        }
        protected virtual bool checkPrerequisites(MicroXDevice genericDevice)
        {
            return MinimumRequiredFirmwareVersion <= genericDevice.Version.FirmwareVersion
                && genericDevice.Configuration.PCBAssembly == Bits
                && genericDevice.Configuration.DeviceType == Type;
        }

        private Type lookUpCorrectType(MicroXDevice device)
        {
            Assembly asm = this.GetType().Assembly;
            var candidates = (from type in asm.GetTypes()
                              where isOfType(type) 
                              && isOfSubtype(type, device.Configuration.DeviceType)
                              && isVersionCompatible(type, device.Version.FirmwareVersion)
                              select type).ToArray();
            return candidates.OrderByDescending(t => getMinFirmwareVersion(t)).First();
        }

        private bool isVersionCompatible(Type type, Version version)
        {
            return version >= getMinFirmwareVersion(type);
        }

        private Version getMinFirmwareVersion(Type type)
        {
            return type.GetField(STRUCTURE.MIN_FW_VERSION, BindingFlags.Static | BindingFlags.Public).GetValue(null) as Version;
        }

        private bool isOfType(Type type)
        {
            return type.IsSubclassOf(typeof(T));
        }

        private bool isOfSubtype(Type type, byte subtype)
        {
            return type.IsSubclassOf(GetSubType(subtype));            
        }

        protected virtual Type GetSubType(byte subtype)
        {
            return typeof(T);
        }
        #endregion
    }
}
