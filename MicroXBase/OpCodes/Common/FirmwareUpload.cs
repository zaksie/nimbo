using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuxiliaryLibrary;
using Base.Devices;
using Base.OpCodes.Management;
using Maintenance.Firmware;
using Maintenance;
using MicroXBase.OpCodes.Management;
using MicroXBase.Devices.Types;
using Base.OpCodes.Helpers;

namespace MicroXBase.OpCodes.Common
{
    [Serializable]
    public abstract class FirmwareUpload : MicroXOpCode
    {
        #region Consts
        protected const byte DATA_SIZE = 32;
        #endregion
        #region Fields
        protected DeviceFirmware DeviceFW;
        protected Iterator FWIterator;
        #endregion
        #region Properties
        public override byte[] SendOpCode
        {
            get { return new byte[] { 0x31 }; }
        }
        public override byte[] AckOpCode
        {
            get { return new byte[] { 0x32 }; }
        }
        protected int ProgressPercent
        {
            get
            {
                return FWIterator.PercentProgress;
            }
        }
        public Version FirmwareVersion
        {
            get { return DeviceFW.FirmwareVersion; }
        }
        #endregion
        #region Constructors
        public FirmwareUpload(MicroXDevice device)
            : base(device)
        {
        }
        #endregion

        #region Methods
        protected override void PacketParsed(Result result)
        {
            if (result.Value == ResultEnum.OK)
            {
                if (FWIterator.HasNext)
                {
                    FWIterator.MoveNext();
                    Start();
                }
                else
                    Finished(ParentOpCodeWrapper.FirmwareUploadDone.Invoke());
            }
            else
                Start();
        }
        protected override void DoBeforeInitialRun()
        {
            FWIterator = DeviceFW.GetIterator(DATA_SIZE);
        }

        public void Load(FirmwareWrapper firmwareWrapper)
        {
            DeviceFW = firmwareWrapper[ParentDevice.FirmwareDeviceName];
        }
        #endregion
        #region Parse Methods
        protected override void ParseReport()
        {
            parseAndCheckAddress();
            parseAndCheckData();
        }

        private void parseAndCheckData()
        {
            byte[] data = InReport.GetBlock(DATA_SIZE);
            if(!Utilities.CompareArrays(data, FWIterator.Current.Data))
                throw new Exception("Failed to verify address");
        }

        private void parseAndCheckAddress()
        {
            byte[] address = new byte[2];
            address[0] = InReport.Next;
            address[1] = InReport.Next;

            if(!Utilities.CompareArrays(address, FWIterator.Current.AddressArray))
                throw new Exception("Failed to verify data");
        }
        #endregion
        #region Populate Methods
        protected override void PopulateReport()
        {
            PopulateOpCodeSend();
            PopulateAddress();
            PopulateData();
        }

        protected void PopulateData()
        {
            OutReport.InsertBlock(FWIterator.Current.Data);
        }

        protected void PopulateAddress()
        {
            OutReport.Next = (byte)((FWIterator.Current.Address & 0xff00) >> 8);
            OutReport.Next = (byte)(FWIterator.Current.Address & 0x00ff);
        }
        #endregion
    }
}
