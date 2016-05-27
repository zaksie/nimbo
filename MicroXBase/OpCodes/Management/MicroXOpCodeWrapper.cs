using MicroXBase.OpCodes.Common;
using Base.OpCodes.Management;
using Maintenance;
using Maintenance.Communication;
using MicroXBase.Devices.Types;
using System;
using UsbLibrary;

namespace MicroXBase.OpCodes.Management
{
    public abstract class MicroXOpCodeWrapper : OpCodeWrapper, IDisposable
    {
        #region Fields
        private SpecifiedDevice specifiedDevice = null;
        #endregion
        #region Properties
        public virtual SpecifiedDevice DeviceIO
        {
            get { return specifiedDevice; }
            protected set
            {
                if (value != specifiedDevice)
                {
                    if (specifiedDevice != null)
                    {
                        specifiedDevice.DataRecieved -= Device_DataRecieved;
                        specifiedDevice.DataSend -= Device_DataSent;
                    }
                    if (value != null)
                    {
                        value.DataRecieved += Device_DataRecieved;
                        value.DataSend += Device_DataSent;
                    }

                    specifiedDevice = value;
                }
            }
        }
        #endregion
        #region Constructors
        public MicroXOpCodeWrapper(MicroXDevice parent)
            : base(parent)
        {
        }
        #endregion
        #region Events
        protected virtual void Device_DataRecieved(object sender, USBDataReceivedEventArgs args)
        {
            //This is a workaround for an unresolved issue with the PicoLite device
            if (false && args.data[1] == 0xff)
            {
                //MOVE TO PICOLITE!!!!!!!
                Log.WriteLine("ERROR: FOUND AN 0xFF PACKET");
                Download.Invoke();
            }
            else
            {
                base.OnDataReceived(args.data);
            }
        }

        protected virtual void Device_DataSent(object sender, DataSendEventArgs args)
        {
            Console.WriteLine(args.data);
        }
        #endregion
        #region OpCodes
        public MicroXHeaderNoParse Run;
        public MicroXHeaderNoParse Stop;
        public Setup Setup;
        public Status Status;
        public SetSN SetSN;
        public TimerRun TimerRun;
        public DefaultCalibration DefaultCalibration;
        public DataPacket DataPacket;
        public BaseDataPacket OnlineDataPacket;
        public BaseDataPacket OnlineTimeStamp;
        public FirmwareUpload FirmwareUpload;
        public FirmwareUploadDone FirmwareUploadDone;
        public Boomerang GetBoomerang;
        public Boomerang SetBoomerang;
        public Download Download;
        #endregion
        #region Overriden Methods
        public override void Dispose()
        {
            this.DeviceIO = null;
            base.Dispose();
            foreach (var opcode in OpCodeInstances)
                opcode.Dispose();
        }
        public override void SetDeviceInterfacer(IDeviceIO deviceIO)
        {
            if (deviceIO is SpecifiedDevice)
                this.DeviceIO = deviceIO as SpecifiedDevice;
        }
        public override void DeviceSpecificSend(byte[] data)
        {
            DeviceIO.SendData(data);
        }
        #endregion
    }
}
