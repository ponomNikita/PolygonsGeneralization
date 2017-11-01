using System;
using System.Collections.Generic;

namespace PolygonGeneralization.Core
{
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
        public bool IsInside(List<List<PointD>> contour)
        {
            return A.IsInside(contour) && B.IsInside(contour);
        }

        public bool IsInside(List<PointD> contour)
        {
            return A.IsInside(contour) && B.IsInside(contour);
        }
    }
}