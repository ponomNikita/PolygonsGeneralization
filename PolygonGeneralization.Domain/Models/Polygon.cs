using System.Collections.Generic;
using System.Collections.ObjectModel;
using PolygonGeneralization.Domain.Interfaces;

namespace PolygonGeneralization.Domain.Models
{
    public class Polygon
    {
        public Polygon(params Path[] paths)
        {
            Paths = paths;
            if (paths?.Length == 0)
            {
                Paths = new Collection<Path>();
            }
        }

        public ICollection<Path> Paths { get; }

        public void AddPath(Path path)
        {
            Paths.Add(path);
        }
    }
}
