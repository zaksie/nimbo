using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.OpCodes.Helpers
{
    [Serializable]
    public class SpareBytes
    {
        private int start, end;
        public int Size
        {
            get {return end - start + 1;}
        }
        public SpareBytes(int start, int end)
        {
            this.start = start;
            this.end = end;
        }
        public SpareBytes(int length)
        {
            this.start = 1;
            this.end = length;
        }
    }
}
