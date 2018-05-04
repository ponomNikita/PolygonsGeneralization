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
        private static IEnumerable<object[]> ComparerTestCases2 => new[]
        {
            new object[] { new Point(-5, 0), new Point(-5, -5) },
            new object[] { new Point(-4, -4), new Point(-5, -5) },
            new object[] { new Point(6, 6), new Point(7, 7) },
            new object[] { new Point(0, -5), new Point(-5, 0) },
            new object[] { new Point(0, -5), new Point(5, 0) },
            new object[] { new Point(4, 0), new Point(5, 0) },
            new object[] { new Point(-5, 0), new Point(-6, 0) },
            new object[] { new Point(4, -1), new Point(-3, 3) },
            new object[] { new Point(0, 10), new Point(2, 2) },
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