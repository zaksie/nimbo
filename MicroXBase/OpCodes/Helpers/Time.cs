using Base.OpCodes.Reports;
using Base.OpCodes.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MicroXBase.OpCodes.Helpers
{
    [Serializable]
    public class MicroXTime : Time
    {
        #region Properties
        public static MicroXTime Now
        {
            get { return new MicroXTime(); }
        }
        #endregion
        #region Constructors
        public MicroXTime()
            : base()
        {
        }
        public MicroXTime(Base.OpCodes.Helpers.Time time)
        {
            Set(time);
        }
        #endregion
        #region Time Methods
        public static MicroXTime Parse(Report report)
        {
            MicroXTime time = new MicroXTime();
            time.Year = (byte)((report.Current & 0xFC) >> 2);
            time.Month = (byte)((report.Next & 0x03) << 2);
            time.ParseAux(report);
            return time;
        }
        protected override void ParseAux(Report Report)
        {
            Month += (byte)((Report.Current & 0xC0) >> 6);
            Day = (byte)((Report.Current & 0x3E) >> 1);
            Hour = (byte)((Report.Next & 0x01) << 4);
            Hour += (byte)((Report.Current & 0xF0) >> 4);
            Minute = (byte)((Report.Next & 0x0F) << 2);
            Minute += (byte)((Report.Current & 0xC0) >> 6);
            Second = (byte)((Report.Next & 0x3F));
        }
        #endregion
        #region Methods
        public static MicroXTime ParseDataPacketTime(Report Report, DateTime StartingTime)
        {
            MicroXTime time = new MicroXTime();
            time.Year = (byte)(((Report.Current & 0x0C) >> 2) + StartingTime.Year);
            time.Month = (byte)((Report.Next & 0x03) << 2);  
            time.ParseAux(Report);

            return time;
        }
        public override byte[] GetBytes()
        {
            byte[] bytes = new byte[4];
            int index = 0;
            bytes[index++] = (byte)(Year << 2 | ((Month & 0x0C) >> 2));
            bytes[index++] = (byte)(((Month & 0x03) << 6) | ((Day & 0x1F) << 1) | ((Hour & 0x10) >> 4));
            bytes[index++] = (byte)(((Hour & 0x0F) << 4) | ((Minute & 0x3C) >> 2));
            bytes[index++] = (byte)(((Minute & 0x03) << 6) | (Second & 0x3F));

            return bytes;
        }
        #endregion
    }
}
