using System.Collections.Generic;
using NUnit.Framework;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain.Tests
{
    public class VectorGeometryTests
    {
        public static IEnumerable<object[]> CalculateProjectionTestCases =>
            new List<object[]>
            {
                /*
                 *     W                V                    P      expectedProjection   distance
                 */
                new object[]
                {
                    new Point(0, 0), new Point(10, 0), new Point(5, 5), new Point(5, 0), 25
                },
                new object[]
                {
                    new Point(0, 0), new Point(10, 0), new Point(10, 5), new Point(10, 0), 25
                },
                new object[]
                {
                    new Point(0, 0), new Point(10, 0), new Point(20, 5), new Point(10, 0), 125
                },
                new object[]
                {
                    new Point(0, 0), new Point(10, 0), new Point(0, 5), new Point(0, 0), 25
                },
                new object[]
                {
                    new Point(0, 0), new Point(10, 0), new Point(-10, 5), new Point(0, 0), 125
                },
                new object[]
                {
                    new Point(0, 0), new Point(10, 0), new Point(-10, 0), new Point(0, 0), 100
                },
                new object[]
                {
                    new Point(0, 0), new Point(10, 0), new Point(20, 0), new Point(10, 0), 100
                }
            };

        [TestCaseSource(nameof(CalculateProjectionTestCases))]
        public void CalculateProjectionTests(
            Point w,
            Point v, 
            Point p, 
            Point expected, 
            double distanceSqr)
        {
            var sut = new VectorGeometry();
            var actual = sut.CalculateProjection(w, v, p);
            
            Assert.AreEqual(expected, actual.Item1);
            Assert.AreEqual(distanceSqr, actual.Item2);
        }

        public static IEnumerable<object[]> GetSideTestCases =>
            new List<object[]>
            {
                new object[]
                { new Point(0, 0), new Point(0, 10), new Point(5, 5), 1 },
                new object[]
                { new Point(0, 0), new Point(0, 10), new Point(-5, 5), -1 },
                new object[]
                { new Point(0, 0), new Point(0, 10), new Point(1, 20), 1 },
                new object[]
                { new Point(0, 0), new Point(0, 10), new Point(1, -20), 1 },
                new object[]
                { new Point(0, 0), new Point(0, 10), new Point(-1, 20), -1 },
                new object[]
                { new Point(0, 0), new Point(0, 10), new Point(-1, -20), -1 },
                new object[]
                { new Point(0, 0), new Point(0, 10), new Point(0, 5), 0 },
                new object[]
                { new Point(0, 0), new Point(0, 10), new Point(0, 0), 0 },
                new object[]
                { new Point(0, 0), new Point(0, 10), new Point(0, 10), 0 },
                new object[]
                { new Point(0, 0), new Point(0, 10), new Point(0, 15), 0 },
                new object[]
                { new Point(5, 5), new Point(0, 0), new Point(0, 5), 1 },
                new object[]
                { new Point(5, 5), new Point(0, 0), new Point(5, 0), -1 }
            };

        [TestCaseSource(nameof(GetSideTestCases))]
        public void GetSideTests(Point a, Point b, Point p, int expected)
        {
            var sut = new VectorGeometry();
            var actual = sut.GetSide(a, b, p);

            Assert.AreEqual(expected, actual);
        }
        
        public static IEnumerable<object[]> GetMinDistanceTestCases =>
            new List<object[]>
            {
                new object[]
                {
                    new Point(0, 0), new Point(5, 0), new Point(2, 2), new Point(5, 5),
                    new Point(2, 0), new Point(2, 2), 4
                },
                
                new object[]
                {
                    new Point(0, 0), new Point(5, 0), new Point(7, 2), new Point(10, 10),
                    new Point(5, 0), new Point(7, 2), 8
                },
                
                new object[]
                {
                    new Point(0, 0), new Point(5, 0), new Point(7, 0), new Point(10, 10),
                    new Point(5, 0), new Point(7, 0), 4
                },
            };

        [TestCaseSource(nameof(GetMinDistanceTestCases))]
        public void GetMinDistanceTests(Point a, Point b, Point c, Point d, 
            Point r1, Point r2, double distance)
        {
            var sut = new VectorGeometry();
            var actual = sut.GetMinDistance(a, b, c, d);

            Assert.AreEqual(distance, actual.Item3);
            Assert.AreEqual(r1, actual.Item1);
            Assert.AreEqual(r2, actual.Item2);
        }
    }
}