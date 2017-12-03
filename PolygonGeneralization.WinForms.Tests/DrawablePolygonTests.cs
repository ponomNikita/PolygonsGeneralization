using NUnit.Framework;
using PolygonGeneralization.Domain.Models;
using PolygonGeneralization.WinForms.Models;

namespace PolygonGeneralization.WinForms.Tests
{
    [TestFixture]
    public class DrawablePolygonTests
    {
        private DrawablePolygon drawable;
        private ScreenAdapter.ScreenAdapter _screenAdapter;

        [SetUp]
        public void SetUp()
        {
            var geometry = new Polygon(new[]
            {
                new[]
                {
                    new [] { 0.0, 0.0},
                    new [] { 0.0, 10.0 },
                    new [] { 10.0, 10.0 },
                    new [] { 10.0, 0.0 },
                }
            });

            drawable = new DrawablePolygon(geometry, _screenAdapter, null);
        }

        
    }
}
