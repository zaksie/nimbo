using Base.OpCodes.Helpers;
using Base.OpCodes.Management;
using MicroXBase.Devices.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroXBase.OpCodes.Management
{
    [Serializable]
    public abstract class MicroXHeaderNoParse : MicroXOpCode
    {
        public MicroXHeaderNoParse(MicroXDevice device)
            : base(device)
        {

        }

        #region Populate Methods
        protected override void PopulateReport()
        {
            PopulateOpCodeSend();
        }
        #endregion
        #region Not Supported
        protected override void ParseReport()
        {
            throw new NotSupportedException();
        }
        protected override void PacketParsed(Result result)
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}
