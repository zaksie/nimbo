using AuxiliaryLibrary;
using Base.OpCodes.Helpers;
using Base.OpCodes.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.OpCodes.Reports
{
    public abstract class Report : IDisposable
    {
        #region Fields
        protected OpCode ParentOpCode;
        #endregion
        #region Properties
        public int Index { get; protected set; }
        public int SpaceLeft { get { return Content.Length - Index - 1; } }
        public bool HasNext { get { return SpaceLeft > 0; } }
        public byte Next
        {
            get { return Content[Index++]; }
            set { Content[Index++] = value; }
        }

        public byte Current
        {
            get { return Content[Index]; }
            set { Content[Index] = value; }
        }

        protected byte[] Content;

        public byte this[int index]
        {
            get { return Content[index]; }
            set { Content[index] = value; }
        }
        #endregion
        #region Ctors
        public Report(OpCode parent)
        {
            this.ParentOpCode = parent;
        }
        public virtual void Initialize()
        {
            Index = ParentOpCode.StartByte;
            Content = Utilities.InitializeArray(Content);
        }
        #endregion
        #region Methods
        public byte[] GetContent() { return Content; }

        public bool Has(int bytes)
        {
            return Index + bytes < Content.Length;
        }

        public void Skip(int count)
        {
            Index += count;
        }

        public void Skip(SpareBytes spare)
        {
            Index += spare.Size;
        }

        public byte GenerateCheckSum()
        {
            UInt64 TotalSum = 0;

            for (int Index = 0; Index < Content.Length; Index++)
                TotalSum += Content[Index];

            return (byte)(TotalSum % 256);
        }

        protected byte GenerateXORCheckSum()
        {
            byte checksum = Content[0];
            for (int index = 1; index < Content.Length - 1; index++)
                checksum ^= Content[index];

            return checksum;
        }
        #endregion
        #region IDisposable
        public void Dispose()
        {
            //Nothing here for now
        }
        #endregion
    }
}
