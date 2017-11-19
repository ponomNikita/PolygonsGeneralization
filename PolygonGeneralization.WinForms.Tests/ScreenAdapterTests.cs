
using NUnit.Framework;
using PolygonGeneralization.Domain.Models;
using PolygonGeneralization.WinForms.Models;

namespace PolygonGeneralization.WinForms.Tests
{
    [TestFixture]
    public class ScreenAdapterTests
    {
        private int _screenWidth = 1000;
        private int _screenHeight = 500;

        [Test]
        [Ignore("Need debug test")]
        public void ScreenInfoInitialization()
        {
            var points = new []
            {
                new Point(90000, 30000),
                new Point(30000, 90000),
                new Point(33000, 33000),
            };

            var screenAdapter = new ScreenAdapter(_screenWidth, _screenHeight, points, 1);

            Assert.NotNull(screenAdapter);
            Assert.AreEqual(120, screenAdapter.Zoom);
            Assert.AreEqual(new Point(30000, 30000), screenAdapter.ZeroPoint);
            Assert.AreEqual(new Point(30000, 30000), screenAdapter.Bbox.LeftDown);
            Assert.AreEqual(new Point(150000, 90000), screenAdapter.Bbox.RightTop);
        }

        [Test]
        [Ignore("Need debug test")]
        public void PointsToPixelHasCorrectCoordinates()
        {
            var points = new[]
            {
                new Point(90000, 30000),
                new Point(30000, 90000),
                new Point(33000, 33000),
            };

            var screenAdapter = new ScreenAdapter(_screenWidth, _screenHeight, points, 1);

            var pixel0 = screenAdapter.ToPixel(points[0]);
            var pixel1 = screenAdapter.ToPixel(points[1]);
            var pixel2 = screenAdapter.ToPixel(points[2]);

            AssertPixelIsOnScreen(screenAdapter, pixel0);
            AssertPixelIsOnScreen(screenAdapter, pixel1);
            AssertPixelIsOnScreen(screenAdapter, pixel2);
        }

        private void AssertPixelIsOnScreen(ScreenAdapter screenAdapter, Point2D pixel)
        {
            Assert.True(pixel.X >= 0);
            Assert.True(pixel.X <= _screenWidth);
            
            Assert.True(pixel.Y >= 0);
            Assert.True(pixel.Y <= _screenHeight);
        }
    }
}
