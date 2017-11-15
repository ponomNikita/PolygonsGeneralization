using System;

namespace PolygonGeneralization.Domain.Models
{
    public class Path
    {
        public Path(params Point[] points)
        {
            Points = points;
        }

        public Point[] Points { get; }

        public void TransformToR3()
        {
            for (int i = 0; i < Points.Length; i++)
            {
                Points[i] = LanLonToR3(Points[i]);
            }
        }

        private Point LanLonToR3(Point p)
        {
            double lonRad = 180 * p.X / Math.PI; // Перевод градусов в радианы
            double latRad = 180 * p.Y / Math.PI;

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
