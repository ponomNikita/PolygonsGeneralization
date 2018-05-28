using System.Collections.Generic;
using System.Linq;
using ClipperLib;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Domain.Models;
using PolygonGeneralization.Domain.SimpleClipper;

namespace PolygonGeneralization.Domain
{
    public class ExternalClipper : SuperClipper
    {
        private readonly Clipper _clipper = new Clipper();

        public ExternalClipper(ILogger logger) : base(logger)
        {
        }

        public override IEnumerable<Point> UnionPaths(IEnumerable<Point> pathA, IEnumerable<Point> pathB, double minDistance)
        {
            _clipper.Clear();
            
            var subj = pathA.Select(p => new IntPoint((int)p.X, (int)p.Y)).ToList();
            var clipping =pathB.Select(p => new IntPoint((int)p.X, (int)p.Y)).ToList();
            _clipper.AddPolygon(subj, PolyType.ptSubject);
            _clipper.AddPolygon(clipping, PolyType.ptClip);

            var solution = new List<List<IntPoint>>();

            _clipper.Execute(ClipType.ctUnion, solution);

            return solution.First().Select(p => new Point(p.X, p.Y));
        }

        private bool Close(Point a, Point b)
        {
            return VectorGeometry.DistanceSqr(a, b) < 0.1f;
        }
    }
}