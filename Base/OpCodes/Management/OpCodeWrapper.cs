using Base.Devices;
using Base.OpCodes.Reports;
using Maintenance;
using Maintenance.Communication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using Base.Devices.Management;

namespace Base.OpCodes.Management
{
    [Serializable]
    public abstract class OpCodeWrapper : IDisposable
    {
        #region Fields
        protected GenericDevice ParentDevice;
        private ManualResetEvent onQueueFilledUpMRE = new ManualResetEvent(false);
        protected Queue<OpCode> OpCodeQueue;
        protected BackgroundWorker OpCodeQueueProcessor;
        protected object analyzeLock = new object();
        protected object invokeOpCodeLock = new object();
        #endregion
        #region Properties
        private OpCode[] opCodeInstances;
        protected IEnumerable<OpCode> OpCodeInstances
        {
            get
            {
                if (opCodeInstances == null)
                {
                    opCodeInstances = (from field in OpCodeFields
                                       select field.GetValue(this) as OpCode).ToArray();
                }

                return opCodeInstances;
            }
        }
        protected IEnumerable<FieldInfo> OpCodeFields
        {
            get
            {
                return from FieldInfo field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance)
                              where field.FieldType.IsSubclassOf(typeof(OpCode))
                              select field;
            }
        }
        public virtual string OpCodeNamespacePrefix 
        { 
            get 
            {
                string fullname = ParentDevice.GetType().Namespace;
                return fullname.Substring(0, fullname.IndexOf('.') + 1) + STRUCTURE.OPCODES + "." + ParentDevice.ClassVersion ;                
            } 
        }
        protected virtual bool AckReceivedProceedWithQueue
        {
            get;
            set;
        }
        protected bool Disposed
        {
            get;
            set;
        }
        public bool IsBusy
        {
            get { return OpCodeQueue.Count > 0 && AckReceivedProceedWithQueue; }
        }
        #endregion
        #region Common OpCodes
        #endregion
        #region Constructors & Initializers
        public OpCodeWrapper(GenericDevice parent)
        {
            Initialize(parent);
        }

        public void Initialize(GenericDevice parent)
        {
            this.ParentDevice = parent;
            InitializeQueue();
        }

        public abstract void InitializeOpCodes();

        protected void InitializeQueue()
        {
            try
            {
                OpCodeQueue = new Queue<OpCode>();
                
                AckReceivedProceedWithQueue = true;
                
                OpCodeQueueProcessor = new BackgroundWorker();
                OpCodeQueueProcessor.DoWork += new DoWorkEventHandler(OpCodeQueueProcessor_DoWork);
                OpCodeQueueProcessor.WorkerSupportsCancellation = true;
                OpCodeQueueProcessor.RunWorkerAsync();
            }
            catch (System.Exception ex)
            {
                Log.Write(ex);
            }
        }
        #endregion
        #region Events
        public void OnDataReceived(byte[] data)
        {
            foreach (var opCode in OpCodeInstances)
            {
                opCode.BeginParseReport(data);
            }
        }
        #endregion
        #region Methods
        public abstract void DeviceSpecificSend(byte[] data);
        public abstract void SetDeviceInterfacer(IDeviceIO deviceIO);
        protected OpCode InstantiateOpCodeClass(string name)
        {
            string fullname = OpCodeNamespacePrefix + "." + name;
            Type type = ParentDevice.GetType().Assembly.GetType(fullname);

            if (type != null)
                return Activator.CreateInstance(type, this.ParentDevice) as OpCode;
            return null;
        }
        internal virtual void Assimilate(OpCodeWrapper newOpCodes)
        {
            ParentDevice.ReplaceOpCodeWrapper(newOpCodes);
        }
        internal bool CompatibleWith(Type type)
        {
            return this.GetType() == type;
        }
        #endregion
        #region Workers
        void OpCodeQueueProcessor_DoWork(object sender, DoWorkEventArgs e)
        {
            Log.WriteLine("Starting OpCode Queue Processor");
            while (!ParentDevice.Offline && !Disposed)
            {
                try
                {
                    if (OpCodeQueueProcessor.CancellationPending)
                        return;
                    if (!IsBusy)
                    {
                        onQueueFilledUpMRE.WaitOne();
                    }
                    else
                        lock (invokeOpCodeLock)
                        {
                            OpCode opCode = OpCodeQueue.Dequeue();
                            opCode.Invoke();
                            AckReceivedProceedWithQueue = false;
                        }
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                }
            }
        }
        #endregion
        #region OpCode Invoke Methods
        public void Schedule(OpCode opCode)
        {
            OpCodeQueue.Enqueue(opCode);
            onQueueFilledUpMRE.Set();
        }
        public void OnOpCodeAcknowledged(OpCodeInvokerArgs args)
        {
            lock (invokeOpCodeLock)
                AckReceivedProceedWithQueue = true;
        }
        
        #endregion
        #region IDisposable Members

        public virtual void Dispose()
        {
            OpCodeQueueProcessor.CancelAsync();
            onQueueFilledUpMRE.Set();
            OpCodeQueueProcessor.Dispose();
            Disposed = true;
        }

        #endregion
    }
}