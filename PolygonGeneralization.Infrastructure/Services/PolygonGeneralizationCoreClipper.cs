using System.Threading.Tasks;
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

        public async Task<Polygon> Union(Polygon a, Polygon b)
        {
            var subject = _adapter.GetPaths(a);
            var clipping = _adapter.GetPaths(b);

            _clipper.SetSubject(subject);
            _clipper.SetClipping(clipping);

            var result = await ExecuteOperation();

            return result;
        }

        public Task<Polygon> ExecuteOperation()
        {
            return Task.Run(() =>
            {
                _clipper.Execute();
                return _adapter.GetPolygon(_clipper.GetSolution());
            });
        }
    }
}