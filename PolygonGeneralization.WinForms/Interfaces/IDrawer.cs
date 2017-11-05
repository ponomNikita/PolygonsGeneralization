using PolygonGeneralization.WinForms.Models;

namespace PolygonGeneralization.WinForms.Interfaces
{
    public interface IDrawer
    {
        void DrawLine(Point2D[] points);
        void FillPolygon(Point2D[] points);
    }
}