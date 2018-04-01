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

            MassCenter = GetMassCenter();
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
            MassCenter = GetMassCenter();
        }

        public virtual Map Map { get; set; }
        public Guid MapId { get; set; }
        public virtual List<Path> Paths { get; }
        public Point MassCenter { get; private set; }

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

        public void CalculateMassCenter()
        {
            MassCenter = GetMassCenter();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Polygon;
            if (other != null)
            {
                if (other.Paths.Count != Paths.Count)
                    return false;

                for (var i = 0; i < Paths.Count; i++)
                {
                    if (!Paths[i].Equals(other.Paths[i]))
                        return false;
                }

                return true;
            }
            
            return false;
        }

        private Point GetMassCenter()
        {
            var allPoints = Paths.SelectMany(p => p.Points).ToList();
            var x = allPoints.Sum(p => p.X) / allPoints.Count();
            var y = allPoints.Sum(p => p.Y) / allPoints.Count();
            
            return new Point(x, y);
        }
    }
}
