using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace PolygonGeneralization.Core.Tests
{
    [TestFixture]
    public class PolygonUnionTests
    {
        //[Test]
        //public void UnionSuccessWithSimplePolygons()
        //{
        //    var polygon = new Polygon(new List<List<IntPoint>>()
        //    {
        //        new List<IntPoint>()
        //        {
        //            new IntPoint(0, 0),
        //            new IntPoint(0, 2),
        //            new IntPoint(2, 2),
        //            new IntPoint(2, 0),
        //        }
        //    });

        //    var clipping = new Polygon(new List<List<IntPoint>>()
        //    {
        //        new List<IntPoint>()
        //        {
        //            new IntPoint(1, 1),
        //            new IntPoint(1, 3),
        //            new IntPoint(3, 3),
        //            new IntPoint(3, 1),
        //        }
        //    });

        //    var expectedContour = new List<IntPoint>
        //    {
        //        new IntPoint(3, 3),
        //        new IntPoint(1, 3),
        //        new IntPoint(1, 2),
        //        new IntPoint(0, 2),
        //        new IntPoint(0, 0),
        //        new IntPoint(2, 0),
        //        new IntPoint(2, 1),
        //        new IntPoint(3, 1),
        //    };

        //    var result = polygon.Union(clipping);

        //    Assert.NotNull(result);
        //    Assert.AreEqual(1, result.ContoursCount);
        //    AssertCorrectContour(result.GetContours().Single(), expectedContour);
        //}

        //[Test]
        //public void UnionSuccessWithPolygonWithHole()
        //{
        //    var polygon = new Polygon(new List<List<IntPoint>>()
        //    {
        //        new List<IntPoint>() // Main contour
        //        {
        //            new IntPoint(0, 0),
        //            new IntPoint(0, 6),
        //            new IntPoint(6, 6),
        //            new IntPoint(6, 0),
        //        },

        //        new List<IntPoint>() // Hole
        //        {
        //            new IntPoint(2, 2),
        //            new IntPoint(4, 2),
        //            new IntPoint(4, 4),
        //            new IntPoint(2, 4),
        //        }
        //    });

        //    var clipping = new Polygon(new List<List<IntPoint>>()
        //    {
        //        new List<IntPoint>()
        //        {
        //            new IntPoint(3, 3),
        //            new IntPoint(3, 9),
        //            new IntPoint(9, 9),
        //            new IntPoint(9, 3),
        //        }
        //    });

        //    var expectedContours = new Polygon(new List<List<IntPoint>>()
        //    {
        //        new List<IntPoint> // Main contour
        //        {
        //            new IntPoint(9, 9),
        //            new IntPoint(3, 9),
        //            new IntPoint(3, 6),
        //            new IntPoint(0, 6),
        //            new IntPoint(0, 0),
        //            new IntPoint(6, 0),
        //            new IntPoint(6, 3),
        //            new IntPoint(9, 3),
        //        },

        //        new List<IntPoint>() // Hole
        //        {
        //            new IntPoint(3, 4),
        //            new IntPoint(2, 4),
        //            new IntPoint(2, 2),
        //            new IntPoint(4, 2),
        //            new IntPoint(4, 3),
        //            new IntPoint(3, 3)
        //        }
        //    });

        //    var result = polygon.Union(clipping);

        //    Assert.NotNull(result);
        //    Assert.AreEqual(2, result.ContoursCount);
        //    AssertCorrectContour(result.GetContours()[0], expectedContours.GetContours()[0]);
        //    AssertCorrectContour(result.GetContours()[1], expectedContours.GetContours()[1]);
        //}

        //private void AssertCorrectContour(List<IntPoint> actualContour, List<IntPoint> expectedContour)
        //{
        //    Assert.AreEqual(expectedContour.Count, actualContour.Count);
        //    foreach (var point in actualContour)
        //    {
        //        Assert.Contains(point, expectedContour);
        //    }
        //}
    }
}
