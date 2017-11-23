using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain.Interfaces
{
    public interface IGisDataReader
    {
        Map ReadFromFile(string filename);
    }
}