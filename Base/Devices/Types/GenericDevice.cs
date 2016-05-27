using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.ComponentModel;
using Base.Devices.Components;
using Base.Devices.Features;
using Base.OpCodes.Management;
using AuxiliaryLibrary.Tools;
using Maintenance;
using Base.Devices.Management;
using Base.OpCodes.Helpers;

namespace Base.Devices
{
    public delegate void DeviceOperationDelegate<T>(object sender, DeviceEventArgs<T> e) where T : GenericDevice;

    [Serializable()]
    public abstract class GenericDevice : ISerializable, IDisposable
    {
        #region Events
        public event DeviceOperationDelegate<GenericDevice> OnOffline;
        public event DeviceOperationDelegate<GenericDevice> OnStatusReceived;
        public event DeviceOperationDelegate<GenericDevice> OnReconnected;
        #endregion
        #region Consts, Enums, etc.
        public enum IdentificationStages { NewDeviceFound, Identified };
        #endregion
        #region Fields
        protected bool offline = true;
        #endregion
        #region Properties
        public abstract Version MinimumRequiredFirmwareVersion { get; }
        public abstract string FirmwareDeviceName { get; }
        public abstract int AllowedSNLength { get; }
        public abstract int AllowedCommentLength { get; }
        public abstract bool IsUpdatingFirmware { get; }

        public IDeviceManager ParentDeviceManager { get; private set; }

        public virtual bool SupportsFirmwareUpdate
        {
            get { return true; }
        }   
        
        public abstract string DeviceTypeName
        {
            get;
        }

        public string UID // Currently used to store the full identifier of a MicroLab device (VID, PID and unique usb connection id)
        {
            get;
            protected set;
        }

        public virtual bool Offline
        {
            get {return offline;}
            set
            {
                if (offline != value)
                {
                    offline = value;
                    if (offline)
                        InvokeOnOffline();
                    else
                        InvokeOnReconnected();
                }
            }
        }

        public virtual bool RunnableWhileConnected
        {
            get { return true; }
        }

        public GenericStatus Status { get; protected set; }
        public GenericBattery Battery { get     ;protected set; }
        public GenericVersion  Version { get     ;protected set; }
        public GenericMemorySize Memory  { get     ;protected set;}
        public OpCodeWrapper OpCodes { get;      protected set;    }
        public abstract string ClassVersion { get; }
        #endregion
        #region Constructors
        public GenericDevice(IDeviceManager parent)
        {
            Initialize(parent);
        }

        public abstract void Initialize();
        protected virtual void Initialize(IDeviceManager parent)
        {
            this.ParentDeviceManager = parent;
        }
        public virtual void InitializeInterfacer(string uid)
        {
            this.UID = uid;
        }
        #endregion
        #region Virtual (Common) Methods
        public virtual bool IsOfSameType(GenericDevice other)
        {
            return this.DeviceTypeName == other.DeviceTypeName
                    && this.Version.Equals(other.Version);
        }
        public virtual void Predispose()
        {
        }
        #endregion
        #region Device Functionalities
        public abstract Result Setup(IConfiguration configuration);
        public abstract Result GetStatus();
        public abstract Result Stop();
        public abstract Result Run();
        public abstract Result CancelFirmwareUpdate();
        public abstract Result UploadFirmware();
        
        public abstract IAsyncResult BeginSetup(IConfiguration configuration, AsyncCallback callback);
        public abstract IAsyncResult BeginGetStatus(AsyncCallback callback);
        public abstract IAsyncResult BeginStop(AsyncCallback callback);
        public abstract IAsyncResult BeginRun(AsyncCallback callback);
        public abstract IAsyncResult BeginCancelFirmwareUpdate(AsyncCallback callback);
        public abstract IAsyncResult BeginUploadFirmware(AsyncCallback callback);
    
        #endregion
        #region Abstract Methods
        public abstract AvailableFunction[] WhatCanDeviceDoNow();
        public abstract DeviceFeature[] WhatFeaturesDeviceHas();
        #endregion
        #region Methods
        internal void SendReport(byte[] report)
        {
            throw new NotImplementedException();
        }
        internal virtual void ReplaceOpCodeWrapper(OpCodeWrapper newOpCodes)
        {
            OpCodes.Dispose();
            newOpCodes.Initialize(this);
            //implementing classes must carry out the substitution
        }
        protected void InvokeOnStatusReceived()
        {
            if (OnStatusReceived != null)
                OnStatusReceived(this, new DeviceEventArgs<GenericDevice> { Device = this, State = DeviceStateEnum.NONE });
        }
        protected void InvokeOnOffline()
        {
            if (OnOffline != null)
                OnOffline(this, new DeviceEventArgs<GenericDevice> { Device = this, State = DeviceStateEnum.REMOVED });
        }
        protected void InvokeOnReconnected()
        {
            if (OnReconnected != null)
                OnReconnected(this, new DeviceEventArgs<GenericDevice> { Device = this, State = DeviceStateEnum.RECONNECTED });
        }
        
        #endregion
        #region Object Methods
        public override bool Equals(object obj)
        {
            try
            {
                return base.Equals(obj)
                    || (obj as GenericDevice).Status.SerialNumber == this.Status.SerialNumber;
            }
            catch { return false; }
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override string ToString()
        {
            return DeviceTypeName + ": " + Status.SerialNumber;
        }
        #endregion
        #region Serialization & Deserialization
        //Deserialization constructor.
        public GenericDevice(SerializationInfo info, StreamingContext ctxt)
        {
            UID = (string)info.GetValue("UID", typeof(string));
        }

        //Serialization function.
        public virtual void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
                info.AddValue("UID", UID, typeof(string));
        }
        #endregion
        #region IDisposable
        public virtual void Dispose()
        {
            Offline = true;
            if (OpCodes != null)
                OpCodes.Dispose();

            OnOffline = null;
            OnStatusReceived = null;
        }
        #endregion
    }
}
