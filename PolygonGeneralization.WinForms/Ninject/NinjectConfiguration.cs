using Ninject;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Infrastructure.Services;

namespace PolygonGeneralization.WinForms.Ninject
{
    public static class NinjectConfiguration
    {
        public static StandardKernel GetKernel()
        {
            var kernel = new StandardKernel();

            kernel.Bind<IClipper>().To<PolygonGeneralizationCoreClipper>();

            return kernel;
        }
    }
}
