using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PolygonGeneralization.Domain.Models;
using PolygonGeneralization.Domain.SimpleClipper;

namespace PolygonGeneralization.Domain.Tests
{
    public class AntiClockwiseOrderComparerTests
    {
        private static IEnumerable<object[]> VectorSource => new[]
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
                new Point(10, 0) // expected
            },
            // Case 2
            new object[]
            {
                new Point[] 
                {
                    new Point(-1, 20), 
                    new Point(-5, 20), 
                    new Point(0, 20), 
                    new Point(10, 0),
                    new Point(10, 10),
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
                new Point(0, 15) // expected
            }
        };
        
        [Test]
        [TestCaseSource(nameof(VectorSource))]
        public void ComparerTests(Point[] points, Point expected)
        {
            var sut = new AntiClockwiseOrderComparer(new Point(0, 0), new Point(0, 10));
            Array.Sort(points, sut);
            
            Assert.AreEqual(expected, points.First());
        }
    }
}