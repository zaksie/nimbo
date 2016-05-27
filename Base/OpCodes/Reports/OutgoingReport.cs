using Base.OpCodes.Management;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.OpCodes.Reports
{
    public class OutgoingReport : Report
    {
        public OutgoingReport(OpCode opCode)
            : base(opCode)
        {
            Content = new byte[opCode.OutReportSize];
            Initialize();
        }

        protected void SignWithCheckSum()
        {
            Content[Index++] = GenerateCheckSum();
        }


        public void InsertString(string str)
        {
            int endByte = Index + str.Length - 1;
            InsertString(str, endByte);
        }

        public void InsertString(string str, int maxLength)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] bytesString = encoding.GetBytes(str);

            foreach (byte b in bytesString)
                if (Index < maxLength + 1)
                    Content[Index++] = b;
            if (Index > maxLength + 1)
                throw new Exception("Comment exceeds " + str.Length + " chars");
            for (int LoopIndex = Index; LoopIndex <= maxLength; LoopIndex++)
                Content[Index++] = 0;
        }


        public void InsertBlock(byte[] block)
        {
            int length = Math.Min(SpaceLeft, block.Length);
            Buffer.BlockCopy(block, 0, Content, Index, length);
            Index += block.Length;
        }

        public void InsertSingle(float number)
        {
            byte[] block = BitConverter.GetBytes(number);
            InsertBlock(block);
        }

        public void SignXORCheckSum()
        {
            Content[Content.Length - 1] = GenerateXORCheckSum();
        }
    }
}
