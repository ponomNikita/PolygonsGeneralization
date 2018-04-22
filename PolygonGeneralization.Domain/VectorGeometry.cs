using System;
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

            var projection = a + (b - a) * t;
            
            return new Tuple<Point, double, double>(projection, DistanceSqr(projection, p), t);
        }

        public double Dot(Point a, Point b)
        {
            return a.X * b.X + a.Y * b.Y;
        }
        
        public double DistanceSqr(Point a, Point b)
        {
            return (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);
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
            return - Math.Sign((b.X - a.X) * (p.Y - a.Y) - (b.Y - a.Y) * (p.X - a.X));
            //return Math.Sign((p.X - b.X) * (b.Y - a.Y));
        }
    }
}