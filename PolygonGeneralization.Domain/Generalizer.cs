using System;
using System.Collections.Generic;
using System.Linq;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain
{
    public class Generalizer : IGeneralizer
    {
        public List<Polygon> Generalize(List<Polygon> polygons, double minDistance)
        {
            return polygons;
        }

        public Polygon GetConvexHull(params Polygon[] polygons)
        {
            var points = polygons.SelectMany(p => p.Paths).SelectMany(p => p.Points).ToList();

            points.Sort(new PointsByXComparator());

            int n = points.Count, k = 0;
            var convexHull = new Point[2 * n];

            // Build lower hull
            for (int i = 0; i < n; ++i)
            {
                while (k >= 2 && Cross(convexHull[k - 2], convexHull[k - 1], points[i]) <= 0)
                    k--;
                convexHull[k++] = points[i];
            }

            // Build upper hull
            for (int i = n - 2, t = k + 1; i >= 0; i--)
            {
                while (k >= t && Cross(convexHull[k - 2], convexHull[k - 1], points[i]) <= 0)
                    k--;
                convexHull[k++] = points[i];
            }

            if (k > 1)
                convexHull = convexHull.Take(k - 1).ToArray();

            return new Polygon(new Path(convexHull));
        }

        private double Cross(Point o, Point a, Point b)
        {
            return (a.X - o.X) * (b.Y - o.Y) - (a.Y - o.Y) * (b.X - o.X);
        }

        private class PointsByXComparator : IComparer<Point>
        {
            public int Compare(Point x, Point y)
            {
                if (Math.Abs(x.X - y.X) < Double.Epsilon)
                {
                    return x.Y - y.Y > 0 ? 1 : -1;
                }
                return x.X - y.X > 0 ? 1 : -1;
            }
        }
    }
}