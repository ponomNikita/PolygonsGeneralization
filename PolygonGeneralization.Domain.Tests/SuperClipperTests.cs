using NUnit.Framework;
using PolygonGeneralization.Domain.Models;
using PolygonGeneralization.Domain.SimpleClipper;

namespace PolygonGeneralization.Domain.Tests
{
    public class SuperClipperTests
    {
        private SuperClipper _sut = new SuperClipper(new LoggerMock());

        [Test]
        public void UnionTest1()
        {
            var polygonA = new Polygon(new Path(new []
            {
                new Point(0, 0), 
                new Point(4, 0), 
                new Point(4, 4), 
                new Point(0, 4), 
            }));


            var polygonB = new Polygon(new Path(new[]
            {
                new Point(0, 5),
                new Point(4, 5),
                new Point(4, 9),
                new Point(0, 9),
            }));

            var actual = _sut.Union(polygonA, polygonB, 2);
        }
    }
}