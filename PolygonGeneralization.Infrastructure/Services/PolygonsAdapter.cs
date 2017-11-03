using System.Collections.Generic;
using System.Linq;
using PolygonGeneralization.Core;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Infrastructure.Services
{
    public class PolygonsAdapter
    {
        public List<List<PointD>> GetPaths(Polygon polygon)
        {
            var paths = new List<List<PointD>>();
            foreach (var aPath in polygon.Paths)
            {
                var path = new List<PointD>();

                foreach (var aPoint in aPath.Points)
                {
                    path.Add(new PointD(aPoint.X, aPoint.Y));
                }

                paths.Add(path);
            }

            return paths;
        }

        public Polygon GetPolygon(List<List<PointD>> paths)
        {
            var result = new Polygon();
            foreach (var path in paths)
            {
                var contour = new Path(path.Select(p => new Point(p.X, p.Y)).ToArray());
                result.AddPath(contour);
            }

            return result;
        }
    }
}
