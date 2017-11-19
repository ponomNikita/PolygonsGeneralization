using System;
using PolygonGeneralization.WinForms.Models;

namespace PolygonGeneralization.WinForms.Interfaces
{
    public interface IDrawer
    {
        void DrawLine(Point2D a, Point2D b);
        void DrawPolygon(Point2D[] points);
    }
}