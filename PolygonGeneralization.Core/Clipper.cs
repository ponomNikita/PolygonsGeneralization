using System;
using System.Collections.Generic;
using System.Linq;

namespace PolygonGeneralization.Core
{
    using Paths = List<List<PointD>>;
    using Path = List<PointD>;
    public class Clipper
    {
        private Paths _subject;
        private Paths _clipping;
        private Paths _solution;

        private HashSet<PointD> _pointsSet;
        private HashSet<Edge> _edgesSet;
       
        public Clipper(Paths subject, Paths clipping)
        {
            _subject = subject;
            _clipping = clipping;
        }

        /// <summary>
        /// Only union implemented now
        /// </summary>
        public void Execute()
        {
            BuildModel();
            MarkEdges();
            BuildSolution();
        }

        private void BuildSolution()
        {
            while (_edgesSet.Count > 0)
            {
                // Строим контуры начиная с внешнего
                BuildContour();
            }
        }

        private void BuildContour()
        {
            var initial = _edgesSet.OrderBy(e => e.A.X).ThenBy(e => e.A.Y).First();
            
            Path contour = new Path { initial.A };

            Edge current = initial;

            while (!current.B.Equals(initial.A))
            {
                current = GetNextRightEdge(current, _edgesSet);

                _edgesSet.Remove(current);

                contour.Add(current.A);
            }

            _solution.Add(contour);
        }

        private Edge GetNextRightEdge(Edge edge, HashSet<Edge> edgesSet)
        {
            var pointsForSortByClockwise = _edgesSet.Where(e => e.A == edge.B).Select(e => e.B).ToList();
            pointsForSortByClockwise.Add(edge.A);

            SortPointsByAntiClockwiseOrder(edge, pointsForSortByClockwise);

            var mostRightPoint = pointsForSortByClockwise.Last();

            return edgesSet.FirstOrDefault(e => e.B == mostRightPoint);
        }

        private void SortPointsByAntiClockwiseOrder(Edge edge, List<PointD> points)
        {
            points.Sort(new AntiClockwiseOrderComparer(edge.B, edge.A));
        }

        private void MarkEdges()
        {
            foreach (var edge in _edgesSet)
            {
                if (edge.IsFromSubject && !edge.IsFromClipping && !edge.IsInside(_clipping) ||
                    edge.IsFromClipping && !edge.IsFromSubject && !edge.IsInside(_subject))
                {
                    edge.IsResult = true;
                }
            }

            _edgesSet.RemoveWhere(e => !e.IsResult);
        }

        private void BuildModel()
        {
            if (_solution == null)
                _solution = new Paths();

            if (_pointsSet == null)
                _pointsSet = new HashSet<PointD>();

            if (_edgesSet == null)
                _edgesSet = new HashSet<Edge>();

            _solution.Clear();
            _pointsSet.Clear();
            _edgesSet.Clear();

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

            var edgesList = _edgesSet.ToList();

            foreach (var existing in edgesList)
            {
                var subdivision = newEdge.GetSubdivision(existing);

                var existingEdgeSubdivision = existing.GetSubdivision(newEdge);
                if (existingEdgeSubdivision.Item2 != null)
                {
                    existingEdgeSubdivision.Item1.IsFromSubject = existing.IsFromSubject;
                    existingEdgeSubdivision.Item1.IsFromClipping = existing.IsFromClipping;
                    existingEdgeSubdivision.Item2.IsFromSubject = existing.IsFromSubject;
                    existingEdgeSubdivision.Item2.IsFromClipping = existing.IsFromClipping;

                    _edgesSet.Remove(existing);
                    _edgesSet.Add(existingEdgeSubdivision.Item1);
                    _edgesSet.Add(existingEdgeSubdivision.Item2);
                }

                if (subdivision.Item2 != null)
                {
                    subdivision.Item1.IsFromSubject = newEdge.IsFromSubject;
                    subdivision.Item1.IsFromClipping = newEdge.IsFromClipping;
                    subdivision.Item2.IsFromSubject = newEdge.IsFromSubject;
                    subdivision.Item2.IsFromClipping = newEdge.IsFromClipping;

                    InsertEdge(subdivision.Item1);
                    InsertEdge(subdivision.Item2);

                    success = false;
                    break;
                }
            }

            if (success)
            {
                _edgesSet.Add(newEdge);
                _pointsSet.Add(newEdge.A);
                _pointsSet.Add(newEdge.B);
            }
        }
    }

    public class AntiClockwiseOrderComparer : IComparer<PointD>
    {
        private readonly PointD _origin;
        private PointD _reference;

        public AntiClockwiseOrderComparer(PointD origin, PointD reference)
        {
            _origin = origin;
            _reference = reference - origin;
        }

        /// <summary>
        /// https://stackoverflow.com/questions/7774241/sort-points-by-angle-from-given-axis
        /// </summary>
        public int Compare(PointD x, PointD y)
        {
            var da = x - _origin;
            var db = y - _origin;

            var detB = Xp(_reference, db);

            if (Math.Abs(detB) < Double.Epsilon && db.X * _reference.X + db.Y * _reference.Y >= 0)
            {
                return 1;
            }

            var detA = Xp(_reference, da);

            if (Math.Abs(detA) < Double.Epsilon && da.X * _reference.X + da.Y * _reference.Y >= 0)
            {
                return -1;
            }

            if (detA * detB >= 0)
                return Xp(da, db) > 0 ? -1 : 1;

            return detA > 0 ? -1 : 1;
        }

        private double Xp(PointD a, PointD b)
        {
            return a.X * b.Y - a.Y * b.X;
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

        public bool IsInside(Path contour, bool includeBorders = true)
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

        public bool IsInside(Paths contours)
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

    public class Edge
    {
        public Edge(PointD a, PointD b, bool isFromSubject, bool isFromClipping)
            : this(a, b)
        {
            IsFromClipping = isFromClipping;
            IsFromSubject = isFromSubject;
        }
        public Edge(PointD a, PointD b)
        {
            A = a;
            B = b;
        }

        public PointD A { get; }
        public PointD B { get; }

        public bool IsFromSubject { get; set; }
        public bool IsFromClipping { get; set; }
        public bool IsResult { get; set; }

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

        /// <summary>
        /// Метод проверяет лежит ли ребро внутри контура. 
        /// В методе  нет проверки на пересечение отрезком дырки или выпуклости, 
        /// так как после построения модели нет пересекающихся ребер.
        /// </summary>
        /// <param name="contour"></param>
        /// <returns></returns>
        public bool IsInside(Paths contour)
        {
            return A.IsInside(contour) && B.IsInside(contour);
        }

        public bool IsInside(Path contour)
        {
            return A.IsInside(contour) && B.IsInside(contour);
        }
    }
}
