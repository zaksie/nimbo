using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.OpCodes.Management
{
    public delegate void SendReportDelegate();
    public delegate void OnInvokeFinishedDelegate(OpCodeInvokerArgs e);

    public class OpCodeDelegates
    {
        public SendReportDelegate SendReport;
        public OnInvokeFinishedDelegate OnInvokeFinished;

        public int Retries { get; set; }

        public TimeSpan WaitDuration { get; set; }
    }
}
