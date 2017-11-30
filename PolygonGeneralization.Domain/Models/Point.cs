using System;

namespace PolygonGeneralization.Domain.Models
{
    public class Point : Entity
    {

        /// <param name="x">Longitude</param>
        /// <param name="y">Latitude</param>
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

        protected Point()
        { }

        static Point()
        {
            EmptyPoint = new Point(double.NaN, double.NaN);
        }
        public static Point EmptyPoint { get; }

        public virtual Path Path { get; set; }

        public Guid PathId { get; set; }

        public double X { get; set; } // Longitude
        public double Y { get; set; } // Latitude

        public int OrderNumber { get; set; }

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
