using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PolygonGeneralization.Domain.Models;
using PolygonGeneralization.Domain.SimpleClipper;

namespace PolygonGeneralization.Domain.Tests
{
    public class ClockwiseOrderComparerTests
    {
        private static IEnumerable<object[]> ComparerTestCases => new[]
        {
            // Case 1
            new object[]
            {
                new Point[] 
                {
                    new Point(1, 10), 
                    new Point(10, 0),
                    new Point(10, 10),
                    new Point(10, 5)
                },
                new Point(10, 10) // expected
            },
            // Case 2
            new object[]
            {
                new Point[] 
                {
                    new Point(-1, 20), 
                    new Point(-5, 20), 
                    new Point(10, 0),
                    new Point(10, 10),
                    new Point(0, 20), 
                    new Point(10, 5)
                },
                new Point(0, 20) // expected
            },
            // Case 3
            new object[]
            {
                new Point[] 
                { 
                    new Point(0, 20), 
                    new Point(0, 15), 
                },
                new Point(0, 20) // expected
            },
            // Case 4
            new object[]
            {
                new Point[] 
                { 
                    new Point(-5, 10), 
                    new Point(0, 20), 
                },
                new Point(0, 20) // expected
            }
        };
        
        [TestCaseSource(nameof(ComparerTestCases))]
        public void ComparerTests(Point[] points, Point expected)
        {
            var sut = new ClockwiseOrderComparer(new Point(0, 0), new Point(0, 10));
            Array.Sort(points, sut);
            
            Assert.AreEqual(expected, points.First());
        }

        private static IEnumerable<object[]> ComparerTestCases2 => new[]
        {
            new object[] { new Point(-5, -5), new Point(-5, 0) },
            new object[] { new Point(-5, -5), new Point(-4, -4) },
            new object[] { new Point(7, 7), new Point(6, 6) },
            new object[] { new Point(-5, 0), new Point(0, -5) },
            new object[] { new Point(5, 0), new Point(0, -5) },
            new object[] { new Point(5, 0), new Point(4, 0) },
            new object[] { new Point(-6, 0), new Point(-5, 0) },
            new object[] { new Point(-3, 3), new Point(4, -1) },
        };
        
        
        [TestCaseSource(nameof(ComparerTestCases2))]
        public void ComparerTests2(Point a, Point b)
        {
            var sut = new ClockwiseOrderComparer(new Point(5, 5), new Point(0, 0));

            var actual = sut.Compare(a, b);
            
            Assert.True(actual < 0);
        }

    }
}