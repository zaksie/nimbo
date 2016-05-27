using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using Base.Devices.Management;
using Maintenance;
using MicroXBase.Devices.Management;
using MicroXBase.Devices.Types;

namespace MicroXBase.Modules
{
    public class ModuleManager
    {
        #region Static
        private static ModuleManager instance = new ModuleManager();
        public static IDeviceInterfacer[] Modules 
        { 
            get 
            {
                return (from module in instance.modules
                        select module.Value).ToArray();
            }
        }
        #endregion
        #region Fields/Properties
        private CompositionContainer _container;
        
        [ImportMany]
        IEnumerable<Lazy<IDeviceInterfacer, IDeviceInterfacerMetadata>> modules;

        #endregion
        #region Initializers
        internal static void Start(MicroXDeviceManager parent)
        {
            instance.loadModuleDLLs(parent);
        }

        private void loadModuleDLLs(MicroXDeviceManager parent)
        {
            //An aggregate catalog that combines multiple catalogs
            var catalog = new AggregateCatalog();
            //Adds all the parts found in the same assembly as the Program class
            catalog.Catalogs.Add(new DirectoryCatalog(".\\Modules","Module*"));
            //Create the CompositionContainer with the parts in the catalog
            _container = new CompositionContainer(catalog);

            //Fill the imports of this object
            try
            {
                _container.ComposeExportedValue(parent as IDeviceManager);
                _container.ComposeParts(this);
                goOverModules();
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

        private void goOverModules()
        {
            foreach (var module in modules)
                Console.WriteLine(module.Metadata.Name);
        }
        #endregion
    }
}
