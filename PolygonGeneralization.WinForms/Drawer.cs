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
        private readonly Pen _penForPoints;
        public Drawer(Graphics graphics)
        {
            _graphics = graphics;
            _pen = new Pen(Color.Blue);
            _penForPoints = new Pen(Color.Red);
        }

        public void DrawLine(Point2D a, Point2D b)
        {
            _graphics.DrawLine(_pen, new Point(a.X, a.Y), new Point(a.X, a.Y));
        }

        public void DrawPolygon(Point2D[] points)
        {
            //_graphics.FillPolygon(_pen.Brush, points.Select(p => new Point(p.X, p.Y)).ToArray());
            _graphics.DrawPolygon(_pen, points.Select(p => new Point(p.X, p.Y)).ToArray());
            //foreach (var point2D in points)
            //{
            //    _graphics.FillRectangle(_penForPoints.Brush, point2D.X, point2D.Y, 3, 3);
            //}
        }
    }
}