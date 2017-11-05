using Ninject;
using PolygonGeneralization.Domain;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Infrastructure.Services;
using PolygonGeneralization.WinForms.Interfaces;

namespace PolygonGeneralization.WinForms.Ninject
{
    public static class NinjectConfiguration
    {
        public static StandardKernel GetKernel()
        {
            var kernel = new StandardKernel();

            kernel.Bind<IClipper>().To<PolygonGeneralizationCoreClipper>();
            kernel.Bind<IGisDataReader>().To<JsonGisDataReader>();
            kernel.Bind<IDrawer>().To<Drawer>();

            return kernel;
        }
    }
}
