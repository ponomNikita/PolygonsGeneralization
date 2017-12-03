using System.Linq;
using PolygonGeneralization.Domain.Models;
using PolygonGeneralization.WinForms.Interfaces;

namespace PolygonGeneralization.WinForms.Models
{
    public class DrawablePolygon : IDrawable
    {
        public DrawablePolygon(Polygon geometry, ScreenAdapter _screenAdapter, IDrawerFactory drawerFactory)
        {
            Geometry = geometry;
            this._screenAdapter = _screenAdapter;
            _drawerFactory = drawerFactory;
        }

        public Polygon Geometry { get; }

        private readonly ScreenAdapter _screenAdapter;

        private readonly IDrawerFactory _drawerFactory;

        public bool IsVisible()
        {
            foreach (var path in Geometry.Paths)
            {
                foreach (var point in path.Points)
                {
                    if (_screenAdapter.Bbox.HasPoint(point))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void Draw()
        {
            var drawer = _drawerFactory.CreateDrawer();
            var points = Geometry.Paths.SelectMany(p => p.Points).OrderBy(p => p.OrderNumber)
                .Select(p => _screenAdapter.ToPixel(p))
                .ToArray();

            if (points.Length > 2)
                drawer.DrawPolygon(points);
        }
    }
}
