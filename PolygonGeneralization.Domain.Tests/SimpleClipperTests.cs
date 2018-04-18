using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using NUnit.Framework;
using NUnit.Framework.Constraints;
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

        [Test]
        public void UnionTest()
        {
            var sut = new SimpleClipper.SimpleClipper();
            
            var polygonA = new Polygon(new Path(
                new Point[]
                {
                    new Point(0, 0),
                    new Point(4, 0),
                    new Point(4, 4),
                    new Point(0, 4)
                }
            ));
            
            var polygonB = new Polygon(new Path(
                new Point[]
                {
                    new Point(0, 6),
                    new Point(4, 6),
                    new Point(4, 10),
                    new Point(0, 10)
                }
            ));

            var expectedPoints = new Point[]
            {
                new Point(0, 0),
                new Point(4, 0),
                new Point(4, 4),
                new Point(4, 6),
                new Point(4, 10),
                new Point(0, 10),
                new Point(0, 6),
                new Point(0, 4),

            };

            var actual = sut.Union(polygonA, polygonB).Result;
            
            CollectionAssert.AreEqual(expectedPoints, actual.Paths.Single().Points);
        }
        
        [Test]
        public void UnionTest2()
        {
            var sut = new SimpleClipper.SimpleClipper();
            
            var polygonA = new Polygon(new Path(
                new Point[]
                {
                    new Point(0, 0),
                    new Point(4, 0),
                    new Point(4, 4),
                    new Point(0, 4)
                }
            ));
            
            var polygonB = new Polygon(new Path(
                new Point[]
                {
                    new Point(5, 3),
                    new Point(8, 3),
                    new Point(8, 7),
                    new Point(5, 7),
                }
            ));

            /*var expectedPoints = new Point[]
            {
                new Point(0, 0),
                new Point(4, 0),
                new Point(4, 4),
                new Point(4, 6),
                new Point(4, 10),
                new Point(0, 10),
                new Point(0, 6),
                new Point(0, 4),

            };*/

            var actual = sut.Union(polygonA, polygonB).Result;
            
            //CollectionAssert.AreEqual(expectedPoints, actual.Paths.Single().Points);
        }

        #region GetBridge tests

        [Test]
        public void GetBridgeTest1()
        {
            var sut = new SimpleClipper.SimpleClipper();
            var pathA = new List<Point>
            {
                new Point(0, 0),
                new Point(4, 0),
                new Point(4, 4),
                new Point(0, 4),
            };
            
            var pathB = new List<Point>
            {
                new Point(6, 0),
                new Point(10, 0),
                new Point(10, 4),
                new Point(6, 4),
            };

            var bridge = sut.GetBridge(pathA, pathB);
            Assert.NotNull(bridge);
            Assert.AreEqual(4, bridge.Length);
            Assert.AreEqual(new Point(4, 0), bridge[0]);
            Assert.AreEqual(new Point(6, 0), bridge[1]);
            Assert.AreEqual(new Point(4, 4), bridge[2]);
            Assert.AreEqual(new Point(6, 4), bridge[3]);
        }

        #endregion
    }
}