using PolygonGeneralization.Domain.Models;
using PolygonGeneralization.WinForms.Interfaces;

namespace PolygonGeneralization.WinForms.Models
{
    public class DrawablePolygon : IDrawable
    {
        public DrawablePolygon(Polygon geometry, ScreenInfo screenInfo, IDrawerFactory drawerFactory)
        {
            Geometry = geometry;
            _screenInfo = screenInfo;
            _drawerFactory = drawerFactory;
        }

        public Polygon Geometry { get; }

        private readonly ScreenInfo _screenInfo;

        private readonly IDrawerFactory _drawerFactory;

        public bool IsVisible()
        {
            foreach (var path in Geometry.Paths)
            {
                foreach (var point in path.Points)
                {
                    if (point.X >= _screenInfo.WorldPosition.X &&
                        point.X < _screenInfo.WorldPosition.X + _screenInfo.ScaledWidth &&
                        point.Y >= _screenInfo.WorldPosition.Y &&
                        point.Y < _screenInfo.WorldPosition.Y + _screenInfo.ScaledHeight)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public void Draw()
        {
            using (var drawer = _drawerFactory.CreateDrawer())
            {
                //drawer.FillPolygon();
            }
        }
    }
}
