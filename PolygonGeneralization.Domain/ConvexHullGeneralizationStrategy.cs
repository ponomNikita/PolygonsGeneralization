using System;
using System.Collections.Generic;
using System.Linq;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain
{
    public class ConvexHullGeneralizationStrategy : IGeneralizePolygonStrategy
    {
        public List<Polygon> Generalize(List<Claster> clasters, double minDistance)
        {
            var resultPolygons = new List<Polygon>();
            foreach (var claster in clasters)
            {
                resultPolygons.Add(GetConvexHull(claster.Polygons.ToArray()));
            }

            return resultPolygons;
        }

        public Polygon GetConvexHull(params Polygon[] polygons)
        {
            if (polygons == null || polygons.Length == 0)
            {
                throw new ArgumentNullException(nameof(polygons));
            }
            
            if (polygons.Length == 1)
                return polygons.First();
            
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