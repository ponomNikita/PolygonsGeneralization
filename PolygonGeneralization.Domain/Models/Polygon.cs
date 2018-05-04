using System;
using System.Collections.Generic;
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
        public double Diameter { get; private set; }

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
        
        /// <summary>
        /// Построение эпсилон окресности полигона.
        /// </summary>
        /// <param name="delta">Дельта, на которую увеличится расстояние от цетра масс до вершин</param>
        public Polygon GetIncreasePolygon(double delta)
        {
            var points = new List<Point>();
            foreach (var point in Paths[0].Points)
            {
                var length = Math.Sqrt((point.X - MassCenter.X) * (point.X - MassCenter.X) +
                             (point.Y - MassCenter.Y) * (point.Y - MassCenter.Y));

                var coeff = (delta + length) / length;
                var modifiedPoint = new Point(MassCenter.X + (point.X - MassCenter.X) * coeff,
                    MassCenter.Y + (point.Y - MassCenter.Y) * coeff);
                
                points.Add(modifiedPoint);
            }
            
            return new Polygon(new []{new Path(points.ToArray())}.Union(Paths.Skip(1)).ToArray());
        }

        public void CalculateMassCenter()
        {
            MassCenter = GetMassCenter();
        }

        public void CalculateDiameter()
        {
            double maxDiameter = 0;
            for (var i = 0; i < Paths[0].Points.Count; i++)
            {
                maxDiameter = i != Paths[0].Points.Count - 1 
                    ? Math.Max(maxDiameter, SqrDistance(Paths[0].Points[i], Paths[0].Points[i + 1]))
                    : Math.Max(maxDiameter, SqrDistance(Paths[0].Points[i], Paths[0].Points[0]));
            }

            Diameter = maxDiameter;
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
        
        private double SqrDistance(Point a, Point b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }
    }
}
