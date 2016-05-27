using Base.OpCodes.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.OpCodes.Helpers
{
    [Serializable]
    public abstract class Time : ICloneable
    {
        #region Consts
        const int YEAR_OFFSET = 2000;
        #endregion
        #region Properties
        public virtual byte Year
        {
            get;
            set;
        }
        public virtual byte Month
        {
            get;
            set;
        }
        public virtual byte Day
        {
            get;
            set;
        }
        public virtual byte Hour
        {
            get;
            set;
        }
        public virtual byte Minute
        {
            get;
            set;
        }
        public virtual int Second
        {
            get;
            set;
        }
        #endregion
        #region Constructors
        public Time()
        {
            DateTime now = DateTime.Now;
            Year = (byte)(now.Year % 100);
            Month = (byte)now.Month;
            Day = (byte)now.Day;
            Hour = (byte)now.Hour;
            Minute = (byte)now.Minute;
            Second = (byte)now.Second;
        }
        public void Set(Time time)
        {
            Year = time.Year;
            Month = time.Month;
            Day = time.Day;
            Hour = time.Hour;
            Minute = time.Minute;
            Second = time.Second;
        }
        public void Set(DateTime time)
        {
            Year = Convert.ToByte(time.Year % 100); // in order to preserve just the 20xx part of the year
            Month = Convert.ToByte(time.Month);
            Day = Convert.ToByte(time.Day);
            Hour = Convert.ToByte(time.Hour);
            Minute = Convert.ToByte(time.Minute);
            Second = Convert.ToByte(time.Second);
        }
        #endregion
        #region Abstract Methods
        protected abstract void ParseAux(Report report);
        public abstract byte[] GetBytes();
        #endregion
        #region Operator Overload
        public static bool operator !=(Time Time1, Time Time2)
        {
            if ((object)Time1 == null && (object)Time2 == null)
                return false;
            else if ((object)Time1 == null ^ (object)Time2 == null)
                return true;
            return Time1.ToDateTime() != Time2.ToDateTime();
        }
        public static bool operator ==(Time Time1, Time Time2)
        {
            if ((object)Time1 == null && (object)Time2 == null)
                return true;
            else if ((object)Time1 == null ^ (object)Time2 == null)
                return false;
            return Time1.ToDateTime() == Time2.ToDateTime();
        }
        public static bool operator >=(Time Time1, Time Time2)
        {
            return Time1.ToDateTime() >= Time2.ToDateTime();
        }
        public static bool operator <=(Time Time1, Time Time2)
        {
            return Time1.ToDateTime() <= Time2.ToDateTime();
        }
        public static bool operator <(Time Time1, Time Time2)
        {
            return Time1.ToDateTime() < Time2.ToDateTime();
        }
        public static bool operator >(Time Time1, Time Time2)
        {
            return Time1.ToDateTime() > Time2.ToDateTime();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }
        #endregion
        #region Virtual Methods
        public virtual void Increase(long StepUp)
        {
            Increase(StepUp, this);
        }
        public virtual void Decrease(long StepUp)
        {
            Increase(-StepUp, this);
        }
        
        public virtual void Set(string dateTime)
        {
            Set(DateTime.Parse(dateTime));
        }
        
        public virtual bool Valid
        {
            get
            {
                //since we're past 2000 and time travel is still elusive
                //I have no choice but to use the year 2000 as an indication for an invalid time stamp
                return this.ToDateTime() != default(DateTime);
            }
        }

        #endregion
        #region Methods
        public DateTime ToDateTime()
        {
            try
            {
                return new DateTime
                    (Year + YEAR_OFFSET,
                        Month,
                        Day,
                        Hour,
                        Minute,
                        Second);
            }
            catch
            {
                return default(DateTime);
            }
        }
        public void Initialize()
        {
            Year =
            Month =
            Day =
            Hour =
            Minute = 0;
            Second = 0;
        }
        private static void Increase(Int64 seconds, ref Time time)
        {
            DateTime TempTime = time.ToDateTime().AddSeconds(seconds);
            time.Set(TempTime);
        }
        private static void Increase(Int64 seconds, Time time)
        {
            DateTime TempTime = time.ToDateTime().AddSeconds(seconds);
            time.Set(TempTime);
        }
        public void ThrowExceptionIfInvalid()
        {
            if (!this.Valid)
                throw new InvalidOperationException();
        }
        #endregion
        #region ICloneable
        public object Clone()
        {
            Time time = (Time)Activator.CreateInstance(this.GetType());
            time.Set(this);
            return time;
        }
        #endregion
    }
}
