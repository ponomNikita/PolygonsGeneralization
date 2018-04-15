using System;
using System.Collections.Generic;
using NUnit.Framework;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain.Tests
{
    public class SimpleClipperTests
    {
        [Test]
        public void UnionPathsTest()
        {
            var pathA = new List<Point>
            {
                new Point(0, 0),
                new Point(4, 0),
                new Point(4, 4),
                new Point(0, 4)
            };
            
            var pathB = new List<Point>
            {
                new Point(0, 6),
                new Point(4, 6),
                new Point(4, 10),
                new Point(0, 10)
            };

            var firstPair = new Tuple<Point, Point>(new Point(0, 4), new Point(0, 6));
            var secondPair = new Tuple<Point, Point>(new Point(4, 4), new Point(4, 6));

            var sut = new SimpleClipper.SimpleClipper();
            
            var expected = new Path(new Point[]
            {
                new Point(0, 0),
                new Point(4, 0),
                new Point(4, 4),
                new Point(4, 6),
                new Point(4, 10),
                new Point(0, 10),
                new Point(0, 6),
                new Point(0, 4),
                
            });

            var union = sut.UnionPaths(pathA, pathB, firstPair, secondPair);
            
            CollectionAssert.AreEqual(expected.Points, union.Points);
        }
    }
}