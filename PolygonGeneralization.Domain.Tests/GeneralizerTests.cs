
using System.Linq;
using NUnit.Framework;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain.Tests
{
    [TestFixture]
    public class GeneralizerTests
    {
        private readonly Generalizer _generalizer = new Generalizer();

        [Test]
        public void GetConvexHullOfSeparatedPolygonsTests()
        {
            var polygonA = new Polygon(new Path(new []
            {
                new Point(4, 4),
                new Point(0, 4),
                new Point(0, 0),
                new Point(4, 0),
            }));

            var polygonB = new Polygon(new Path(new[]
            {
                new Point(9, 4),
                new Point(5, 4),
                new Point(5, 0),
                new Point(9, 0),
            }));

            var expecetdPointArray = new[]
            {
                new Point(0, 0),
                new Point(9, 0),
                new Point(9, 4),
                new Point(0, 4),
            };

            var actual = _generalizer.GetConvexHull(polygonA, polygonB);

            var actualPointsArray = actual.Paths.SelectMany(p => p.Points).ToArray();

            CollectionAssert.AreEqual(expecetdPointArray, actualPointsArray);
        }

        [Test]
        public void GetConvexHullOfFourSeparatedPolygonsTests()
        {
            var polygonA = new Polygon(new Path(new[]
            {
                new Point(4, 3),
                new Point(0, 3),
                new Point(0, 0),
                new Point(4, 0),
            }));

            var polygonB = new Polygon(new Path(new[]
            {
                new Point(9, 4),
                new Point(5, 4),
                new Point(5, 1),
                new Point(9, 1),
            }));

            var polygonC = new Polygon(new Path(new[]
            {
                new Point(7, 7),
                new Point(2, 7),
                new Point(2, 4),
                new Point(7, 4),
            }));

            var polygonD = new Polygon(new Path(new[]
            {
                new Point(11, 8),
                new Point(8, 8),
                new Point(8, 5),
                new Point(11, 5),
            }));

            var expecetdPointArray = new[]
            {
                new Point(0, 0),
                new Point(4, 0),
                new Point(9, 1),
                new Point(11, 5),
                new Point(11, 8),
                new Point(8, 8),
                new Point(2, 7),
                new Point(0, 3),
            };

            var actual = _generalizer.GetConvexHull(polygonA, polygonB, polygonC, polygonD);

            var actualPointsArray = actual.Paths.SelectMany(p => p.Points).ToArray();

            CollectionAssert.AreEqual(expecetdPointArray, actualPointsArray);
        }

        [Test]
        public void GetConvexHullOfTwoSquares()
        {
            var polygonA = new Polygon(new Path(new[]
            {
                new Point(4, 4),
                new Point(0, 4),
                new Point(0, 0),
                new Point(4, 0),
            }));

            var polygonB = new Polygon(new Path(new[]
            {
                new Point(8, 8),
                new Point(2, 8),
                new Point(2, 2),
                new Point(8, 2),
            }));

            var expecetdPointArray = new[]
             {
                new Point(0, 0),
                new Point(4, 0),
                new Point(8, 2),
                new Point(8, 8),
                new Point(2, 8),
                new Point(0, 4)
            };

            var actual = _generalizer.GetConvexHull(polygonA, polygonB);

            var actualPointsArray = actual.Paths.SelectMany(p => p.Points).ToArray();

            CollectionAssert.AreEqual(expecetdPointArray, actualPointsArray);
        }
    }
}
