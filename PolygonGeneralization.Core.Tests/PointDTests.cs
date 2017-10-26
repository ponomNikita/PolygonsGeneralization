

using System.Collections.Generic;
using NUnit.Framework;

namespace PolygonGeneralization.Core.Tests
{
    [TestFixture]
    public class PointDTests
    {
        private List<PointD> _convexContour;
        private List<PointD> _notConvexContour;

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

        #endregion
    }
}
