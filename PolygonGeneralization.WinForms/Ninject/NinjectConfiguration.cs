using System.Data.Entity;
using Ninject;
using PolygonGeneralization.Domain;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Domain.LinearGeneralizer;
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
            
            var generalizerOptions = new GeneralizerOptions
            {
                MinDistance = 10,
                MinDistanceCoeff = 3,
                MaxDifferenceInPercent = 5
            };

            kernel.Bind<GeneralizerOptions>().ToConstant(generalizerOptions).InSingletonScope();
            kernel.Bind<IClipper>().To<SimpleClipper>();
            kernel.Bind<ILinearGeneralizer>().To<LinearGeneralizer>();
            kernel.Bind<IGisDataReader>().To<CustomJsonGisDataReader>();
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
