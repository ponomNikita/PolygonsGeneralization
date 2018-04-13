using System.Collections.Generic;
using PolygonGeneralization.Domain.Models;
using PolygonGeneralization.Domain.SimpleClipper;

namespace PolygonGeneralization.Domain
{
    public class Vertex
    {
        public Vertex(Point point)
        {
            Point = point;
            Edges = new List<Edge>();
        }

        public Point Point { get; }
        public List<Edge> Edges { get; }

        public override int GetHashCode()
        {
            return Point.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var other = obj as Vertex;
            var point = obj as Point;
            if (other != null)
            {
                return Point.Equals(other.Point);
            }
            else if (point != null)
            {
                return point.Equals(Point);
            }

            return false;
        }
    }
}