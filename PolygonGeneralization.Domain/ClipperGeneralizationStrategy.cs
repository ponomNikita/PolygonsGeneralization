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

        public List<Polygon> Generalize(List<Claster> clasters, double minDistance)
        {
            var count = clasters.Count;
            var completedCount = 0;
            var result = new List<Polygon>();
            foreach (var claster in clasters)
            {
                var mergedClaster = MergeClaster(claster);
                
                result.AddRange(mergedClaster);
                _logger.Log($"Merged {++completedCount} from {count} clasters");
            }

            return result;
        }

        private List<Polygon> MergeClaster(Claster claster)
        {
            if (claster.Polygons.Count == 1)
            {
                return claster.Polygons;
            }

            List<Polygon> resultList = new List<Polygon>();
            Polygon result = null;
            var count = claster.Polygons.Count;
            var mergedCount = 0;
            foreach (var polygon in claster.Polygons)
            {
                if (result == null)
                {
                    result = polygon;
                    //_logger.Log($"Merged {++mergedCount} from {count} polygons");
                    continue;
                }

                var union = _clipper.Union(result, polygon);
                result = union.First();

                if (union.Count == 2)
                {
                    resultList.Add(union.Last());
                }
                
                //_logger.Log($"Merged {++mergedCount} from {count} polygons");
            }

            resultList.Add(result);
            
            return resultList;
        }
    }
}