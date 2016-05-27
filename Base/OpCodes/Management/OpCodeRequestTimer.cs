using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Base;
using Base.Devices;
using System.Reflection;
using System.ComponentModel;
using System.Threading;
using Maintenance;
using Base.Devices.Management;
using Base.OpCodes.Helpers;

namespace Base.OpCodes.Management
{
    public class OpCodeRequestTimer
    {
        #region Fields
        private OpCodeDelegates methods { get; set; }
        private BackgroundWorker OpCodeWorker;
        private object startStopLock = new object();
        private ManualResetEvent onOpCodeWorkerAckedMRE = new ManualResetEvent(false);
        private OpCode parentOpCode;
        #endregion
        #region Properties
        public Result LastResult { get; private set; }
        private bool RetriesLeft
        {
            get { return methods.Retries == 0 || retryIndex < methods.Retries; }
        }
        private int retryIndex
        {
            get;
            set;
        }
        public bool Started
        {
            get;
            private set;
        }
        #endregion
        #region Constructors
        public OpCodeRequestTimer(OpCode parent, OpCodeDelegates methods)
        {
            this.parentOpCode = parent;
            this.methods = methods;
        }
        #endregion
        #region Methods
        public void Start()
        {
            lock(startStopLock)
            if (!Started)
            {
                retryIndex = 0;
                OpCodeWorker = new BackgroundWorker();

                OpCodeWorker.DoWork += new DoWorkEventHandler(OpCodeWorker_DoWork);
                OpCodeWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(OpCodeWorker_RunWorkerCompleted);
                OpCodeWorker.WorkerSupportsCancellation = true;

                parentOpCode.OnAcked += parentOpCode_OnAcked;
                OpCodeWorker.RunWorkerAsync();
                Started = true;
            }
        }

        void parentOpCode_OnAcked(OpCodeInvokerArgs e)
        {
            onOpCodeWorkerAckedMRE.Set();
        }
        public void Stop()
        {
            lock (startStopLock)
            if (Started)
            {
                OpCodeWorker.CancelAsync();
                onOpCodeWorkerAckedMRE.Set();
                Started = false;
            }
        }
        #endregion
        #region Events
        void OpCodeWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (RetriesLeft || OpCodeWorker.CancellationPending)
            {
                methods.SendReport();
                if (onOpCodeWorkerAckedMRE.WaitOne(methods.WaitDuration))
                    break;
                retryIndex++;
            }
        }
        void OpCodeWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            parentOpCode.OnAcked -= parentOpCode_OnAcked;
            methods.OnInvokeFinished(getStatus(e));
        }

        private OpCodeInvokerArgs getStatus(RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
                LastResult = Result.CANCELED;
            else if (e.Error != null)
                LastResult = Result.ERROR;
            else if (!onOpCodeWorkerAckedMRE.WaitOne(0))
                LastResult = Result.TIMED_OUT;
            else
                LastResult = Result.OK;

            return new OpCodeInvokerArgs { OpCode = parentOpCode, Result = LastResult, Parameters = null };
        }
        #endregion
    }
}
