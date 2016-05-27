using Base.OpCodes.Helpers;
using System;
using System.Diagnostics;
using System.Threading;

namespace Base.OpCodes.Management
{
    public class OpCodeAsyncResult : IAsyncResult
    {
        public OpCodeInvokerArgs OpCodeArgs { get; set; }
        object _state;

        public bool IsCompleted { get; set; }
        public WaitHandle AsyncWaitHandle { get; internal set; }
        public object AsyncState
        {
            get
            {
                if (Exception != null)
                {
                    throw Exception;
                }
                return _state;
            }
            internal set
            {
                _state = value;
            }
        }
        public bool CompletedSynchronously { get { return IsCompleted; } }
        internal Exception Exception { get; set; }
        public Result Result { get { return AsyncState as Result; } }

        public OpCodeAsyncResult(bool isCompleted)
        {
            this.IsCompleted = isCompleted;
        }
    }
}