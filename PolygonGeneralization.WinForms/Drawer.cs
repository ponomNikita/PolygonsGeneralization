using System.Drawing;
using System.Linq;
using PolygonGeneralization.WinForms.Interfaces;
using PolygonGeneralization.WinForms.Models;

namespace PolygonGeneralization.WinForms
{
    public class Drawer : IDrawer
    {
        private readonly Graphics _graphics;
        private readonly Brush _brash;
        private readonly Pen _pen;
        public Drawer(Graphics graphics)
        {
            _graphics = graphics;
            _brash = new SolidBrush(Color.Blue);
            _pen = new Pen(Color.Blue);
        }

        public void DrawLine(Point2D a, Point2D b)
        {
            _graphics.DrawLine(_pen, new Point(a.X, a.Y), new Point(a.X, a.Y));
        }

        public void FillPolygon(Point2D[] points)
        {
            _graphics.FillPolygon(_brash, points.Select(p => new Point(p.X, p.Y)).ToArray());
        }
    }
}