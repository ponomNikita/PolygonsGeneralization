﻿namespace PolygonGeneralization.Domain.Models
{
    public class Point
    {
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Point(Point pt)
        {
            X = pt.X;
            Y = pt.Y;
        }

        static Point()
        {
            EmptyPoint = new Point(double.NaN, double.NaN);
        }
        public static Point EmptyPoint { get; }

        public double X { get; }
        public double Y { get; }

        public static bool operator ==(Point a, Point b)
        {
            return a.X.Equals(b.X) && a.Y.Equals(b.Y);
        }

        public static bool operator !=(Point a, Point b)
        {
            return !a.X.Equals(b.X) || !a.Y.Equals(b.Y);
        }

        public static Point operator -(Point a, Point b)
        {
            return new Point(a.X - b.X, a.Y - b.Y);
        }

        public static Point operator +(Point a, Point b)
        {
            return new Point(a.X + b.X, a.Y + b.Y);
        }

        public static Point operator *(Point a, double c)
        {
            return new Point(a.X * c, a.Y * c);
        }

        public override bool Equals(object obj)
        {
            if (obj is Point)
            {
                var other = (Point)obj;
                return other.X.Equals(X) && other.Y.Equals(Y);
            }

            return false;
        }
        public bool Equals(Point other)
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
    }
}