using Ninject;
using System;

namespace NoDoze.Bindings
{
    public static class DIFactory
    {
        private static IKernel kernel = null;

        public static T Resolve<T>()
        {
            if (kernel == null)
            {
                CreateKernel();
            }
            return kernel.Get<T>();
        }

        private static void CreateKernel()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            kernel = BootstrapHelper.LoadNinjectKernel(assemblies);
        }
    }
}
