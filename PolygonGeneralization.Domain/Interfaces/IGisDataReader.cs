using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain.Interfaces
{
    public interface IGisDataReader
    {
        Polygon[] ReadFromFile(string filename);
    }
}