using Base.Devices;
using Base.Devices.Management;
using Base.OpCodes.Helpers;
using Base.OpCodes.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MicroXBase.OpCodes.Management;
using MicroXBase.Devices.Types;

namespace MicroXBase.OpCodes.Common
{
    [Serializable]
    public abstract class Download : MicroXOpCode
    {
        #region Enums
        public enum MessageTypeEnum { PacketCount = 0x00, MemoryFull = 0x01, EndOfPackets = 0x0F, EndOfCycleReached };
        public enum DownloadCommandEnum : byte { Next = 0x00, Anew = 0x01, Cancel = 0x02, Info = 0x03};
        #endregion
        #region Properties
        public MessageTypeEnum MessageType
        {
            get;
            set;
        }
        public UInt32 PacketCount
        {
            get;
            protected set;
        }
        protected DownloadCommandEnum Command
        {
            get;
            set;
        }
        #endregion
        #region Constructors
        public Download(MicroXDevice device)
            : base(device)
        {}
        #endregion
        #region Methods
        protected override void DoBeforeInitialRun()
        {
            Command = DownloadCommandEnum.Info;
        }

        protected void RunNextCommand()
        {
            setNextCommand();
            Start();
        }

        private void setNextCommand()
        {
            switch (Command)
            {
                case DownloadCommandEnum.Next:
                case DownloadCommandEnum.Anew:
                    Command = DownloadCommandEnum.Next;
                    break;
                case DownloadCommandEnum.Cancel:
                    break;
                case DownloadCommandEnum.Info:
                    Command = DownloadCommandEnum.Anew;
                    break;
            }
        }
        #endregion
        #region Parse Methods
        protected abstract void ParseMessage();
        protected abstract void ParsePacketCount();
        #endregion
        #region Populate Methods
        protected abstract void PopulateMessage();
        #endregion
    }
}
