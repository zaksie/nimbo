using Base.Sensors.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.Devices.Components
{
    public abstract class GenericStatus : GenericComponent
    {
        #region Prperties
        public abstract UInt16 Interval      {get;}
        public abstract bool Running         {get;}
        public abstract bool CyclicMode      {get;}
        public abstract bool FahrenheitMode     {get;}
        public abstract string SerialNumber  {get;}
        public abstract string Comment { get; }
        #endregion
        #region Constructors
        public GenericStatus(GenericDevice device)
            : base(device)
        {
        }
        #endregion
    }
}
