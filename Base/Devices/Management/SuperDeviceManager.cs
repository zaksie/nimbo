using Base.Modules;
using Communication;
using Maintenance.Communication;
using Maintenance.Firmware;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Base.Devices.Management
{
    public sealed partial class SuperDeviceManager
    {
        #region Fields
        private object moduleDeviceReportLock = new object();
        #endregion
        #region Properties
        private Dictionary<IDeviceManagerMetadata, IDeviceManager> modules = new Dictionary<IDeviceManagerMetadata, IDeviceManager>();
        private IReadOnlyCollection<IDeviceManager> Modules
        {
            get { return new ReadOnlyCollection<IDeviceManager>(modules.Values.ToArray()); }
        }
        internal static readonly SuperDeviceManager instance = new SuperDeviceManager();
        #endregion
        #region Constructors        
        #endregion
        #region Methds
        private void RegisterDeviceIDs()
        {
            foreach (var module in modules)
                CommunicationManager.Instance.RegisterDeviceID(module.Value.DeviceIDs);
        }

        private void UnregisterDeviceIDs()
        {
            foreach (var module in modules)
                CommunicationManager.Instance.UnregisterDeviceID(module.Value.DeviceIDs);
        }

        private void onDeviceFound(string UID, uint VID, uint PID, IDeviceIO deviceIO)
        {
            foreach (var module in Modules)
                module.OnDeviceFound(UID, VID, PID, deviceIO);
        }

        private void onDeviceRemoved(string UID)
        {
            foreach (var module in Modules)
                module.OnDeviceRemoved(UID);
        }

        internal void Add(IDeviceManager module, IDeviceManagerMetadata metadata)
        {
            if (!this.modules.ContainsKey(metadata))
            {
                module.OnReportDevice += module_ReportDeviceEvent;
                modules.Add(metadata, module);
            }
        }

        void module_ReportDeviceEvent(object sender, DeviceEventArgs<GenericDevice> e)
        {
            lock (moduleDeviceReportLock)
                switch (e.State)
                {
                    case DeviceStateEnum.FOUND:
                        if (OnDeviceConnected != null)
                            OnDeviceConnected(sender, e);
                        break;
                    case DeviceStateEnum.REMOVED:
                        if (OnDeviceRemoved != null)
                            OnDeviceRemoved(sender, e);
                        break;
                }
        }

        #endregion
    }
}