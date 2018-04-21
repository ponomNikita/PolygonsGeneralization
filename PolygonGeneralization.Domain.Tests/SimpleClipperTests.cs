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

            var union = sut.UnionPaths(pathA, pathB);
            
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

            var actual = sut.Union(polygonA, polygonB);
            
            CollectionAssert.AreEqual(expectedPoints, actual.Single().Paths.Single().Points);
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

            var expectedPoints = new Point[]
            {
                new Point(0, 0),
                new Point(4, 0),
                new Point(4, 3),
                new Point(5, 3),
                new Point(8, 3),
                new Point(8, 7),
                new Point(5, 7),
                new Point(5, 4),
                new Point(4, 4),
                new Point(0, 4),

            };

            var actual = sut.Union(polygonA, polygonB);
            
            CollectionAssert.AreEqual(expectedPoints, actual.Single().Paths.Single().Points);
        }
        
        [Test]
        public void UnionTest3()
        {
            var sut = new SimpleClipper.SimpleClipper();
            
            var polygonA = new Polygon(new Path(
                new Point[]
                {
                    new Point(0, 0),
                    new Point(2, 2),
                    new Point(0, 4),
                    new Point(-2, 2)
                }
            ));
            
            var polygonB = new Polygon(new Path(
                new Point[]
                {
                    new Point(5, 0),
                    new Point(7, 2),
                    new Point(5, 4),
                    new Point(3, 2),
                }
            ));

            var expectedPoints = new Point[]
            {
                new Point(-2, 2),
                new Point(0, 0),
                new Point(2, 2),
                new Point(5, 0),
                new Point(7, 2),
                new Point(5, 4),
                new Point(3, 2),
                new Point(2, 2),
                new Point(0, 4),
            };

            var actual = sut.Union(polygonA, polygonB);
            
            CollectionAssert.AreEqual(expectedPoints, actual.Single().Paths.Single().Points);
        }
    }
}