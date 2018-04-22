using System.Collections.Generic;
using NUnit.Framework;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain.Tests
{
    using LinearGeneralizer;

    public class LinearGeneralizerTests
    {
        private LinearGeneralizer _sut = new LinearGeneralizer(new GeneralizerOptions { MaxDifferenceInPercent = 15 });

        public static IEnumerable<object[]> SimplifyTestsCases = new List<object[]>
        {
            new object[]
            {
                new Point[]
                {
                    new Point(0, 0), new Point(2, 0), new Point(4, 0), new Point(4, 2), new Point(4, 4), new Point(0, 4),   
                },
                new Point[]
                {
                    new Point(0, 0), new Point(4, 0), new Point(4, 4), new Point(0, 4),   
                },
            },
            new object[]
            {
                new Point[]
                {
                    new Point(0, 0), new Point(2, -1), new Point(4, 0), new Point(3, 2), new Point(4, 4), new Point(0, 4),   
                },
                new Point[]
                {
                    new Point(0, 0), new Point(4, 0), new Point(4, 4), new Point(0, 4),   
                },
            },
            new object[]
            {
                new Point[]
                {
                    new Point(0, 0), new Point(2, 0.5), new Point(4, 0), 
                    new Point(3.5, 2), new Point(4, 4), new Point(0, 4),   
                },
                new Point[]
                {
                    new Point(0, 0), new Point(4, 0), new Point(4, 4), new Point(0, 4),   
                },
            },
            new object[]
            {
                new Point[]
                {
                    new Point(0, 0), new Point(1, 0.5), new Point(2, -0.5), new Point(4, 0),
                    new Point(3.5, 2), new Point(4, 4), new Point(0, 4),   
                },
                new Point[]
                {
                    new Point(0, 0), new Point(4, 0), new Point(4, 4), new Point(0, 4),   
                },
            },
            new object[]
            {
                new Point[]
                {
                    new Point(0, 0), new Point(2, 0), new Point(3, 0), new Point(4, 0), new Point(4, 1),
                    new Point(4, 2), new Point(4, 4), new Point(0, 4),   
                },
                new Point[]
                {
                    new Point(0, 0), new Point(4, 0), new Point(4, 4), new Point(0, 4),   
                },
            },
        };

        [TestCaseSource(nameof(SimplifyTestsCases))]
        public void SimplifyTests(Point[] points, Point[] expected)
        {
            var actual = _sut.Simplify(points);
            
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}