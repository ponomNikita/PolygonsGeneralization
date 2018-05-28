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

        public IEnumerable<Point> IncreaseContour(IEnumerable<Point> points, Point center, double delta)
        {
            var resultPoints = new List<Point>();
            foreach (var point in points)
            {
                var modifiedPoint = ScaleVector(center, point, delta);

                resultPoints.Add(modifiedPoint);
            }

            return resultPoints.ToArray();
        }

        public Point ScaleVector(Point center, Point point, double delta)
        {
            var lengthX = Math.Abs(point.X - center.X);
            var lengthY = Math.Abs(point.Y - center.Y);

            var coeffX = Math.Abs(lengthX) > Double.Epsilon ? (delta + lengthX) / lengthX : 1;
            var coeffY = Math.Abs(lengthY) > Double.Epsilon ?(delta + lengthY) / lengthY : 1;

            var modifiedPoint = new Point(center.X + (point.X - center.X) * coeffX,
                center.Y + (point.Y - center.Y) * coeffY);
            return modifiedPoint;
        }

        public IEnumerable<Point> IncreaseContour(IEnumerable<Point> points, double delta)
        {
            return IncreaseContour(points, GetMassCenter(points), delta);
        }
        public Point GetIntersection(Point a, Point b, Point c, Point d)
        {
            var dir1 = b - a;
            var dir2 = d - c;

            //считаем уравнения прямых проходящих через отрезки
            var a1 = -dir1.Y;
            var b1 = dir1.X;
            var d1 = -a1 * a.X - b1 * a.Y;

            var a2 = -dir2.Y;
            var b2 = dir2.X;
            var d2 = -a2 * c.X - b2 * c.Y;

            //подставляем концы отрезков, для выяснения в каких полуплоскотях они
            var firstSegmentStart = a2 * a.X + b2 * a.Y + d2;
            var firstSegmentEnd = a2 * b.X + b2 * b.Y + d2;

            var secondSegmentStart = a1 * c.X + b1 * c.Y + d1;
            var secondSegmentEnd = a1 * d.X + b1 * d.Y + d1;

            //если концы одного отрезка имеют один знак, значит он в одной полуплоскости и пересечения нет.
            if (firstSegmentStart * firstSegmentEnd > 0 || secondSegmentStart * secondSegmentEnd > 0)
                return null;

            var u = firstSegmentStart / (firstSegmentStart - firstSegmentEnd);

            return a + (dir1 * u);
        }

        public Point GetMassCenter(IEnumerable<Point> points)
        {
            var x = points.Sum(p => p.X) / points.Count();
            var y = points.Sum(p => p.Y) / points.Count();

            return new Point(x, y);
        }
    }
}