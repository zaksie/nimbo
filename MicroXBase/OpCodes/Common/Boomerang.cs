using System;
using System.Collections.Generic;
using Base;
using Base.Devices;
using Base.Misc;
using Base.OpCodes.Management;
using Base.Devices.Management;
using MicroXBase.OpCodes.Management;
using MicroXBase.Devices.Types;
using Base.OpCodes.Helpers;

namespace MicroXBase.OpCodes.Common
{
    [Serializable]
    public abstract class Boomerang : MicroXOpCode
    {
        #region Consts & Enums
        protected ActionTypeEnum LAST_ACTION { get { return ActionTypeEnum.Bank1; } }
        protected enum ActionTypeEnum { Bank0 = 0x00, Bank1 = 0x01, Info = 0x02 };
        protected const byte BREAK_BYTE = 0xff;
        protected const byte NO_TIMEZONE = 0x1A;
        protected const byte CELSIUS = 0x10;
        protected const byte FAHRENHEIT = 0x11;
        protected const int MAX_BANK0_SIZE = 61;
        protected static string[] SEPARATOR_CHAR { get { return new string[] { System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { BREAK_BYTE }) }; } }
        #endregion
        #region Properties
        public int AllowedConactLength { get { return 61 * 2; } } //2 Banks contain up to 62 bytes
        public int AllowedCommentAuthorLength { get { return OutReportSize - 3; } } //Where 3 include 1 opcode, 1 set/get byte, 1 separator byte

        public List<BasicContact> Contacts
        {
            get;
            protected set;
        }
        protected abstract byte Method
        {
            get;
        }
        protected ActionTypeEnum ActionType
        {
            get;
            set;
        }
        protected byte[] ContactsInBytes
        {
            get
            {
                List<byte> Bytes = new List<byte>();
                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                if(Contacts != null)
                foreach (BasicContact Contact in Contacts)
                {
                    byte[] bytes = enc.GetBytes(Contact.EMail.ToLowerInvariant());
                    if (bytes.Length + Bytes.Count > AllowedConactLength)
                        break;
                    else
                    {
                        Bytes.AddRange(bytes);
                        Bytes.Add(BREAK_BYTE);
                    }
                }
                return Bytes.ToArray();
            }
        }
        public string Comment
        {
            get;
            set;
        }
        public string Author
        {
            get;
            set;
        }

        public bool ShowTemperatureInCelsius
        {
            get;
            set;
        }

        public bool DisplayAlarmLevels
        {
            get;
            set;
        }

        public bool Valid
        {
            get
            {
                return Comment.Length + Author.Length < AllowedCommentAuthorLength
                 && ContactsInBytes != null;
            }
        }
        #endregion
        #region Constructors
        public Boomerang(MicroXDevice device)
            : base(device)
        {
        }
        #endregion
        #region Methods
        protected override void PacketSent(Result result)
        {
            NextStep(result);
        }
        protected void NextStep(Result result)
        {
            if (result.Value == ResultEnum.OK)
            {
                if (ActionType != LAST_ACTION)
                {
                    SetNextActionType();
                    Start();
                }
            }
             
            Finished(result);
        }
        protected override void DoBeforeInitialRun()
        {
            ResetActionType();
        }

        protected virtual void SetNextActionType()
        {
            switch (ActionType)
            {
                case ActionTypeEnum.Info:
                    ActionType = ActionTypeEnum.Bank0;
                    break;
                case ActionTypeEnum.Bank0:
                    ActionType = ActionTypeEnum.Bank1;
                    break;
            }
        }
        protected virtual void Initialize()
        {
            Contacts.Clear();
            Comment = Author = string.Empty;
            ShowTemperatureInCelsius = true;
            ActionType = ActionTypeEnum.Info;
        }

        protected void ResetActionType()
        {
            ActionType = ActionTypeEnum.Info;
        }
        #endregion
    }
}
