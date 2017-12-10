using System.Collections.Generic;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain.Interfaces
{
    public interface IGeneralizer
    {
        List<Polygon> Generalize(Polygon[] polygons, double pxForMeter);
    }
}