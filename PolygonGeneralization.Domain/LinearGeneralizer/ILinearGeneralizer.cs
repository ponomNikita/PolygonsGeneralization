using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain.LinearGeneralizer
{
    public interface ILinearGeneralizer
    {
        Point[] Simplify(Point[] points);
    }
}