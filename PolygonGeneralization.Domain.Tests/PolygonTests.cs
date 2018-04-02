using NUnit.Framework;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain.Tests
{
    [TestFixture]
    public class PolygonTests
    {
        [Test]
        public void Correct_massCenter_after_preparing()
        {
            var polygon = new Polygon(new Path[]
            {
                new Path(new Point(0, 0), new Point(4, 0), new Point(4, 4), new Point(0, 4))
            });
            
            polygon.PreparePolygon(1);

            Assert.AreEqual(new Point(2, 2), polygon.MassCenter);
        }

        [Test]
        public void Correct_epsilonArea_after_preparing()
        {
            var polygon = new Polygon(new Path[]
            {
                new Path(new Point(0, 0), new Point(4, 0), new Point(4, 4), new Point(0, 4))
            });
            
            polygon.PreparePolygon(2);
            
            Assert.AreEqual(new Path(
                new Point(-2, -2),
                new Point(6, -2),
                new Point(6, 6),
                new Point(-2, 6)), polygon.EpsilonArea);
        }
    }
}