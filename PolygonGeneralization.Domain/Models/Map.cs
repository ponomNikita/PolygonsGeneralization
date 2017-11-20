using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PolygonGeneralization.Domain.Models
{
    public class Map : Entity
    {
        public Map(string name) : this()
        {
            Name = name;
        }
        protected Map()
        {
            Polygons = new Collection<Polygon>();
        }
        public string Name { get; set; }
        public ICollection<Polygon> Polygons { get; set; }
    }
}