using System;
using System.Collections.Generic;

namespace PolygonGeneralization.Core
{
    public struct PointD
    {
        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        public PointD(PointD pt)
        {
            X = pt.X;
            Y = pt.Y;
        }

        static PointD()
        {
            EmptyPoint = new PointD(double.NaN, double.NaN);
        }
        public static PointD EmptyPoint { get; }

        public double X { get; }
        public double Y { get; }

        public static bool operator ==(PointD a, PointD b)
        {
            return a.X.Equals(b.X) && a.Y.Equals(b.Y);
        }

        public static bool operator !=(PointD a, PointD b)
        {
            return !a.X.Equals(b.X) || !a.Y.Equals(b.Y);
        }

        public static PointD operator -(PointD a, PointD b)
        {
            return new PointD(a.X - b.X, a.Y - b.Y);
        }

        public static PointD operator +(PointD a, PointD b)
        {
            return new PointD(a.X + b.X, a.Y + b.Y);
        }

        public static PointD operator *(PointD a, double c)
        {
            return new PointD(a.X * c, a.Y  * c);
        }

        public override bool Equals(object obj)
        {
            if (obj is PointD)
            {
                var other = (PointD)obj;
                return other.X.Equals(X) && other.Y.Equals(Y);
            }

            return false;
        }
        public bool Equals(PointD other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return X.GetHashCode() * 397 + Y.GetHashCode();
            }
        }

        public bool IsInside(List<PointD> contour, bool includeBorders = true)
        {
            int i, j;
            bool isInside = false;
            int size = contour.Count;

            for (i = 0, j = size - 1; i < size; j = i++)
            {
                if (IsOnEdge(contour[j], contour[i]))
                    return includeBorders;

                if (((contour[i].Y > Y) != (contour[j].Y > Y)) && (X < (contour[j].X - contour[i].X) * (Y - contour[i].Y) / (contour[j].Y - contour[i].Y) + contour[i].X))
                    isInside = !isInside;
            }
            return isInside;
        }

        public bool IsInside(List<List<PointD>> contours)
        {
            if (contours == null || contours.Count == 0)
            {
                throw new ArgumentException(nameof(contours));
            }

            bool isInOuterContour = IsInside(contours[0]);
            bool isInInnerContour = false;

            if (contours.Count > 1)
            {
                for (int i = 1; i < contours.Count; i++)
                {
                    if (IsInside(contours[i], false))
                    {
                        isInInnerContour = true;
                        break;
                    }
                }
            }

            return isInOuterContour && !isInInnerContour;
        }

        public bool IsOnEdge(Edge edge)
        {
            return IsOnEdge(edge.A, edge.B);
        }

        public bool IsOnEdge(PointD a, PointD b)
        {
            if (a == b)
            {
                throw new ArgumentException("Points a and b must be not equal");
            }

            var isOnLine = Math.Abs((X - a.X)*(b.Y - a.Y) - 
                                    (Y - a.Y)*(b.X - a.X)) < Double.Epsilon;

            return isOnLine && 
                   (X >= a.X && X <= b.X && a.X < b.X || 
                    X <= a.X && X >= b.X && a.X > b.X ||
                    Y >= a.Y && Y <= b.Y && a.Y < b.Y ||
                    Y <= a.Y && Y >= b.Y && a.Y > b.Y);
        }
    }
}