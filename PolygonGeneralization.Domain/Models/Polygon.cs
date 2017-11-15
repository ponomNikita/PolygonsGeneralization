using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PolygonGeneralization.Domain.Models
{
    public class Polygon
    {
        public Polygon(params Path[] paths)
        {
            Paths = paths;
            if (paths?.Length == 0)
            {
                Paths = new Collection<Path>();
            }
        }

        public Polygon(double[][][] polygon)
        {
            Paths = new Collection<Path>();
            foreach (var contour in polygon)
            {
                var path = new Path(contour.Select(c => new Point(c[0], c[1])).ToArray());
                Paths.Add(path);
            }
        }

        public ICollection<Path> Paths { get; }

        public void AddPath(Path path)
        {
            Paths.Add(path);
        }

        public void TransformToR3()
        {
            foreach (var path in Paths)
            {
                path.TransformToR3();
            }
        }
    }
}
