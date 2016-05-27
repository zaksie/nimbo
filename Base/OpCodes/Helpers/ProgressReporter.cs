using Base.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.OpCodes.Helpers
{
    public delegate void ReportProgressDelegate(object sender, ProgressReportEventArgs e);

    public class ProgressReportEventArgs
    {
        public GenericDevice Device
        {
            get;
            set;
        }
        public int Percent
        {
            get;
            set;
        }
    }
}
