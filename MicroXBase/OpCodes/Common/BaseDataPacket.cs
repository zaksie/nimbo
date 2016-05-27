using Base.Devices;
using Base.Sensors.Types;
using MicroXBase.Devices.Types;
using MicroXBase.OpCodes.Helpers;
using MicroXBase.OpCodes.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroXBase.OpCodes.Common
{
    [Serializable]
    public abstract class BaseDataPacket : MicroXOpCode
    {
        #region Properties
        protected MicroXTime PacketTime;
        protected GenericLogger ParentLogger { get { return ParentDevice as MicroXDevice; } }
        #endregion
        #region Constructors
        public BaseDataPacket(MicroXDevice device)
            : base(device)
        {
        }
        #endregion
        #region Parse Methods
        protected abstract void AddSampleToSensor(GenericSensor sensor, long value, DateTime dateTime);

        protected void ExtractAndAdd()
        {
            PacketTime.ThrowExceptionIfInvalid();
            foreach (GenericSensor sensor in ParentLogger.Sensors.ActiveSensors)
            {
                AddSampleToSensor(sensor, ParseRawValue(), PacketTime.ToDateTime());
            }
        }

        //Meant to throw index out of bounds exception
        protected virtual long ParseRawValue()
        {
            long value = (UInt16)(InReport.Next << 8);
            value += (UInt16)(InReport.Next);
            return value;
        }
        
        protected MicroXTime ParsePacketTime()
        {
            try
            {
                return MicroXTime.ParseDataPacketTime(InReport, ParentDevice.Configuration.CurrentTime);
            }
            catch { throw new Exception("** ERROR with datetime format"); }
        }
        #endregion
    }
}
