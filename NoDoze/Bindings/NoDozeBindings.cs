using Ninject;
using Ninject.Modules;
using Serilog;

using NoDoze.Interfaces;
using NoDoze.Logging;
using NoDoze.Services;

namespace NoDoze.Bindings
{
    public class NoDozeBindings : NinjectModule
    {
        public override void Load()
        {
            // Utilites, Helpers
            Bind<Interfaces.ILogger>().To<SerilogAdapter>().WithConstructorArgument("adaptee", new LoggerConfiguration().WriteTo.Console().CreateLogger());

            // Services, Repositories
            Bind<Interfaces.ISleepingService>().To<SleepingService>();
        }
    }
}
