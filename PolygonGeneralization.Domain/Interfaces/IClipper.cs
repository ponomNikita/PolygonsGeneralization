using System.Threading.Tasks;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain.Interfaces
{
    public interface IClipper
    {
        Task<Polygon> Union(Polygon a, Polygon b);
    }
}