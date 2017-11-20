using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain.Interfaces
{
    public interface IDbService
    {
        void SaveMap(Map map);
    }
}