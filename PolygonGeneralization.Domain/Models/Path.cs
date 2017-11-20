using System;
using System.Collections.Generic;
using System.Linq;

namespace PolygonGeneralization.Domain.Models
{
    public class Path : Entity
    {
        public Path(params Point[] points)
        {
            Points = points;
        }

        public virtual ICollection<Point> Points { get; }

        public void TransformToR3()
        {
            var points = Points.ToList();
            for (int i = 0; i < points.Count; i++)
            {
                points[i] = LanLonToR3(points[i]);
            }
        }

        private Point LanLonToR3(Point p)
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

            return new Point(x, y);
        }
    }
}
