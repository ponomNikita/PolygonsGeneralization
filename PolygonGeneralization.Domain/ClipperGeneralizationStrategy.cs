using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain
{
    public class ClipperGeneralizationStrategy : IGeneralizePolygonStrategy
    {
        private readonly IClipper _clipper;
        private ILogger _logger;

        public ClipperGeneralizationStrategy(IClipper clipper, ILogger logger)
        {
            _clipper = clipper;
            _logger = logger;
        }

        public async Task<List<Polygon>> Generalize(List<Claster> clasters, double minDistance)
        {
            var count = clasters.Count;
            var completedCount = 0;
            var result = new List<Polygon>();
            foreach (var claster in clasters)
            {
                result.Add(await MergeClaster(claster));
                _logger.Log($"Merged {++completedCount} from {count} clsters");
            }

            return result;
        }

        private async Task<Polygon> MergeClaster(Claster claster)
        {
            if (claster.Polygons.Count == 1)
            {
                return claster.Polygons.Single();
            }

            Polygon result = null;
            var count = claster.Polygons.Count;
            var mergedCount = 0;
            foreach (var polygon in claster.Polygons)
            {
                if (result == null)
                {
                    result = polygon;
                    _logger.Log($"Merged {++mergedCount} from {count} polygons");
                    continue;
                }

                result = await _clipper.Union(result, polygon);
                _logger.Log($"Merged {++mergedCount} from {count} polygons");
            }

            return result;
        }
    }
}