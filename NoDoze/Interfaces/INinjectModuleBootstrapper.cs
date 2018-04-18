using Ninject.Modules;
using System.Collections.Generic;

namespace NoDoze.Interfaces
{
    public interface INinjectModuleBootstrapper
    {
        IList<INinjectModule> GetModules();
    }
}
