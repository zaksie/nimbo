using Base.OpCodes.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.OpCodes.Reports
{
    public class IncomingReport : Report
    {
        public bool IsXORSigned
        {
            get { return Content[Content.Length - 1] == GenerateXORCheckSum(); }
        }

        public IncomingReport(OpCode opCode)
            : base(opCode)
        {
            Content = new byte[opCode.InReportSize];
            Initialize();
        }
        public bool ShiftIfIsBlock(byte[] block)
        {
            int index = Index;
            foreach (byte b in block)
                if (b != Content[index++])
                    return false;

            Index = index;
            return true;
        }

        public void Initialize(IncomingReport report)
        {
            this.Initialize(report.Content);
        }
        public void Initialize(byte[] data)
        {
            Initialize();
            Buffer.BlockCopy(data, 0, Content, 0, data.Length);
        }


        public string GetString()
        {
            return GetString(SpaceLeft);
        }

        public string GetString(int length)
        {
            if (length < 0)
                length = Content.Length - Index;
            byte[] receivedBytes = new byte[length];
            Buffer.BlockCopy(Content, Index, receivedBytes, 0, length);            

            Index += length;
            return System.Text.ASCIIEncoding.ASCII.GetString(receivedBytes).Substring(0, length).Replace("\0", "").Trim();
        }

        public byte[] GetBlock(int size)
        {
            byte[] result = new byte[size];
            Array.Copy(Content, Index, result, 0, size);
            Index += size;
            return result;
        }

        public static IncomingReport Create(OpCode opCode, byte[] data)
        {
            IncomingReport report = new IncomingReport(opCode);
            report.Content = data;
            return report;
        }
        
        internal void Assimilate(IncomingReport report)
        {
            Initialize(report);
            Index = report.Index;
        }

        public float GetSingle()
        {
            var result = BitConverter.ToSingle(Content, Index);
            Index += sizeof(float);
            return result;
        }
    }
}
