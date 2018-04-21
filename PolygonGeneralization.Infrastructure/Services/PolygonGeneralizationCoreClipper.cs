using System.Collections.Generic;
using PolygonGeneralization.Core;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Infrastructure.Services
{
    public class PolygonGeneralizationCoreClipper : IClipper
    {
        private readonly Clipper _clipper;
        private readonly PolygonsAdapter _adapter;

        public PolygonGeneralizationCoreClipper(Clipper clipper)
        {
            _clipper = clipper;
            _adapter = new PolygonsAdapter();
        }

        public List<Polygon> Union(Polygon a, Polygon b)
        {
            var subject = _adapter.GetPaths(a);
            var clipping = _adapter.GetPaths(b);

            _clipper.SetSubject(subject);
            _clipper.SetClipping(clipping);

            _clipper.Execute();
            return new List<Polygon>() {_adapter.GetPolygon(_clipper.GetSolution())};
        }
    }
}