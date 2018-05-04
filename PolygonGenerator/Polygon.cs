
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace PolygonGenerator
{
    public class Polygon
    {
        public Polygon(Point[] points)
        {
            Points = points.ToList();
        }

        public List<Point> Points { get; set; }

        public Point GetMassCenter()
        {
            var x = Points.Sum(p => p.X) / Points.Count;
            var y = Points.Sum(p => p.Y) / Points.Count;

            return new Point(x, y);
        }
    }
}
