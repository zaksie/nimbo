using Base.Devices;
using Base.OpCodes.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MicroXBase.OpCodes.Management;
using MicroXBase.Devices.Types;

namespace MicroXBase.OpCodes.Common
{
    [Serializable]
    public abstract class FirmwareUploadDone : MicroXOpCode
    {
        #region Properties
        public override byte[] SendOpCode
        {
            get { return new byte[] { 0x33 }; }
        }
        public override byte[] AckOpCode
        {
            get { return null; }
        }
        #endregion
        #region Constructors
        public FirmwareUploadDone(MicroXDevice device)
            : base(device)
        {
        }
        #endregion
        #region Populate Methods
        protected override void PopulateReport()
        {
            PopulateOpCodeSend();
        }
        #endregion
        #region Not Supported
        protected override void ParseReport()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
