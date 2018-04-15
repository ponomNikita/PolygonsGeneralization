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
            Neigbours = new List<Vertex>();
        }

        public Point Point { get; }
        public List<Vertex> Neigbours { get; }

        public override int GetHashCode()
        {
            return Point.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var vertex = obj as Vertex;
            if (vertex != null)
            {
                return Point.Equals(vertex.Point);
            }

            if (obj is Point)
            {
                return Point.Equals(((Point)obj));
            }

            return false;
        }
    }
}