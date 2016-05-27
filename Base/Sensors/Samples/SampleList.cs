using Base.Sensors.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Sensors.Samples
{
    public delegate void SampleAddedDelegate(GenericSensor sensor, Sample sample);

    [Serializable]
    public class SampleList : IDisposable
    {
        #region Events
        public event SampleAddedDelegate OnSampleAdded;
        #endregion
        #region Properties
        protected List<Sample> Items = new List<Sample>();
        protected GenericSensor ParentSensor;
        public int Count { get { return Items.Count; } }
        public Sample this[int index]
        {
            get { return Items[index]; }
        }
        #endregion
        #region Constructors
        public SampleList(GenericSensor parent)
        {
            Initialize(parent);
        }

        protected virtual void Initialize(GenericSensor parent)
        {
            this.ParentSensor = parent;
        }
        #endregion
        #region Methods
        public IReadOnlyList<Sample> AsReadOnly()
        {
            return Items.AsReadOnly();
        }

        public void Add(Sample sample)
        {
            Items.Add(sample);
            if (OnSampleAdded != null)
                OnSampleAdded(ParentSensor, sample);
        }

        public void AddOnline(long digitalValue, DateTime time)
        {
            AddOnline(digitalValue, time, string.Empty);
        }
        public void AddOnline(long digitalValue, DateTime time, string comment)
        {
            Sample sample = ParentSensor.GenerateSample(digitalValue, time, comment, true);
            Add(sample);
        }

        public void AddOffline(long digitalValue, DateTime time)
        {
            AddOffline(digitalValue, time, string.Empty);
        }
        public void AddOffline(long digitalValue, DateTime time, string comment)
        {
            Sample sample = ParentSensor.GenerateSample(digitalValue, time, comment, false);
            Add(sample);
        }

        public void Clear()
        {
            Items.Clear();
        }
        #endregion
        #region IDisposable
        public void Dispose()
        {
            OnSampleAdded = null;
            Items.Clear();
            ParentSensor = null;
        }
        #endregion
    }
}
