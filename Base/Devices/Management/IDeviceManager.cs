using Maintenance.Communication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Devices.Management
{
    public delegate void ReportGenericDeviceDelegate(object sender, DeviceEventArgs<GenericDevice> e);
    public delegate void ReportDeviceDelegate<T>(object sender, DeviceEventArgs<T> e) where T : GenericDevice;
    
    public interface IDeviceManager : ISearchableDeviceList
    {
        DeviceID[] DeviceIDs { get; }
        
        event ReportGenericDeviceDelegate OnReportDevice;
        DeviceList<GenericDevice> GetDeviceList();
        void OnDeviceFound(string UID, uint VID, uint PID, IDeviceIO deviceIO);
        void OnDeviceRemoved(string UID);

        void Subscribe<E>(ReportDeviceDelegate<E> reportMethod) where E : GenericDevice;
    }
}
