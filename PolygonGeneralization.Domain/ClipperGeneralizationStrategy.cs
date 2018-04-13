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

        public ClipperGeneralizationStrategy(IClipper clipper)
        {
            _clipper = clipper;
        }

        public async Task<List<Polygon>> Generalize(List<Claster> clasters, double minDistance)
        {
            var result = new List<Polygon>();
            foreach (var claster in clasters)
            {
                result.Add(await MergeClaster(claster));
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
            foreach (var polygon in claster.Polygons)
            {
                if (result == null)
                {
                    result = polygon;
                    continue;
                }

                result = await _clipper.Union(result, polygon);
            }

            return result;
        }
    }
}