using System.Data.Entity;
using Ninject;
using PolygonGeneralization.Domain;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Domain.SimpleClipper;
using PolygonGeneralization.Infrastructure;
using PolygonGeneralization.Infrastructure.Logger;
using PolygonGeneralization.Infrastructure.Services;
using PolygonGeneralization.WinForms.Interfaces;

namespace PolygonGeneralization.WinForms.Ninject
{
    public static class NinjectConfiguration
    {
        public static StandardKernel GetKernel()
        {
            var kernel = new StandardKernel();

            kernel.Bind<IClipper>().To<SimpleClipper>();
            kernel.Bind<IGisDataReader>().To<JsonGisDataReader>();
            kernel.Bind<IDrawerFactory>().To<DrawerFactory>();
            kernel.Bind<IDbService>().To<DbService>().InSingletonScope();
            kernel.Bind<DbContext>().To<DataBaseContext>();
            kernel.Bind<ILogger>().ToConstant(LoggerFactory.Create()).InSingletonScope();
            kernel.Bind<IGeneralizePolygonStrategy>().To<ClipperGeneralizationStrategy>();
            kernel.Bind<IGeneralizer>().To<Generalizer>();

            return kernel;
        }
    }
}
