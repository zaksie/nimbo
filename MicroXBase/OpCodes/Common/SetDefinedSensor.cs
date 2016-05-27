using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base.Sensors;
using Base.Sensors.Types;
using Base.Devices;
using Base.OpCodes.Management;
using MicroXBase.OpCodes.Management;
using MicroXBase.Devices.Types;

namespace MicroXBase.OpCodes.Common
{
    [Serializable]
    public abstract class SetDefinedSensor : MicroXOpCode
    {
        #region Constructors
        public SetDefinedSensor(MicroXDevice device)
            : base(device)
        { }
        #endregion
        #region Populate Methods
//        protected abstract void PopulateStartValue();
//        protected abstract void PopulateStepValue();
//        protected abstract void PopulatePCValues();

        protected abstract void PopulateName();
        protected abstract void PopulateUnitAndSignificantFigures();
        protected abstract void PopulateDisplayedValues();
        protected abstract void PopulateDefinedValues();
        #endregion
    }
}
