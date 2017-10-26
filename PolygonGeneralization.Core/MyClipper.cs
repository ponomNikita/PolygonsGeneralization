using System;
using System.Collections.Generic;

namespace PolygonGeneralization.Core
{
    using Paths = List<List<PointD>>;
    using Path = List<PointD>;
    public class MyClipper
    {
        private Paths _subject;
        private Paths _clipping;
        private Paths _solution;

        private HashSet<PointD> _pointsSet;
        private HashSet<Edge> _edgesSet;
       
        public MyClipper(Paths subject, Paths clipping)
        {
            _subject = subject;
            _clipping = clipping;
        }

        public void Execute()
        {
            if(_solution == null)
                _solution = new Paths();

            if (_pointsSet == null)
                _pointsSet = new HashSet<PointD>();

            if (_edgesSet == null)
                _edgesSet = new HashSet<Edge>();

            _solution.Clear();
            _pointsSet.Clear();
            _edgesSet.Clear();

            BuildModel();
        }

        private void BuildModel()
        {
            BuildFrom(_subject, true);
            BuildFrom(_clipping, false);
        }

        private void BuildFrom(Paths countours, bool isSubject)
        {
            foreach (var contour in countours)
            {
                var size = contour.Count;
                for (int i = 0; i < size; i++)
                {
                    _pointsSet.Add(contour[i]);

                    var newEdge = i == size - 1
                        ? new Edge(contour[i], contour[0])
                        : new Edge(contour[i], contour[i + 1]);

                    newEdge.IsFromSubject = isSubject; // TODO возможно второе поле лишнее
                    newEdge.IsFromClipping = !isSubject;

                    InsertEdge(newEdge);
                }
            }
        }

        private void InsertEdge(Edge newEdge)
        {
            bool success = true;

            foreach (var existing in _edgesSet)
            {
                Tuple<Edge, Edge> subdivision = newEdge.GetSubdivision(existing);

                if (subdivision.Item2 != null)
                {
                    InsertEdge(subdivision.Item1);
                    InsertEdge(subdivision.Item2);

                    success = false;
                    break;
                }
            }

            if (success)
            {
                _edgesSet.Add(newEdge);
            }
        }
    }

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

        public bool IsInside(Path contour)
        {
            int i, j;
            bool isInside = false;
            int size = contour.Count;

            for (i = 0, j = size - 1; i < size; j = i++)
            {
                if (IsOnEdge(contour[j], contour[i]))
                    return true;

                if (((contour[i].Y > Y) != (contour[j].Y > Y)) && (X < (contour[j].X - contour[i].X) * (Y - contour[i].Y) / (contour[j].Y - contour[i].Y) + contour[i].X))
                    isInside = !isInside;
            }
            return isInside;
        }

        public bool IsOnEdge(Edge edge)
        {
            return IsOnEdge(edge.A, edge.B);
        }

        public bool IsOnEdge(PointD a, PointD b)
        {
            var isOnLine = Math.Abs((X - a.X)*(b.Y - a.Y) - 
                (Y - a.Y)*(b.X - a.X)) < Double.Epsilon;

            return isOnLine && (X >= a.X && X <= b.X && a.X < b.X ||
                   Y >= a.Y && Y <= b.Y && a.Y < b.Y);
        }
    }

    public class Edge
    {
        public Edge(PointD a, PointD b)
        {
            A = a;
            B = b;
        }

        public PointD A { get; }
        public PointD B { get; }

        public bool IsFromSubject { get; set; }
        public bool IsFromClipping { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as Edge;
            if (other != null)
            {
                return A.Equals(other.A) && B.Equals(other.B) ||
                       A.Equals(other.B) && B.Equals(other.A);
            }

            return false;
        }

        protected bool Equals(Edge other)
        {
            return A.Equals(other.A) && B.Equals(other.B) ||
                   A.Equals(other.B) && B.Equals(other.A);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return A.GetHashCode()*397 + B.GetHashCode();
            }
        }

        public Tuple<Edge, Edge> GetSubdivision(Edge other)
        {
            PointD intersectionPoint = GetIntersection(other);

            if (!intersectionPoint.Equals(PointD.EmptyPoint) && 
                !intersectionPoint.Equals(A) && 
                !intersectionPoint.Equals(B))
            {
                return new Tuple<Edge, Edge>(
                       new Edge(A, intersectionPoint),
                       new Edge(intersectionPoint, B));
            }

            return new Tuple<Edge, Edge>(this, null);
        }

        public PointD GetIntersection(Edge other)
        {
            var dir1 = B - A;
            var dir2 = other.B - other.A;

            //считаем уравнения прямых проходящих через отрезки
            var a1 = -dir1.Y;
            var b1 = dir1.X;
            var d1 = -a1*A.X - b1*A.Y;

            var a2 = -dir2.Y;
            var b2 = dir2.X;
            var d2 = -a2*other.A.X - b2*other.A.Y;

            //подставляем концы отрезков, для выяснения в каких полуплоскотях они
            var firstSegmentStart = a2 * A.X + b2 * A.Y + d2;
            var firstSegmentEnd = a2 * B.X + b2 * B.Y + d2;

            var secondSegmentStart = a1 * other.A.X + b1 * other.A.Y + d1;
            var secondSegmentEnd = a1 * other.B.X + b1 * other.B.Y + d1;

            //если концы одного отрезка имеют один знак, значит он в одной полуплоскости и пересечения нет.
            if (firstSegmentStart * firstSegmentEnd > 0 || secondSegmentStart * secondSegmentEnd > 0)
                return PointD.EmptyPoint;

            var u = firstSegmentStart / (firstSegmentStart - firstSegmentEnd);
            
            return A + (dir1 * u);
        }
    }
}
