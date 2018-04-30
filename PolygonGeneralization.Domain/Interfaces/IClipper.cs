using System.Collections.Generic;
using System.Threading.Tasks;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain.Interfaces
{
    public interface IClipper
    {
        List<Polygon> Union(Polygon a, Polygon b, double minDistance);
    }
}