using System.Drawing;
using System.Linq;
using PolygonGeneralization.WinForms.Interfaces;
using PolygonGeneralization.WinForms.Models;

namespace PolygonGeneralization.WinForms
{
    public class Drawer : IDrawer
    {
        private readonly Graphics _graphics;
        private readonly Pen _pen;
        public Drawer(Graphics graphics)
        {
            _graphics = graphics;
            _pen = new Pen(Color.Blue);
        }

        public void DrawLine(Point2D a, Point2D b)
        {
            _graphics.DrawLine(_pen, new Point(a.X, a.Y), new Point(a.X, a.Y));
        }

        public void DrawPolygon(Point2D[] points)
        {
            _graphics.DrawPolygon(_pen, points.Select(p => new Point(p.X, p.Y)).ToArray());
        }
    }
}