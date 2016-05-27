using Base.Devices;
using Base.Devices.Features;
using Base.Devices.Management;
using Base.OpCodes.Helpers;
using Base.Sensors.Types;
using Maintenance;
using Maintenance.Communication;
using MicroXBase.Devices.Components;
using MicroXBase.Devices.Features;
using MicroXBase.OpCodes.DataStructures;
using MicroXBase.OpCodes.Helpers;
using MicroXBase.OpCodes.Management;
using MicroXBase.Sensors.Management;
using System;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace MicroXBase.Devices.Types
{
    [Serializable]
    public abstract class MicroXDevice : GenericLogger, ISerializable, IDisposable
    {
        #region Properties
        public override int AllowedSNLength { get { return 8; } }
        public override int AllowedCommentLength { get { return 15; } }
        public abstract byte Type { get; }
        public abstract byte Bits { get; }
        public abstract MicroXFeature[] Features { get; }
        public override string ClassVersion
        {
            get
            {
                string name = this.GetType().FullName;
                return Regex.Match(name, @"\.V[0-9]+\.").Groups[0].Value.Replace(".", "");
            }
        }

        new public MicroXSensorManager<GenericSensor> Sensors
        {
            get { return base.Sensors as MicroXSensorManager<GenericSensor>; }
            protected set { base.Sensors = value; }
        }

        new public MicroXStatus Status
        {
            get { return base.Status as MicroXStatus; }
            protected set { base.Status = value; }
        }

        new public MicroXOpCodeWrapper OpCodes
        {
            get { return base.OpCodes as MicroXOpCodeWrapper; }
            protected set { base.OpCodes = value; }
        }

        public IMicroXStatusConfiguration Configuration
        {
            get;
            set;                  
        }

        public virtual MicroXAvailableFunction[] AvailableFunctions
        {
            get
            {
                AvailableFunctionList<MicroXAvailableFunction> availableFunctions 
                    = new AvailableFunctionList<MicroXAvailableFunction>();

                if (!Offline)
                {
                    availableFunctions.Add(new MicroXAvailableFunction(MicroXDeviceFunctionEnum.DOWNLOAD) { Visible = true, Enabled = IsBusy });
                    availableFunctions.Add(new MicroXAvailableFunction(MicroXDeviceFunctionEnum.CANCEL_DOWNLOAD) { Visible = true, Enabled = IsDownloading });
                    availableFunctions.Add(new MicroXAvailableFunction(MicroXDeviceFunctionEnum.SETUP) { Visible = true, Enabled = IsBusy });
                    availableFunctions.Add(new MicroXAvailableFunction(MicroXDeviceFunctionEnum.STOP) { Visible = true, Enabled = Status.Running });
                    availableFunctions.Add(new MicroXAvailableFunction(MicroXDeviceFunctionEnum.RUN) { Visible = true, Enabled = Runnable && !IsBusy });
                    availableFunctions.Add(new MicroXAvailableFunction(MicroXDeviceFunctionEnum.CALIBRATE) { Visible = true, Enabled = !Status.Running && !IsBusy });
                    availableFunctions.Add(new MicroXAvailableFunction(MicroXDeviceFunctionEnum.DEFAULT_CALIBRATION_LOAD) { Visible = true, Enabled = !Status.Running && !IsBusy });
                    availableFunctions.Add(new MicroXAvailableFunction(MicroXDeviceFunctionEnum.DEFAULT_CALIBRATION_SAVE) { Visible = true, Enabled = !IsUpdatingFirmware });
                    availableFunctions.Add(new MicroXAvailableFunction(MicroXDeviceFunctionEnum.UPDATE_FIRMWARE) { Visible = true, Enabled = !Status.Running && !IsBusy });
                    availableFunctions.Add(new MicroXAvailableFunction(MicroXDeviceFunctionEnum.CANCEL_FIRMWARE_UPDATE) { Visible = true, Enabled = IsUpdatingFirmware });
                }

                return availableFunctions.ToArray();
            }
        }
        
        public virtual bool Runnable
        {
            get { return !Status.Running && !Status.TimerRunEnabled && !Status.PushToRunMode; }
        }
        public virtual bool IsBusy
        {
            get { return IsDownloading || IsUpdatingFirmware; }
        }
        protected override string SensorTypeLocation
        {
            get { return "MicroLabAPI.Sensors"; }
        }

        #endregion
        #region Constructors & Cleanup
        public MicroXDevice(IDeviceManager parent)
            :base(parent)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            Status = new MicroXStatus(this);
            Battery = new MicroXBattery(this);
        }

        protected override void Initialize(IDeviceManager parent)
        {
            base.Initialize(parent);
        }

        public void InitializeInterfacer(string uid, IDeviceIO deviceIO)
        {
            base.InitializeInterfacer(uid);
            OpCodes.SetDeviceInterfacer(deviceIO);
            Offline = false;
        }
        #endregion
        #region GenericDevice Implementation
        public override bool IsOfSameType(GenericDevice other)
        {
            return base.IsOfSameType(other)
                     && this.Memory == (other as MicroXDevice).Memory;
        }

        public override AvailableFunction[] WhatCanDeviceDoNow()
        {
            return AvailableFunctions;
        }

        public override DeviceFeature[] WhatFeaturesDeviceHas()
        {
            return Features;
        }
        #endregion
        #region OpCodes
        public override Result Run()
        {
            if (Runnable)
                return OpCodes.Run.Invoke();
            else
                return Result.ILLEGAL_CALL;
        }
        public override Result Stop()
        {
            //Downloading was added on the 10/7/11 because it was causing download problems
            if (IsDownloading)
                return Result.ERROR;
            else
                return OpCodes.Stop.Invoke();
        }

        public override Result CancelDownload()
        {
            if (IsDownloading)
                return OpCodes.Download.EndInvoke();
            else
                return Result.ILLEGAL_CALL;
        }
                public override Result Download()
        {
            throw new NotImplementedException();
        }
        
        public override IAsyncResult BeginStop(AsyncCallback callback)
        {
            throw new NotImplementedException();
        }

        public override IAsyncResult BeginRun(AsyncCallback callback)
        {
            throw new NotImplementedException();
        }

        #endregion
        #region Serialization & Deserialization
        //Deserialization constructor.
        public MicroXDevice(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
            try
            {
                //Get the values from info and assign them to the appropriate properties
                //RunTime = (DateTime)info.GetValue("RunTime", typeof(DateTime));
            }
            catch (System.Exception ex)
            {
                Log.Write(ex);
                throw ex;
            }
        }

        //Serialization function.
        public override void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            try
            {
                base.GetObjectData(info, ctxt);

                //info.AddValue("RunTime", RunTime, typeof(DateTime));
            }
            catch (System.Exception ex)
            {
                Log.Write(ex);
                throw ex;
            }
        }
        #endregion
    }
}
