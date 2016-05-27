using Base.OpCodes.Helpers;
using Base.OpCodes.Management;
using MicroXBase.Devices.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroXBase.OpCodes.Common
{
    [Serializable]
    public abstract class SetBoomerang : Boomerang
    {
        #region Consts
        const byte SET_METHOD = 0x00;
        #endregion
        #region Properties
        protected override byte Method
        {
            get { return SET_METHOD; }
        }
        public override byte[] SendOpCode
        {
            get { return new byte[]{0x44}; }
        }
        public override byte[] AckOpCode
        {
            get { return null; }
        }
        #endregion
        #region Constructors
        public SetBoomerang(MicroXDevice device)
            : base(device)
        {
        }
        #endregion
        #region Methods
        
        #endregion
        #region Parse Methods
        protected override void ParseReport()
        {
            throw new NotSupportedException();
        }
        protected override void PacketParsed(Result result)
        {
            throw new NotSupportedException();
        }
        #endregion
        #region Populate Methods
        protected override void PopulateReport()
        {
            if (!Valid)
                throw new Exception("Invalid Boomerang Report");

            switch (ActionType)
            {
                case ActionTypeEnum.Info:
                    populateInfo();
                    break;
                case ActionTypeEnum.Bank0:
                    populateBank0();
                    break;
                case ActionTypeEnum.Bank1:
                    populateBank1();
                    break;
            }
        }

        private void populateInfo()
        {
            OutReport.InsertString(Comment);
            OutReport.Next = BREAK_BYTE;
            OutReport.InsertString(Author);
            OutReport.Next = BREAK_BYTE;
            OutReport.Next = NO_TIMEZONE;
            OutReport.Next = BREAK_BYTE;
            OutReport.Next = Convert.ToByte(ShowTemperatureInCelsius ? CELSIUS : FAHRENHEIT);
            OutReport.Next = BREAK_BYTE;
            OutReport.Next = Convert.ToByte(DisplayAlarmLevels ? '1' : '0');
        }

        private void populateBank0()
        {
            byte[] contactBytes = ContactsInBytes;
            
                OutReport.InsertBlock(contactBytes);
        }
        private void populateBank1()
        {
            byte[] contactBytes = ContactsInBytes;

            if (contactBytes.Length > MAX_BANK0_SIZE)
            {
                byte[] bank1 = new byte[contactBytes.Length - MAX_BANK0_SIZE];
                Array.Copy(contactBytes, MAX_BANK0_SIZE + 1, bank1, 0, bank1.Length);

                OutReport.InsertBlock(bank1);
            }
        }
        #endregion
    }

}
