using Base.Devices;
using Base.Devices.Management;
using Base.OpCodes.Reports;
using Base.OpCodes.Helpers;
using Maintenance;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading;

namespace Base.OpCodes.Management
{
    public delegate void OpCodeEventDelegate(OpCodeInvokerArgs e);

    [Serializable]
    public abstract class OpCode
    {
        #region Events
        public event OpCodeEventDelegate OnAcked;
        public event OpCodeEventDelegate OnParsed;
        public event OpCodeEventDelegate OnError;
        public event OpCodeEventDelegate OnFinished;
        #endregion
        #region Fields
        private readonly ManualResetEvent operationMRE = new ManualResetEvent(false);
        private readonly object _sync = new object();
        public GenericDevice ParentDevice { get; protected set; }
        public OpCodeWrapper ParentOpCodeWrapper { get { return ParentDevice.OpCodes; } }
        protected IncomingReport InReport;
        protected OutgoingReport OutReport;
        protected OpCodeRequestTimer opCodeRequestTimer;
        #endregion
        #region Properties
        public bool IsComplete { get { return result.IsCompleted; } }
        public virtual int Retries { get { return 3; } }
        public virtual TimeSpan WaitDuration { get { return new TimeSpan(0, 0, 3); } }

        public byte[] OpCodeSpecificAckHeader { get { return AckHeader.Concat(SendOpCodeIfNotNull).ToArray(); } }
        public byte[] SendOpCodeIfNotNull { get { return SendOpCode != null ? SendOpCode : new byte[] { }; } }
        public abstract byte[] AckHeader { get; }
        public abstract byte[] SendOpCode { get; }
        public abstract byte[] AckOpCode { get; }
        public virtual int ReportSize
        {
            get { return 65; }
        }
        public virtual int StartByte
        {
            get { return 1; }
        }
        public virtual int OutReportSize
        {
            get { return ReportSize; }
        }
        public virtual int InReportSize
        {
            get { return ReportSize; }
        }
        #endregion
        #region Constructors
        public OpCode(GenericDevice device)
        {
            Initialize(device);
        }

        protected virtual void Initialize(GenericDevice device)
        {
            this.ParentDevice = device;
            this.InReport = new IncomingReport(this);
            this.OutReport = new OutgoingReport(this);
            this.opCodeRequestTimer = new OpCodeRequestTimer(this, ceateDelegates());
        }

        public virtual void Dispose()
        {
            opCodeRequestTimer.Stop();
            operationMRE.Set();
            OnAcked = null;
            OnParsed = null;
            OnError = null;
            OnFinished = null;
            ParentDevice = null;
            InReport = null;
            OutReport = null;
        }
        #endregion
        #region Generic Populate Methods
        protected void PopulateOpCodeSend()
        {
            OutReport.InsertBlock(SendOpCode);
        }
        #endregion
        #region Methods
        protected virtual bool isOpCodeHeader(IncomingReport report)
        {
            if (AckOpCode == null)
                return false;

            return report.ShiftIfIsBlock(AckOpCode);
        }
        private OpCodeDelegates ceateDelegates()
        {
            OpCodeDelegates opCodeDelegates = new OpCodeDelegates();
            opCodeDelegates.SendReport = SendReport;
            opCodeDelegates.OnInvokeFinished = DataSent;
            opCodeDelegates.Retries = Retries;
            opCodeDelegates.WaitDuration = WaitDuration;

            return opCodeDelegates;
        }

        protected virtual void Start()
        {
            opCodeRequestTimer.Start();
        }

        protected virtual void DoBeforeInitialRun() { }
        
        protected void Finished(Result finResult)
        {
            var e = new OpCodeInvokerArgs { OpCode = this, Result = finResult };

            operationMRE.Set();
            if (OnFinished != null)
                OnFinished(e);

            result.AsyncState = finResult;
            result.Exception = finResult.Exception;
            result.OpCodeArgs = e;

            if (callback != null)
                callback(result);

            result.IsCompleted = true;
        }

        #endregion
        #region Parse Methods
        protected abstract void ParseReport();

        public void BeginParseReport(byte[] data)
        {
            IncomingReport report = IncomingReport.Create(this, data);

            if (!checkForAckHeader(report))
                if (isOpCodeHeader(report))
                {
                    ThreadPool.QueueUserWorkItem(delegate
                    {
                        if (ParentDevice == null)
                            return;

                        Log.WriteLine("Running thread: " + this.GetType().Name + ", data opcode: " + report[1].ToString("X"));
                        processReport(report);
                    });
                }
        }

        private void processReport(IncomingReport report)
        {
            Result result = Result.OK;
            InReport.Assimilate(report);
            DoBeforeParseReport();

            try { ParseReport(); }
            catch (Exception ex)
            {
                result = Result.ERROR;
                result.Exception = ex;
            }

            packetParsed(result);
        }

        protected virtual void DoBeforeParseReport()
        {
            
        }

        private bool checkForAckHeader(IncomingReport report)
        {
            if (report.ShiftIfIsBlock(OpCodeSpecificAckHeader))
            {
                if (OnAcked != null)
                    OnAcked(new OpCodeInvokerArgs { OpCode = this, Result = Result.OK });
                return true;
            }

            return false;
        }

        protected virtual void PacketParsed(Result result) 
        {
            Finished(result);
        }

        private void packetParsed(Result result)
        {
            PacketParsed(result);
            if (OnParsed != null)
                OnParsed(new OpCodeInvokerArgs { OpCode = this, Result = result });
        }
        #endregion
        #region Populate/Report Methods
        protected virtual void PacketSent(Result result) { }
        protected virtual void PacketSendFailed(Result result)
        {
            Finished(result);
        }

        private void packetSent(OpCodeInvokerArgs e)
        {
            PacketSent(e.Result);
        }

        private void packetSendFailed(OpCodeInvokerArgs e)
        {
            PacketSendFailed(e.Result);
            if (OnError != null)
                OnError(e);
        }
        private void populateReport()
        {
            OutReport.Initialize();
            PopulateReport();
        }
        protected abstract void PopulateReport();

        protected void SendReport()
        {
            populateReport();
            ParentOpCodeWrapper.DeviceSpecificSend(OutReport.GetContent());
        }

        private void DataSent(OpCodeInvokerArgs e)
        {
            if (e.Result.Value == ResultEnum.OK)
                packetSent(e);
            else
                packetSendFailed(e);
        }
        #endregion
        #region Invoke Methods
        OpCodeAsyncResult result = new OpCodeAsyncResult(true);
        AsyncCallback callback;

        public IAsyncResult BeginInvoke(AsyncCallback callback)
        {
            if (result.IsCompleted)
            {
                this.callback = callback;

                ThreadPool.QueueUserWorkItem(delegate
                {
                    result.AsyncWaitHandle = new ManualResetEvent(false);
                    try
                    {
                        result.AsyncState = invoke();
                    }
                    catch (Exception exception)
                    {
                        result.Exception = exception;
                    }
                    result.IsCompleted = true;
                });
            }
            return result;
        }


        public Result EndInvoke()
        {
            if (!result.IsCompleted)
                result.AsyncWaitHandle.WaitOne();

            return result.Result;
        }


        public virtual Result Invoke()
        {
            result = new OpCodeAsyncResult(false);
            callback = null;
            return invoke();
        }

        protected virtual Result invoke()
        {
            lock (_sync)
            {
                DoBeforeInitialRun();
                Start();
                operationMRE.WaitOne();
                return result.Result;
            }
        }
        #endregion
        #region Serialization
        #endregion
    }
}
