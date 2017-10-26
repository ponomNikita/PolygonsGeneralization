using System.Collections.Generic;

namespace PolygonGeneralization.Core
{
    public class Polygon
    {
        public Polygon(List<List<IntPoint>> contours)
        {
            _contours = contours;
        }
        public Polygon()
        {
            _contours = new List<List<IntPoint>>();
        }

        private readonly List<List<IntPoint>> _contours;

        public int ContoursCount => _contours.Count;

        public List<List<IntPoint>> GetContours()
        {
            return _contours;
        }

        public Polygon Union(Polygon clipping)
        {
            var clipper = new Clipper();
            clipper.AddPaths(_contours, PolyType.ptSubject, true);
            clipper.AddPaths(clipping.GetContours(), PolyType.ptClip, true);

            var solution = new Polygon();
            clipper.Execute(ClipType.ctUnion, solution.GetContours(), PolyFillType.pftNonZero);

            return solution;
        }
    }
}
