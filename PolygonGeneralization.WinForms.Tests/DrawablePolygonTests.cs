using NUnit.Framework;
using PolygonGeneralization.Domain.Models;
using PolygonGeneralization.WinForms.Models;

namespace PolygonGeneralization.WinForms.Tests
{
    [TestFixture]
    public class DrawablePolygonTests
    {
        private DrawablePolygon drawable;
        private ScreenInfo screenInfo;

        [SetUp]
        public void SetUp()
        {
            var geometry = new Polygon(new double[][][]
            {
                new double[][]
                {
                    new [] { 0.0, 0.0},
                    new [] { 0.0, 10.0 },
                    new [] { 10.0, 10.0 },
                    new [] { 10.0, 0.0 },
                }
            });

            screenInfo = new ScreenInfo()
            {
                Height = 480,
                Width = 640,
            };

            drawable = new DrawablePolygon(geometry, screenInfo, null);
        }

        [TestCase(0.0, 0.0, 1.0, TestName = "IsVisible = true when screenWorldPosition = Point(0.0, 0.0), Scale = 1")]
        [TestCase(0.0, 0.0, 100.0, TestName = "IsVisible = true when screenWorldPosition = Point(0.0, 0.0), Scale = 100")]
        [TestCase(5.0, 6.0, 100.0, TestName = "IsVisible = true when screenWorldPosition = Point(5.0, 6.0), Scale = 100")]
        public void IsVisibleTrue(double x, double y, double scale)
        {
            screenInfo.WorldPosition = new Point(x, y);
            screenInfo.Scale = scale;

            Assert.True(drawable.IsVisible());
        }

        [TestCase(11.0, 11.0, 1.0, TestName = "IsVisible = false when screenWorldPosition = Point(11.0, 11.0), Scale = 1")]
        [TestCase(11.0, 11.0, 100.0, TestName = "IsVisible = false when screenWorldPosition = Point(11.0, 11.0), Scale = 100")]
        [TestCase(5.0, 6.0, 1000.0, TestName = "IsVisible = true when screenWorldPosition = Point(5.0, 6.0), Scale = 1000")]
        public void IsVisibleFalse(double x, double y, double scale)
        {
            screenInfo.WorldPosition = new Point(x, y);
            screenInfo.Scale = scale;

            Assert.False(drawable.IsVisible());
        }
    }
}
