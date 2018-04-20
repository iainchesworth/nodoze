using Ninject.Modules;
using System.Collections.Generic;

using NoDoze.Interfaces;

namespace NoDoze.Bindings
{
    public class NoDozeBootstrapper : INinjectModuleBootstrapper
    {
        public IList<INinjectModule> GetModules()
        {
            // This is where you will be considering priority of your modules.
            return new List<INinjectModule>()
            {
                new NoDozeBindings()
            };

            // RepositoryModule cannot be loaded until DataObjectModule is loaded
            // as it is depended on DataObjectModule and DbConnectionModule has
            // dependency on RepositoryModule
        }
    }
}
