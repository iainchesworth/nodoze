using Ninject;
using System;

namespace NoDoze.Bindings
{
    public static class DiFactory
    {
        private static IKernel _kernel;

        public static T Resolve<T>()
        {
            if (_kernel == null)
            {
                _kernel = CreateKernel();
            }

            return _kernel.Get<T>();
        }

        private static IKernel CreateKernel()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return _kernel = BootstrapHelper.LoadNinjectKernel(assemblies);
        }
    }
}
