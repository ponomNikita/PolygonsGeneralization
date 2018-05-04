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
        private const double MinDistance = 5;
        
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

            var union = sut.UnionPaths(pathA, pathB, MinDistance);
            
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

            var actual = sut.Union(polygonA, polygonB, MinDistance);
            
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

            var actual = sut.Union(polygonA, polygonB, MinDistance);
            
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

            var actual = sut.Union(polygonA, polygonB, MinDistance);
            
            CollectionAssert.AreEqual(expectedPoints, actual.Single().Paths.Single().Points);
        }
        
        [Test]
        public void UnionTest4()
        {
            var sut = new SimpleClipper.SimpleClipper();
            
            var polygonA = new Polygon(new Path(
                new Point[]
                {
                    new Point(0, 5),
                    new Point(5, 5),
                    new Point(5, 10),
                    new Point(0, 10)
                }
            ));
            
            var polygonB = new Polygon(new Path(
                new Point[]
                {
                    new Point(1, 0),
                    new Point(4, 0),
                    new Point(4, 4),
                    new Point(1, 4),
                }
            ));

            var expectedPoints = new Point[]
            {
                new Point(0, 5),
                new Point(1, 5),
                new Point(1, 4),
                new Point(1, 0),
                new Point(4, 0),
                new Point(4, 4),
                new Point(4, 5),
                new Point(5, 5),
                new Point(5, 10),
                new Point(0, 10),
            };

            var actual = sut.Union(polygonA, polygonB, MinDistance);
            
            CollectionAssert.AreEqual(expectedPoints, actual.Single().Paths.Single().Points);
        }
        
        [Test]
        public void UnionTest5()
        {
            var sut = new SimpleClipper.SimpleClipper();
            
            var polygonA = new Polygon(new Path(
                new Point[]
                {
                    new Point(0, 5),
                    new Point(5, 5),
                    new Point(5, 10),
                    new Point(0, 10)
                }
            ));
            
            var polygonB = new Polygon(new Path(
                new Point[]
                {
                    new Point(-1, 0),
                    new Point(6, 0),
                    new Point(6, 4),
                    new Point(-1, 4),
                }
            ));

            var expectedPoints = new Point[]
            {
                new Point(-1, 0),
                new Point(6, 0),
                new Point(6, 4),
                new Point(5, 4),
                new Point(5, 5),
                new Point(5, 10),
                new Point(0, 10),
                new Point(0, 5),
                new Point(0, 4),
                new Point(-1, 4),
            };

            var actual = sut.Union(polygonA, polygonB, MinDistance);
            
            CollectionAssert.AreEqual(expectedPoints, actual.Single().Paths.Single().Points);
        }
        
        [Test]
        public void UnionTest6()
        {
            var sut = new SimpleClipper.SimpleClipper();
            
            var polygonA = new Polygon(new Path(
                new Point[]
                {
                    new Point(3, 187),
                    new Point(212, 187),
                    new Point(209, 327),
                    new Point(0, 321),
                }
            ));
            
            var polygonB = new Polygon(new Path(
                new Point[]
                {
                    new Point(4, 0),
                    new Point(213, 1),
                    new Point(214, 171),
                    new Point(2, 176),
                }
            ));

            var expectedPoints = new Point[]
            {
            };

            var actual = sut.Union(polygonA, polygonB, 200);
            
            Assert.AreEqual(1, actual.Count);
            
            //CollectionAssert.AreEqual(expectedPoints, actual.Single().Paths.Single().Points);
        }


        [Test]
        public void UnionTest7()
        {
            var sut = new SimpleClipper.SimpleClipper();

            var polygonA = new Polygon(new Path(
                new Point[]
                {
                    new Point(2, 5),
                    new Point(8, 4),
                    new Point(11, 8),
                    new Point(1, 9),
                }
            ));

            var polygonB = new Polygon(new Path(
                new Point[]
                {
                    new Point(0, 0),
                    new Point(7, 1),
                    new Point(10, 2),
                    new Point(2, 3),
                }
            ));

            var actual = sut.Union(polygonA, polygonB, 200);

            Assert.AreEqual(1, actual.Count);
        }

        [Test]
        public void UnionTest8()
        {
            var sut = new SimpleClipper.SimpleClipper();
            
            var polygonA = new Polygon(new Path(
                new Point[]
                {
                    new Point(0, 337),
                    new Point(206, 340),
                    new Point(210, 460),
                    new Point(-4, 471),
                }
            ));
            
            var polygonB = new Polygon(new Path(
                new Point[]
                {
                    new Point(3, 187),
                    new Point(212, 187),
                    new Point(209, 327),
                    new Point(0, 321),
                }
            ));
            
            var polygonC = new Polygon(new Path(
                new Point[]
                {
                    new Point(4, 0),
                    new Point(213, 1),
                    new Point(214, 171),
                    new Point(2, 176),
                }
            ));
            
            /*var polygonA = new Polygon(new Path(
                new Point[]
                {
                    new Point(1016, 443),
                    new Point(1222, 446),
                    new Point(1226, 566),
                    new Point(1012, 577),
                }
            ));
            
            var polygonB = new Polygon(new Path(
                new Point[]
                {
                    new Point(1019, 293),
                    new Point(1228, 293),
                    new Point(1225, 433),
                    new Point(1016, 427),
                }
            ));
            
            var polygonC = new Polygon(new Path(
                new Point[]
                {
                    new Point(1020, 106),
                    new Point(1229, 107),
                    new Point(1230, 277),
                    new Point(1018, 282),
                }
            ));

            var expectedPoints = new Point[]
            {
            };*/

            var union1 = sut.Union(polygonA, polygonB, 200);
            
            Assert.AreEqual(1, union1.Count);
            
            var union2 = sut.Union(union1[0], polygonC, 400);
            
            Assert.AreEqual(1, union2.Count);
            
            //CollectionAssert.AreEqual(expectedPoints, actual.Single().Paths.Single().Points);
        }
    }
}