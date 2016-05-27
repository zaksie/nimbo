using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base;
using Base.Devices;
using Base.OpCodes.Management;
using Base.OpCodes.Helpers;
using Maintenance;
using MicroXBase.Devices.Types;
using Base.Sensors.Management;
using Base.Sensors.Types;
using MicroXBase.OpCodes.Helpers;
using MicroXBase.OpCodes.Management;

namespace MicroXBase.OpCodes.Common
{
    [Serializable]
    public abstract class DataPacket : BaseDataPacket
    {
        #region Consts
        protected const int TIME_STAMP_SIZE = 4; // a time stamp segment in a data packet spans 10 bytes        
        #endregion
        #region Enums
        protected enum Result { END, NEXT_AVAILABLE, ERROR };
        #endregion
        #region Events
        public event ReportProgressDelegate ReportProgressEvent;        
        #endregion
        #region Properties
        public UInt32 CurrentAddress
        {
            get;
            set;
        }
        protected UInt32 PacketIndex
        {
            get;
            set;
        }
        protected UInt32 TotalPackets
        {
            get { return ParentOpCodeWrapper.Download.PacketCount; }
        }
        protected abstract int AverageEntryCount
        {
            get;
        }
        #endregion
        #region Constructors
        public DataPacket(MicroXDevice device)
            : base(device)
        {
        }
        #endregion
        #region Methods
        protected override void DoBeforeInitialRun()
        {
            PacketTime = null;
            PacketIndex = 0;
        }
        
        protected void ReportProgress(int percentage)
        {
            if (ReportProgressEvent != null)
                ReportProgressEvent(this, new ProgressReportEventArgs { Device = ParentDevice, Percent = percentage });
        }

        protected void ReportProgressBasedOnEntryIndex(int index)
        {
            ReportProgress(Convert.ToInt16(100 * (PacketIndex * AverageEntryCount + index) / (TotalPackets * AverageEntryCount)));
        }
        
        #endregion
        #region Parse Methods
        protected override void AddSampleToSensor(GenericSensor sensor, long value, DateTime dateTime)
        {
            sensor.Samples.AddOffline(value, dateTime);
        }

        //Meant to throw index out of bounds exception
        protected virtual long parseRawValue()
        {
            long value = (UInt16)(InReport.Next << 8);
            value += (UInt16)(InReport.Next);
            return value;
        }

        protected bool CheckAndHandleExpectedPacketTime(MicroXTime packetTime)
        {
            if (PacketTime == null)
            {
                PacketTime = packetTime;
                return true;
            }
            else
                return packetTime != PacketTime;
        }
        #endregion
        #region Populate Methods
        protected override void PopulateReport()
        {
            throw new NotSupportedException();
        }
        #endregion
    }
}
