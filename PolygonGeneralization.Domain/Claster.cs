using System.Collections.Generic;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain
{
    public class Claster
    {
        public Claster()
        {
            Polygons = new List<Polygon>();
        }
            
        public List<Polygon> Polygons { get; }
            
    }
}