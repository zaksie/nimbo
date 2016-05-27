using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using AuxiliaryLibrary;
using Base.Sensors;
using Base;
using AuxiliaryLibrary.MathLib;

namespace Base.OpCodes.Helpers
{
    [Serializable]
    public class Alarm
    {
        #region Fields
        public IntegerFraction Low;
        public IntegerFraction High;
        #endregion
        #region Properties
        public bool Active
        {
            get;
            set;
        }
        #endregion
    }
}
