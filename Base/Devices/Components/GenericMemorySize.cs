using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Base.Devices.Components
{
    [Serializable]
    public abstract class GenericMemorySize : GenericComponent
    {
        #region Properties
        public abstract Enum Type
        {
            get;
        }
        public abstract string Name
        {
            get;
        }
        public abstract int Size
        {
            get;
        }
        #endregion
        #region Constructors
        public GenericMemorySize(GenericDevice device)
            : base(device)
        {
        }
        #endregion
        #region Overriden
        public override bool Equals(object obj)
        {
            return (obj as GenericMemorySize) != null && (obj as GenericMemorySize).Name == this.Name;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }

}
