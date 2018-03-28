using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PolygonGeneralization.Domain.Models
{
    public class Polygon : Entity
    {
        protected Polygon()
        {
            Paths = new List<Path>();
        }
        public Polygon(params Path[] paths)
        {
            Paths = paths.ToList();
            if (paths?.Length == 0)
            {
                Paths = new List<Path>();
            }
        }

        public Polygon(double[][][] polygon)
        {
            Paths = new List<Path>();
            foreach (var contour in polygon)
            {
                var points = contour.Select(c => new Point(c[0], c[1])).ToList();
                if (points.Count > 1 && points[0] == points[points.Count - 1])
                {
                    points.RemoveAt(points.Count - 1);
                }

                var path = new Path(points.ToArray()) {PolygonId = Id};
                Paths.Add(path);
            }
        }

        public virtual Map Map { get; set; }
        public Guid MapId { get; set; }
        public virtual List<Path> Paths { get; }

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
