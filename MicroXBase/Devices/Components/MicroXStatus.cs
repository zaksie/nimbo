using Base.Devices.Components;
using Base.Sensors.Types;
using Base.Sensors.Management;
using MicroXBase.Devices.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroXBase.Devices.Components
{
    public class MicroXStatus : GenericStatus
    {
        #region Prperties
        new protected MicroXDevice ParentDevice
        {
            get { return base.ParentDevice as MicroXDevice; }
            set { base.ParentDevice = value; }
        }
        public override ushort Interval { get { return ParentDevice.Configuration.Interval; } }
        public override bool Running { get { return ParentDevice.Configuration.Running; } }
        public override bool CyclicMode { get { return ParentDevice.Configuration.CyclicMode; } }
        public override bool FahrenheitMode { get { return ParentDevice.Configuration.FahrenheitMode; } }
        public override string SerialNumber { get { return ParentDevice.Configuration.SerialNumber; } }
        public override string Comment { get { return ParentDevice.Configuration.Comment; } }
        public bool BoomerangActive { get { return ParentDevice.Configuration.BoomerangActive; } }
        public bool TimerRunEnabled { get { return ParentDevice.Configuration.TimerRunEnabled; } }
        public DateTime TimerStart { get { return ParentDevice.Configuration.TimerStart; } }
        public bool PushToRunMode { get { return ParentDevice.Configuration.PushToRunMode; } }
        public byte DeviceType { get { return ParentDevice.Configuration.DeviceType; } }
        #endregion
        #region Constructors
        public MicroXStatus(MicroXDevice device)
            : base(device)
        {
        }
        #endregion

    }
}