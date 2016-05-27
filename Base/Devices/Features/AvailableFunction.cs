using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.IO;

namespace Base.Devices.Features
{
    public abstract class AvailableFunction
    {
        #region Properties
        public byte Code
        {
            get;
            protected set;
        }
        public bool Visible
        {
            get;
            set;
        }
        public bool Enabled
        {
            get;
            set;
        }
        public bool Checked
        {
            get;
            set;
        }
        #endregion
        #region Constructors
        public AvailableFunction(byte code)
        {
            this.Code = code;
        }
        #endregion
    }
}
