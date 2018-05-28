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
        private VectorGeometry _vectorGeometry = new VectorGeometry();

        public ClipperGeneralizationStrategy(IClipper clipper, ILogger logger)
        {
            _clipper = clipper;
            _logger = logger;
        }

        public List<Polygon> Generalize(List<Claster> clasters, double minDistance)
        {
            var count = clasters.Count;
            var completedCount = 0;
            var result = new List<Polygon>();
            foreach (var claster in clasters)
            {
                var mergedClaster = MergeClaster(claster, minDistance);
                
                result.AddRange(mergedClaster);
                _logger.Log($"Merged {++completedCount} from {count} clasters");
            }

            return result;
        }

        private IEnumerable<Polygon> MergeClaster(Claster claster, double minDistance)
        {
            if (claster.Polygons.Count == 1)
            {
                return claster.Polygons;
            }

            var resultList = new List<Polygon>();
            var union = claster.Polygons.First();
            claster.Polygons.Remove(union);
            
            var count = claster.Polygons.Count;
            var completedCount = 0;

            while (claster.Polygons.Any())
            {
                var closest = claster.Polygons.OrderBy(it => _vectorGeometry.DistanceSqr(it, union)).First();
                claster.Polygons.Remove(closest);
                
                var unionResult = _clipper.Union(union, closest, minDistance);

                if (unionResult.Count == 2)
                {
                    resultList.Add(unionResult[1]);
                }

                union = unionResult[0];
                
                _logger.Log($"Merged {++completedCount} from {count} polygons");
            }

            resultList.Add(union);
            
            return resultList;
        }
    }
}