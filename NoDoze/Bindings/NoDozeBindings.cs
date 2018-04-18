using Ninject;
using Ninject.Modules;
using Serilog;

using NoDoze.Interfaces;
using NoDoze.Logging;

namespace NoDoze.Bindings
{
    public class NoDozeBindings : NinjectModule
    {
        public override void Load()
        {
            Bind<Interfaces.ILogger>().To<SerilogAdapter>().WithConstructorArgument("adaptee", new LoggerConfiguration().WriteTo.Console().CreateLogger());
        }
    }
}
