using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using Base.Devices;
using Base.Devices.Management;
using Maintenance;
using MicroXBase.Devices.Types;
using MicroXBase.Modules;
using System.ComponentModel.Composition;
using Maintenance.Communication;

namespace MicroXBase.Devices.Management
{
    [Export(typeof(IDeviceManager))]
    [ExportMetadata("Name", "MicroX Devices")]
    [Serializable]
    public class MicroXDeviceManager : GenericDeviceManager<MicroXDevice>, IDeviceManager, ISerializable
    {        
        #region Fields
        private object addRemoveLock = new object();
        #endregion
        #region Properties
        public override DeviceID[] DeviceIDs
        {
            get
            {
                return (from module in ModuleManager.Modules
                        from deviceid in module.DeviceIDs
                        select deviceid).Distinct().ToArray();
            }
        }
        #endregion
        #region Constructor
        public MicroXDeviceManager()
            : base()
        {
            ModuleManager.Start(this);
        }
        #endregion
        #region Methods
        public override void Subscribe<E>(ReportDeviceDelegate<E> reportMethod)
        {
            Debug.Assert(typeof(E).IsSubclassOf(typeof(GenericDevice)));

            foreach (var module in ModuleManager.Modules)
                if (module.DeviceType == typeof(E))
                    module.Subscribe<E>(reportMethod);
        }

        #endregion
        #region Remove Methods
        protected void TurnOffline(string UID)
        {
            lock (addRemoveLock)
            try
            {
                var devices = FindAll(x => x.UID == UID);
                foreach (var device in devices)
                {
                    tryToTurnOffline(device);
                }
            }
            catch (System.Exception ex)
            {
                Log.Write("Device [UID: " + UID + "] failed on remove", ex);
            }
        }

        private void tryToTurnOffline(MicroXDevice device)
        {
            try
            {
                //Remove(device);
                ReportDevice(this, device, DeviceStateEnum.REMOVED);
                (device as MicroXDevice).Dispose();
            }
            catch (Exception ex) { Log.Write(ex); }
        }
        #endregion
        #region Serialization & Deserialization
        //Deserialization constructor.
        public MicroXDeviceManager(SerializationInfo info, StreamingContext ctxt)
            :base(info,ctxt)
        {
        }

        //Serialization function.
        public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            base.GetObjectData(info, ctxt);
        }       
        #endregion

        #region IDeviceManager Methods
        public override DeviceList<GenericDevice> GetDeviceList()
        {
            return new DeviceList<GenericDevice>(this.Cast<GenericDevice>());
        }

        public override void OnDeviceFound(string UID, uint VID, uint PID, IDeviceIO deviceIO)
        {
            var module = ModuleManager.Modules.First(x => x.CanCreate(VID, PID));
            if (module != null)
                module.CreateFromAndReport(UID, VID, PID, deviceIO);
        }

        public override void OnDeviceRemoved(string uid)
        {
            TurnOffline(uid);
        }
        #endregion
    }
}
