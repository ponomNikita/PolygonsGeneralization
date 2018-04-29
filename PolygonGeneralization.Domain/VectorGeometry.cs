using System;
using System.Collections.Generic;
using System.Linq;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain
{
    public class VectorGeometry
    {
        public Tuple<Point, double, double> CalculateProjection(Point a, Point b, Point p, bool inLineSegment = true)
        {
            var segmentLength = DistanceSqr(a, b);
            if (Math.Abs(segmentLength) < Double.Epsilon)
            {
                return new Tuple<Point, double, double>(a, segmentLength, 0);
            }

            var t = inLineSegment 
                ? Math.Max(0, Math.Min(1, Dot(p - a, b - a) / segmentLength))
                : Dot(p - a, b - a) / segmentLength;

            var projection = Math.Abs(t) < Double.Epsilon 
                ? a : Math.Abs(t - 1) < Double.Epsilon ? b :
                a + (b - a) * t;
            
            return new Tuple<Point, double, double>(projection, DistanceSqr(projection, p), t);
        }

        public Tuple<Point, Point, double> GetMinDistance(Point a, Point b, Point c, Point d)
        {
            var d11 = CalculateProjection(c, d, a);
            var d12 = CalculateProjection(c, d, b);
            var d21 = CalculateProjection(a, b, c);
            var d22 = CalculateProjection(a, b, d);

            if (d11.Item2 <= d12.Item2 &&
                d11.Item2 <= d21.Item2 &&
                d11.Item2 <= d22.Item2)
            {
                return new Tuple<Point, Point, double>(a, d11.Item1, d11.Item2);
            }
            
            if (d12.Item2 <= d11.Item2 &&
                d12.Item2 <= d21.Item2 &&
                d12.Item2 <= d22.Item2)
            {
                return new Tuple<Point, Point, double>(b, d12.Item1, d12.Item2);
            }
            
            if (d21.Item2 <= d11.Item2 &&
                d21.Item2 <= d12.Item2 &&
                d21.Item2 <= d22.Item2)
            {
                return new Tuple<Point, Point, double>(d21.Item1, c, d21.Item2);
            }
            
            return new Tuple<Point, Point, double>(d22.Item1, d, d22.Item2);
        }

        public double Dot(Point a, Point b)
        {
            return a.X * b.X + a.Y * b.Y;
        }
        
        public double DistanceSqr(Point a, Point b)
        {
            return (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);
        }
        
        public double DistanceSqr(Polygon a, Polygon b)
        {
            var minDistance = double.MaxValue;

            foreach (var pointFromA in a.Paths.First().Points)
            {
                foreach (var pointFromB in b.Paths.First().Points)
                {
                    var distance = DistanceSqr(pointFromA, pointFromB);
                    minDistance = Math.Min(minDistance, distance);
                }
            }

            return minDistance;
        }
        
        public double Distance(Point a, Point b)
        {
            return Math.Sqrt(DistanceSqr(a, b));
        }

        /// <summary>
        /// Cторона с которой находится точка от вектора
        /// </summary>
        /// <returns>
        /// 1 if right
        /// -1 if left
        /// 0 if on line
        /// </returns>
        public int GetSide(Point a, Point b, Point p)
        {
            var res = (b.X - a.X) * (p.Y - a.Y) - (b.Y - a.Y) * (p.X - a.X);
            if (Math.Abs(res) < 0.000000001)
            {
                return 0;
            }
            
            return - Math.Sign(res);
        }
    }
}