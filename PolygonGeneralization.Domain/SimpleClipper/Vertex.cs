using System.Collections.Generic;
using System.Collections.ObjectModel;
using PolygonGeneralization.Domain.Models;
using PolygonGeneralization.Domain.SimpleClipper;

namespace PolygonGeneralization.Domain
{
    public class Vertex : Point
    {
        public Vertex(Point point)
        {
            X = point.X;
            Y = point.Y;
            
            Neigbours = new HashSet<Vertex>();
        }

        public HashSet<Vertex> Neigbours { get; }
    }
}