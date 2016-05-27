using Base.Misc;
using Base.OpCodes.Helpers;
using Base.OpCodes.Management;
using Maintenance;
using MicroXBase.Devices.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MicroXBase.OpCodes.Common
{
    [Serializable]
    public abstract class GetBoomerang : Boomerang
    {
        #region Consts
        const byte GET_METHOD = 0x01;
        #endregion
        #region Properties
        private string contactListString;
        protected override byte Method
        {
            get { return GET_METHOD; }
        }
        public override byte[] SendOpCode
        {
            get { return new byte[] { 0x43 }; }
        }
        public override byte[] AckOpCode
        {
            get { return new byte[] { 0x43 }; }
        }
        #endregion
        #region Constructors
        public GetBoomerang(MicroXDevice device)
            : base(device)
        {
        }
        #endregion
        #region Parse Methods

        protected override void PacketParsed(Result result)
        {
            NextStep(result);
        }

        protected override void ParseReport()
        {
                ActionTypeEnum actionType = parseActionType();

                switch (actionType)
                {
                    case ActionTypeEnum.Info:
                        parseInfo();
                        break;
                    case ActionTypeEnum.Bank0:
                        clearContacts();
                        parseContactString();
                        break;
                    case ActionTypeEnum.Bank1:
                        parseContactString();
                        parseContacts();
                        //This marks the end of GetBoomerang process
                        break;
                }
        }

        private void clearContacts()
        {
            Contacts.Clear();
            contactListString = string.Empty;
        }

        private void parseContactString()
        {
            contactListString += InReport.GetString();
        }

        private void parseContacts()
        {
            string[] contacts = contactListString.Split(SEPARATOR_CHAR, StringSplitOptions.RemoveEmptyEntries);
            foreach (var contact in contacts)
                Contacts.Add(new BasicContact { EMail = contact });
        }

        private void parseInfo()
        {
            try
            {
                System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();

                string str = InReport.GetString();
                string[] details = str.Split(SEPARATOR_CHAR, StringSplitOptions.None);
                Comment = details[0];
                Author = details[1];
                byte UTCSetting = encoding.GetBytes(details[2])[0]; //Currently not in use
                byte Celsius = encoding.GetBytes(details[3])[0];
                ShowTemperatureInCelsius = Celsius == CELSIUS;
                DisplayAlarmLevels = encoding.GetBytes(details[4])[0] == '1';
            }
            catch (Exception ex)
            {
                Log.Write("GetBoomerang Error", ex);
            }
        }

        protected ActionTypeEnum parseActionType()
        {
            return (ActionTypeEnum)(InReport.Next >> 4);
        }
        #endregion
        #region Populate Methods
        protected override void PopulateReport()
        {
            PopulateOpCodeSend();
            PopulateMethodAndActionCode();
        }

        protected virtual void PopulateMethodAndActionCode()
        {
            OutReport.Next = (byte)(((byte)ActionType) << 4 | (byte)Method);
        }
        #endregion
    }

}
