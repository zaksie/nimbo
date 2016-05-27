using Base.Devices;
using Base.OpCodes.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MicroXBase.Devices.Types;
using MicroXBase.OpCodes.Management;
using Base.OpCodes.Helpers;

namespace MicroXBase.OpCodes.Common
{
    [Serializable]
    public abstract class DefaultCalibration : MicroXOpCode
    {
        public enum MethodEnum { Save = 0x00, Restore = 0x01, Get = 0x02, Set = 0x03 };
        #region Properties
        private bool swConfigBitToSend;
        protected MethodEnum Method
        {
            get;
            set;
        }
        #endregion
        #region Constructors
        public DefaultCalibration(MicroXDevice device)
            : base(device)
        { }
        #endregion
        #region Async Methods
        public IAsyncResult BeginGetConfigBit(AsyncCallback callback)
        {
            return setAsyncMethod(MethodEnum.Get,callback);
        }

        public IAsyncResult BeginSetConfigBit(bool value, AsyncCallback callback)
        {
            swConfigBitToSend = value;
            return setAsyncMethod(MethodEnum.Set,callback);
        }

        public IAsyncResult BeginSave(AsyncCallback callback)
        {
            return setAsyncMethod(MethodEnum.Save,callback);
        }

        public IAsyncResult BeginRestore(AsyncCallback callback)
        {
            return setAsyncMethod(MethodEnum.Restore,callback);
        }

        private IAsyncResult setAsyncMethod(MethodEnum method, AsyncCallback callback)
        {
            Method = method;
            return BeginInvoke(callback);
        }

        #endregion
        #region Invoke Methods
        public Result GetConfigBit()
        {
            return setSyncMethod(MethodEnum.Get);
        }

        public Result SetConfigBit(bool value)
        {
            swConfigBitToSend = value;
            return setSyncMethod(MethodEnum.Set);
        }

        public Result Save()
        {
            return setSyncMethod(MethodEnum.Save);
        }

        public Result Restore()
        {
            return setSyncMethod(MethodEnum.Restore);
        }

        private Result setSyncMethod(MethodEnum method)
        {
            Method = method;
            return Invoke();
        }
        #endregion
        #region Methods
        protected override void PacketSent(Result result)
        {
            if (Method == MethodEnum.Set)
            {
                Method = MethodEnum.Get;
                Start();
            }
            else
                Finished(result);
        }
        #endregion
        #region Parse Methods
        protected override void ParseReport()
        {
            parseSWConfigurationBit();
        }

        private void parseSWConfigurationBit()
        {
            ParentDevice.Configuration.SWConfigurationState = Convert.ToBoolean(InReport.Next);
        }
        #endregion
        #region Populate Methods
        protected override void PopulateReport()
        {
            PopulateOpCodeSend();
            PopulateMethod();
            PopulateSWConfigurationBit();
        }

        private void PopulateSWConfigurationBit()
        {
            OutReport.Next = Convert.ToByte(swConfigBitToSend);
        }
        private void PopulateMethod()
        {
            OutReport.Next = (byte)Method;
        }
        #endregion
    }
}
