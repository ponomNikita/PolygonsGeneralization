using System.Collections.Generic;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain
{
    public interface IGeneralizePolygonStrategy
    {
        List<Polygon> Generalize(List<Claster> clasters, double minDistance);
    }
}