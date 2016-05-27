using Base.Devices.Management;
using Maintenance;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Base.Modules
{
    public class ModuleManager
    {
        #region Static
        private static ModuleManager instance = new ModuleManager();
        public static ModuleManager Instance { get { return instance; } }
        #endregion
        #region Fields/Properties
        private CompositionContainer _container;
        
        [ImportMany]
        IEnumerable<Lazy<IDeviceManager, IDeviceManagerMetadata>> modules;

        #endregion
        #region Constructors
        private ModuleManager()
        {
            loadModuleDLLs();
        }

        private void loadModuleDLLs()
        {
            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();
            //Adds all the parts found in the same assembly as the Program class
            catalog.Catalogs.Add(new AssemblyCatalog(this.GetType().Assembly));
            catalog.Catalogs.Add(new DirectoryCatalog("."));

            //Create the CompositionContainer with the parts in the catalog
            _container = new CompositionContainer(catalog);

            //Fill the imports of this object
            try
            {
                this._container.ComposeParts(this);
            }
            catch (CompositionException compositionException)
            {
                Log.Write(compositionException);
            }
            catch (ReflectionTypeLoadException ex)
            {
                Log.Write(ex);
            }
        }

        public void UpdateDeviceManager()
        {
            if (modules != null)
                foreach (Lazy<IDeviceManager, IDeviceManagerMetadata> module in modules)
                {
                    SuperDeviceManager.instance.Add(module.Value, module.Metadata);
                }
        }
        #endregion
    }
}
