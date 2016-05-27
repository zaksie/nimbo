using Base.Devices;
using Base.OpCodes.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MicroXBase.OpCodes.Management;
using MicroXBase.Devices.Types;
using Base.OpCodes.Helpers;

namespace MicroXBase.OpCodes.Common
{
    [Serializable]
    public abstract class SetSN : MicroXOpCode
    {
        #region Constructors
        public SetSN(MicroXDevice device)
            : base(device)
        {
        }
        #endregion
        #region Populate Methods
        protected override void PopulateReport()
        {
            PopulateOpCodeSend();
            PopulateSN();
        }

        protected virtual void PopulateSN()
        {
            OutReport.InsertString(ParentOpCodeWrapper.Setup.Configuration.SerialNumber);
        }
        #endregion
        #region Not Supported
        protected override void PacketParsed(Result result)
        {
            throw new NotSupportedException();
        }
        protected override void ParseReport()
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}
