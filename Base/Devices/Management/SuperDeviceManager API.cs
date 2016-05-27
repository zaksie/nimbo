using Base.Modules;
using Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Base.Devices.Management
{
    //Here go all the publicly exposed functionalities of SuperDeviceManager
    public partial class SuperDeviceManager : ISearchableDeviceList
    {
        #region Events
        public static event ReportGenericDeviceDelegate OnDeviceConnected;
        public static event ReportGenericDeviceDelegate OnDeviceRemoved;
        #endregion
        #region Properties
        public static int Count
        {
            get 
            {
                throwIfNotStarted();

                return GetDevices().Count(); 
            }
        }

        public static int OnlineDeviceCount
        {
            get
            {
                throwIfNotStarted();

                return (from GenericDevice device in GetDevices()
                        where !device.Offline
                        select device).Count();
            }
        }
        public static bool Started { get; private set; }
        #endregion
        #region Methods
        public static void Start()
        {
            ModuleManager.Instance.UpdateDeviceManager();
            instance.RegisterDeviceIDs();
            CommunicationManager.OnDeviceConnected += instance.onDeviceFound;
            CommunicationManager.OnDeviceRemoved += instance.onDeviceRemoved;
            CommunicationManager.ScanForDevices();
            Started = true;
        }

        public static void Shutdown()
        {
            throwIfNotStarted();

            Started = false;
            instance.UnregisterDeviceIDs();
            CommunicationManager.OnDeviceConnected -= instance.onDeviceFound;
            CommunicationManager.OnDeviceRemoved -= instance.onDeviceRemoved;
        }
        public static void ScanForDevice()
        {
            throwIfNotStarted();

            CommunicationManager.ScanForDevices();
        }

        public static IEnumerable<GenericDevice> GetDevices()
        {
            throwIfNotStarted();

            foreach (var module in instance.Modules)
            {
                foreach (GenericDevice device in module.GetDeviceList())
                    yield return device;
            }
        }

        public static IEnumerable<T> GetDevicesOfType<T>()
        {
            throwIfNotStarted();

            foreach (var module in instance.Modules)
            {
                var selected = module.GetDeviceList().OfType<T>();
                foreach (T device in selected)
                    yield return device;
            }
        }
        public static T FindDevice<T>(string serialNumber) where T : GenericDevice, ISerializable
        {
            throwIfNotStarted();

            var devices = instance.FindDevices<T>(serialNumber);
            if (devices.Count() > 0)
                return devices[0];
            else
                return null;
        }
        #endregion
        #region Subscribe Methods
        public static void Subscribe<T>(ReportDeviceDelegate<T> reportMethod) where T : GenericDevice
        {
            throwIfNotStarted();

            foreach (var module in instance.Modules)
                module.Subscribe<T>(reportMethod);
        }

        private static void throwIfNotStarted()
        {
            if (!Started)
                throw new InvalidOperationException("SuperDeviceManager has not been started yet");
        }
        #endregion
        #region ISearchableDeviceList
        public T[] FindDevices<T>(string serialNumber) where T : GenericDevice, ISerializable
        {
            var list = GetDevicesOfType<T>().ToList();
            return list.FindAll(x => x.Status.SerialNumber == serialNumber).ToArray();
        }

        #endregion
    }
}
