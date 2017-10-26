

using NUnit.Framework;

namespace PolygonGeneralization.Core.Tests
{
    [TestFixture]
    public class EdgesTests
    {

        #region GetIntersectionTests

        [Test]
        public void GetIntersectionPointWhenEdgesAreParallel()
        {
            Edge first = new Edge(new PointD(-1, 0), new PointD(1, 0));
            Edge second = new Edge(new PointD(-1, -1), new PointD(1, -1));

            var intersection = first.GetIntersection(second);

            Assert.AreEqual(PointD.EmptyPoint, intersection);
        }

        [Test]
        public void GetIntersectionPointWhenEdgesHasNoIntersections()
        {
            Edge first = new Edge(new PointD(-1, 0), new PointD(1, 0));
            Edge second = new Edge(new PointD(0, -2), new PointD(0, -0.01));

            var intersection = first.GetIntersection(second);

            Assert.AreEqual(PointD.EmptyPoint, intersection);
        }

        [Test]
        public void GetIntersectionPointWhenEdgesHasIntersections()
        {
            Edge first = new Edge(new PointD(-1, 0), new PointD(1, 0));
            Edge second = new Edge(new PointD(0, -2), new PointD(0, 2));

            var intersection = first.GetIntersection(second);

            Assert.AreEqual(new PointD(0, 0), intersection);
        }

        [Test]
        public void GetIntersectionPointBetweenStartsOfEdges()
        {
            Edge first = new Edge(new PointD(-1, 0), new PointD(1, 0));
            Edge second = new Edge(new PointD(-1, 0), new PointD(-1, 2));

            var intersection = first.GetIntersection(second);

            Assert.AreEqual(new PointD(-1, 0), intersection);
        }

        [Test]
        public void GetIntersectionPointBetweenEndsOfEdges()
        {
            Edge first = new Edge(new PointD(-1, 0), new PointD(1, 0));
            Edge second = new Edge(new PointD(2, 2), new PointD(1, 0));

            var intersection = first.GetIntersection(second);

            Assert.AreEqual(new PointD(1, 0), intersection);
        }

        [Test]
        public void GetIntersectionPointBetweenEndAndStartOfEdges()
        {
            Edge first = new Edge(new PointD(-1, 0), new PointD(1, 0));
            Edge second = new Edge(new PointD(1, 0), new PointD(1, 2));

            var intersection = first.GetIntersection(second);

            Assert.AreEqual(new PointD(1, 0), intersection);
        }

        [Test]
        public void GetIntersectionPointBetweenStartAndEndOfEdges()
        {
            Edge first = new Edge(new PointD(-1, 0), new PointD(1, 0));
            Edge second = new Edge(new PointD(2, 2), new PointD(-1, 0));

            var intersection = first.GetIntersection(second);

            Assert.AreEqual(new PointD(-1, 0), intersection);
        }

        [Test]
        public void GetIntersectionPointWhenEdgesHasIntersectionsOnMiddleOfEdge()
        {
            Edge first = new Edge(new PointD(-1, 0), new PointD(1, 0));
            Edge second = new Edge(new PointD(0, 0), new PointD(0, 2));

            var intersection = first.GetIntersection(second);

            Assert.AreEqual(new PointD(0, 0), intersection);
        }


        #endregion

        #region GetSubdivisionTests

        [Test]
        public void GetSubdivisionPointWhenEdgesAreParallel()
        {
            Edge first = new Edge(new PointD(-1, 0), new PointD(1, 0));
            Edge second = new Edge(new PointD(-1, -1), new PointD(1, -1));

            var subdivision = first.GetSubdivision(second);

            Assert.AreEqual(first, subdivision.Item1);
            Assert.IsNull(subdivision.Item2);
        }

        [Test]
        public void GetSubdivisionPointWhenEdgesHasNoIntersections()
        {
            Edge first = new Edge(new PointD(-1, 0), new PointD(1, 0));
            Edge second = new Edge(new PointD(0, -2), new PointD(0, -0.01));

            var subdivision = first.GetSubdivision(second);

            Assert.AreEqual(first, subdivision.Item1);
            Assert.IsNull(subdivision.Item2);
        }

        [Test]
        public void GetSubdivisionPointWhenEdgesHasIntersections()
        {
            Edge first = new Edge(new PointD(-1, 0), new PointD(1, 0));
            Edge second = new Edge(new PointD(0, -2), new PointD(0, 2));

            var subdivision = first.GetSubdivision(second);

            Assert.AreEqual(new Edge(new PointD(-1, 0), new PointD(0, 0)), subdivision.Item1);
            Assert.AreEqual(new Edge(new PointD(0, 0), new PointD(1, 0)), subdivision.Item2);
        }

        [Test]
        public void GetSubdivisionPointBetweenStartsOfEdges()
        {
            Edge first = new Edge(new PointD(-1, 0), new PointD(1, 0));
            Edge second = new Edge(new PointD(-1, 0), new PointD(-1, 2));

            var subdivision = first.GetSubdivision(second);

            Assert.AreEqual(first, subdivision.Item1);
            Assert.IsNull(subdivision.Item2);
        }

        [Test]
        public void GetSubdivisionPointBetweenEndsOfEdges()
        {
            Edge first = new Edge(new PointD(-1, 0), new PointD(1, 0));
            Edge second = new Edge(new PointD(2, 2), new PointD(1, 0));

            var subdivision = first.GetSubdivision(second);

            Assert.AreEqual(first, subdivision.Item1);
            Assert.IsNull(subdivision.Item2);
        }

        [Test]
        public void GetSubdivisionPointBetweenEndAndStartOfEdges()
        {
            Edge first = new Edge(new PointD(-1, 0), new PointD(1, 0));
            Edge second = new Edge(new PointD(1, 0), new PointD(1, 2));

            var subdivision = first.GetSubdivision(second);

            Assert.AreEqual(first, subdivision.Item1);
            Assert.IsNull(subdivision.Item2);
        }

        [Test]
        public void GetSubdivisionPointBetweenStartAndEndOfEdges()
        {
            Edge first = new Edge(new PointD(-1, 0), new PointD(1, 0));
            Edge second = new Edge(new PointD(2, 2), new PointD(-1, 0));

            var subdivision = first.GetSubdivision(second);

            Assert.AreEqual(first, subdivision.Item1);
            Assert.IsNull(subdivision.Item2);
        }

        [Test]
        public void GetSubdivisionPointWhenEdgesHasIntersectionsOnMiddleOfEdge()
        {
            Edge first = new Edge(new PointD(-1, 0), new PointD(1, 0));
            Edge second = new Edge(new PointD(0, 0), new PointD(0, 2));

            var subdivision = first.GetSubdivision(second);

            Assert.AreEqual(new Edge(new PointD(-1, 0), new PointD(0, 0)), subdivision.Item1);
            Assert.AreEqual(new Edge(new PointD(0, 0), new PointD(1, 0)), subdivision.Item2);
        }

        #endregion
    }
}
