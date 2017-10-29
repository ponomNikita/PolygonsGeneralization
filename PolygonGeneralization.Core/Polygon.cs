using System.Collections.Generic;

namespace PolygonGeneralization.Core
{
    public class Polygon
    {
        public Polygon(List<List<PointD>> contours)
        {
            _contours = contours;
        }
        public Polygon()
        {
            _contours = new List<List<PointD>>();
        }

        private readonly List<List<PointD>> _contours;

        public int ContoursCount => _contours.Count;

        public List<List<PointD>> GetContours()
        {
            return _contours;
        }

        public Polygon Union(Polygon clipping)
        {
            return new Polygon();
        }
    }
}
