using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace PolygonGeneralization.Domain.Models
{
    public class Path : Entity
    {
        protected Path()
        {
            Points = new List<Point>();
        }
        public Path(params Point[] points)
        {
            var counter = 0;
            Points = points.Select(p => new Point(p) {PathId = Id, OrderNumber = counter++}).ToList();
        }

        public virtual List<Point> Points { get; set; }

        public virtual Polygon Polygon { get; set; }

        public Guid PolygonId { get; set; }

        public void TransformToR3()
        {
            foreach (var point in Points)
            {
                LanLonToR3(point);
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as Path;
            if (other != null)
            {
                return !Points.Where((t, i) => !t.Equals(other.Points[i])).Any();
            }

            return false;
        }

        private void LanLonToR3(Point p)
        {
            double lonRad = Math.PI * p.X / 180; // Перевод градусов в радианы
            double latRad = Math.PI * p.Y / 180;

            double b2 = 40409707439405.6373723;
            double a2 = 40682009280025.00;
            double cob, sib, col, sil, pp;
            double a2cob, b2sib;

            cob = Math.Cos(lonRad); sib = Math.Sin(lonRad);
            col = Math.Cos(latRad); sil = Math.Sin(latRad);

            a2cob = a2 * cob;
            b2sib = b2 * sib;
            pp = Math.Sqrt(a2cob * cob + b2sib * sib);
            double x = (a2cob * col) / pp;
            double y = (a2cob * sil) / pp;

            p.X = x;
            p.Y = y;
        }
    }
}
