

using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace PolygonGeneralization.Core.Tests
{
    [TestFixture]
    public class PointDTests
    {
        private List<PointD> _convexContour;
        private List<PointD> _notConvexContour;
        private List<List<PointD>> _polygonWithHoles;
        private List<PointD> _triangleContour;


        [SetUp]
        public void SetUp()
        {
            _convexContour = new List<PointD>
            {
                new PointD(3, 3),
                new PointD(-3, 3),
                new PointD(-3, -3),
                new PointD(3, -3),
            };

            _notConvexContour = new List<PointD>
            {
                new PointD(3, 3),
                new PointD(0, 0),
                new PointD(-3, 3),
                new PointD(-3, -3),
                new PointD(3, -3),
            };

            _polygonWithHoles = new List<List<PointD>>
            {
                new List<PointD>() // внешний контур
                {
                    new PointD(9, 9),
                    new PointD(0, 6),
                    new PointD(-9, 9),
                    new PointD(-9, -9),
                    new PointD(9, -9)
                },

                new List<PointD>() // первый внутренний контур
                {
                    new PointD(-8, 8),
                    new PointD(0, 3),
                    new PointD(-8, 3)
                },

                new List<PointD>() // второй внутренний контур
                {
                    new PointD(8, 8),
                    new PointD(8, 3),
                    new PointD(0, 3)
                },
            };

            _triangleContour = new List<PointD>
            {
                new PointD(8, 8),
                new PointD(8, 3),
                new PointD(0, 3)
            };
        }

        #region IsInsideTests
        [Test]
        public void IsInsideWhenPointIsInsideConvexContour()
        {
            var point = new PointD(0, 0);

            Assert.True(point.IsInside(_convexContour));
        }

        [Test]
        public void IsInsideWhenPointOutsideConvexContour()
        {
            var point = new PointD(5, 5);

            Assert.False(point.IsInside(_convexContour));
        }

        [Test]
        public void IsInsideWhenPointIsInsideNotConvexContour()
        {
            var point = new PointD(0, -1);

            Assert.True(point.IsInside(_notConvexContour));
        }

        [Test]
        public void IsInsideWhenPointIsInsideNotConvexContour2()
        {
            var point = new PointD(-2, 1);

            Assert.True(point.IsInside(_notConvexContour));
        }

        [Test]
        public void IsInsideWhenPointIsOutsideNotConvexContour()
        {
            var point = new PointD(5, 5);

            Assert.False(point.IsInside(_notConvexContour));
        }

        [Test]
        public void IsInsideWhenPointIsOutsideNotConvexContour2()
        {
            var point = new PointD(0, 1);

            Assert.False(point.IsInside(_notConvexContour));
        }

        [Test]
        public void IsInsideWhenPointOnBorder()
        {
            var point = new PointD(1, 1);

            Assert.True(point.IsInside(_notConvexContour));
        }

        [Test]
        public void IsInsideWhenPointOnBorder2()
        {
            var point = new PointD(-3, 0);

            Assert.True(point.IsInside(_notConvexContour));
        }

        [Test]
        public void IsInsideWhenPointIsNode()
        {
            var point = new PointD(0, 0);

            Assert.True(point.IsInside(_notConvexContour));
        }

        [Test]
        public void IsInsideWhenPointIsNode2()
        {
            var point = new PointD(3, 3);

            Assert.True(point.IsInside(_notConvexContour));
        }

        [Test]
        public void IsInsideWhenPointIsNode3()
        {
            var point = new PointD(3, 2.9);

            Assert.True(point.IsInside(_convexContour));
        }

        [Test]
        public void IsInsideTriangleContourWhenBordersAreNotInclude()
        {
            var point = new PointD(0, 3);
            Assert.False(point.IsInside(_triangleContour, false));
        }

        #endregion

        #region IsOnEdgeTests

        [Test]
        public void IsOnEdgeWhenItIsOnMiddleOfLineSegment()
        {
            var point = new PointD(0, 0);

            Assert.True(point.IsOnEdge(new PointD(-1, 0), new PointD(1, 0)));
        }

        [Test]
        public void IsOnEdgeWhenItIsNotOnEdge()
        {
            var point = new PointD(1, 1);

            Assert.False(point.IsOnEdge(new PointD(-1, 0), new PointD(1, 0)));
        }

        [Test]
        public void IsOnEdgeWhenItIsLeftPoint()
        {
            var point = new PointD(-1, 0);

            Assert.True(point.IsOnEdge(new PointD(-1, 0), new PointD(1, 0)));
        }

        [Test]
        public void IsOnEdgeWhenItIsRightPoint()
        {
            var point = new PointD(1, 0);

            Assert.True(point.IsOnEdge(new PointD(-1, 0), new PointD(1, 0)));
        }

        [Test]
        public void IsOnEdgeWhenItIsNotOnEdgeButOnLine()
        {
            var point = new PointD(5, 0);

            Assert.False(point.IsOnEdge(new PointD(-1, 0), new PointD(1, 0)));
        }

        [Test]
        public void IsOnEdgeWhenStartBiggerThenFinish()
        {
            var point = new PointD(0, 3);

            Assert.True(point.IsOnEdge(new PointD(8, 3), new PointD(0, 3)));
        }

        [Test]
        public void IsOnEdgeWhenStartAndFinishIsSamePoint()
        {
            var point = new PointD(0, 3);

            Assert.Throws<ArgumentException>(() => point.IsOnEdge(new PointD(0, 3), new PointD(0, 3)));
        }

        #endregion

        #region IsInsidePolygonWithHolesTests

        [TestCase(0.0, 0.0, Category = "IsInsidePolygonWithHolesTests", TestName = "Is Point (0, 0) inside of polygon with holes")]
        [TestCase(-8.5, 7.0, Category = "IsInsidePolygonWithHolesTests", TestName = "Is Point (-8.5, 7) inside of polygon with holes")]
        [TestCase(0.0, 5.0, Category = "IsInsidePolygonWithHolesTests", TestName = "Is Point (0, 5) inside of polygon with holes")]
        [TestCase(0.0, 3.0, Category = "IsInsidePolygonWithHolesTests", TestName = "Is Point (0, 3) inside of polygon with holes")]
        [TestCase(-8.0, 8.0, Category = "IsInsidePolygonWithHolesTests", TestName = "Is Point (-8, 8) inside of polygon with holes")]
        [TestCase(8.0, 3.0, Category = "IsInsidePolygonWithHolesTests", TestName = "Is Point (8, 3) inside of polygon with holes")]
        [TestCase(9.0, 9.0, Category = "IsInsidePolygonWithHolesTests", TestName = "Is Point (9, 9) inside of polygon with holes")]
        [TestCase(3.0, 3.0, Category = "IsInsidePolygonWithHolesTests", TestName = "Is Point (3, 3) inside of polygon with holes")]
        public void IsInsidePolygonWithHolesSuccessTests(double x, double y)
        {
            var point = new PointD(x, y);
            Assert.True(point.IsInside(_polygonWithHoles));
        }

        [TestCase(5.0, 5.0, Category = "IsInsidePolygonWithHolesTests", TestName = "Is Point (5, 5) inside of polygon with holes")]
        [TestCase(-5.0, 5.0, Category = "IsInsidePolygonWithHolesTests", TestName = "Is Point (-5, 5) inside of polygon with holes")]
        [TestCase(0.0, 8.0, Category = "IsInsidePolygonWithHolesTests", TestName = "Is Point (0, 8) inside of polygon with holes")]
        public void IsInsidePolygonWithHolesFailureTests(double x, double y)
        {
            var point = new PointD(x, y);
            Assert.False(point.IsInside(_polygonWithHoles));
        }

        #endregion
    }
}
