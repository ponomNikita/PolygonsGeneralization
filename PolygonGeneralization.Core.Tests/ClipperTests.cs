using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace PolygonGeneralization.Core.Tests
{
    [TestFixture]
    public class ClipperTests
    {
        private MyClipper _clipper;

        private List<List<PointD>> _subject = new List<List<PointD>>();
        private List<List<PointD>> _clipping = new List<List<PointD>>();

        private MethodInfo _insertEdgeMethod;
        private FieldInfo _edgeSet;

        [SetUp]
        public void SetUp()
        {
            _insertEdgeMethod = typeof(MyClipper).GetMethod("InsertEdge", BindingFlags.Instance | BindingFlags.NonPublic);
            _edgeSet = typeof(MyClipper).GetField("_edgesSet", BindingFlags.Instance | BindingFlags.NonPublic);

            _clipper = new MyClipper(_subject, _clipping);
        }

        #region InsertEdgeTests

        [Test]
        public void InsertEdgeWhenWasOneEdgeAndOneIntersection()
        {
            // arrange
            var existingEdges = new HashSet<Edge>
            {
                new Edge(new PointD(0, 0), new PointD(5, 0))
            };

            _edgeSet.SetValue(_clipper, existingEdges);

            var newEdge = new Edge(new PointD(2, -2), new PointD(2, 2));

            // act 
            _insertEdgeMethod.Invoke(_clipper, new object[]{ newEdge});

            // assert
            var edgesAfterInsert = (HashSet<Edge>) _edgeSet.GetValue(_clipper);

            Assert.True(edgesAfterInsert.Contains(new Edge(new PointD(2, -2), new PointD(2, 0))));
            Assert.True(edgesAfterInsert.Contains(new Edge(new PointD(2, 0), new PointD(2, 2))));
            Assert.True(edgesAfterInsert.Contains(new Edge(new PointD(0, 0), new PointD(2, 0))));
            Assert.True(edgesAfterInsert.Contains(new Edge(new PointD(2, 0), new PointD(5, 0))));
            Assert.AreEqual(4, edgesAfterInsert.Count);
        }

        [Test]
        public void InsertEdgeWhenWasTwoEdgesAndTwoIntersections()
        {
            // arrange
            var existingEdges = new HashSet<Edge>
            {
                new Edge(new PointD(0, 0), new PointD(5, 0)),
                new Edge(new PointD(2, 5), new PointD(6, 1)),
            };

            _edgeSet.SetValue(_clipper, existingEdges);

            var newEdge = new Edge(new PointD(4, -2), new PointD(4, 5));

            // act 
            _insertEdgeMethod.Invoke(_clipper, new object[] { newEdge });

            // assert
            var edgesAfterInsert = (HashSet<Edge>)_edgeSet.GetValue(_clipper);
            
            Assert.True(edgesAfterInsert.Contains(new Edge(new PointD(0, 0), new PointD(4, 0))));
            Assert.True(edgesAfterInsert.Contains(new Edge(new PointD(4, 0), new PointD(5, 0))));
            Assert.True(edgesAfterInsert.Contains(new Edge(new PointD(2, 5), new PointD(4, 3))));
            Assert.True(edgesAfterInsert.Contains(new Edge(new PointD(4, 3), new PointD(6, 1))));

            Assert.True(edgesAfterInsert.Contains(new Edge(new PointD(4, -2), new PointD(4, 0))));
            Assert.True(edgesAfterInsert.Contains(new Edge(new PointD(4, 0), new PointD(4, 3))));
            Assert.True(edgesAfterInsert.Contains(new Edge(new PointD(4, 3), new PointD(4, 5))));

            Assert.AreEqual(7, edgesAfterInsert.Count);
        }

        [Test]
        public void InsertEdgeWhenWasOneEdgeAndOneIntersectionInStartOfExisting()
        {
            // arrange
            var existingEdges = new HashSet<Edge>
            {
                new Edge(new PointD(0, 0), new PointD(5, 0))
            };

            _edgeSet.SetValue(_clipper, existingEdges);

            var newEdge = new Edge(new PointD(0, -2), new PointD(0, 2));

            // act 
            _insertEdgeMethod.Invoke(_clipper, new object[] { newEdge });

            // assert
            var edgesAfterInsert = (HashSet<Edge>)_edgeSet.GetValue(_clipper);

            Assert.True(edgesAfterInsert.Contains(new Edge(new PointD(0, -2), new PointD(0, 0))));
            Assert.True(edgesAfterInsert.Contains(new Edge(new PointD(0, 0), new PointD(0, 2))));
            Assert.True(edgesAfterInsert.Contains(new Edge(new PointD(0, 0), new PointD(5, 0))));
            Assert.AreEqual(3, edgesAfterInsert.Count);
        }

        [Test]
        public void InsertEdgeWhenWasOneEdgeAndOneIntersectionInFinishOfExisting()
        {
            // arrange
            var existingEdges = new HashSet<Edge>
            {
                new Edge(new PointD(0, 0), new PointD(5, 0))
            };

            _edgeSet.SetValue(_clipper, existingEdges);

            var newEdge = new Edge(new PointD(5, -2), new PointD(5, 2));

            // act 
            _insertEdgeMethod.Invoke(_clipper, new object[] { newEdge });

            // assert
            var edgesAfterInsert = (HashSet<Edge>)_edgeSet.GetValue(_clipper);

            Assert.True(edgesAfterInsert.Contains(new Edge(new PointD(5, -2), new PointD(5, 0))));
            Assert.True(edgesAfterInsert.Contains(new Edge(new PointD(5, 0), new PointD(5, 2))));
            Assert.True(edgesAfterInsert.Contains(new Edge(new PointD(0, 0), new PointD(5, 0))));
            Assert.AreEqual(3, edgesAfterInsert.Count);
        }

        [Test]
        public void InsertEdgeWhenWasOneEdgeAndOneIntersectionInMiddleOfExisting()
        {
            // arrange
            var existingEdges = new HashSet<Edge>
            {
                new Edge(new PointD(0, 0), new PointD(5, 0))
            };

            _edgeSet.SetValue(_clipper, existingEdges);

            var newEdge = new Edge(new PointD(3, -2), new PointD(3, 0));

            // act 
            _insertEdgeMethod.Invoke(_clipper, new object[] { newEdge });

            // assert
            var edgesAfterInsert = (HashSet<Edge>)_edgeSet.GetValue(_clipper);

            Assert.True(edgesAfterInsert.Contains(new Edge(new PointD(3, -2), new PointD(3, 0))));
            Assert.True(edgesAfterInsert.Contains(new Edge(new PointD(0, 0), new PointD(3, 0))));
            Assert.True(edgesAfterInsert.Contains(new Edge(new PointD(3, 0), new PointD(5, 0))));
            Assert.AreEqual(3, edgesAfterInsert.Count);
        }

        [Test]
        public void InsertEdgeWhenWasOneEdgeAndNoIntersections()
        {
            // arrange
            var existingEdges = new HashSet<Edge>
            {
                new Edge(new PointD(0, 0), new PointD(5, 0))
            };

            _edgeSet.SetValue(_clipper, existingEdges);

            var newEdge = new Edge(new PointD(0, 1), new PointD(5, 1));

            // act 
            _insertEdgeMethod.Invoke(_clipper, new object[] { newEdge });

            // assert
            var edgesAfterInsert = (HashSet<Edge>)_edgeSet.GetValue(_clipper);

            Assert.True(edgesAfterInsert.Contains(new Edge(new PointD(0, 0), new PointD(5, 0))));
            Assert.True(edgesAfterInsert.Contains(newEdge));
            Assert.AreEqual(2, edgesAfterInsert.Count);
        }

        [Test]
        public void InsertEdgeWhenTryInsertExistingEdge()
        {
            // arrange
            var existingEdges = new HashSet<Edge>
            {
                new Edge(new PointD(0, 0), new PointD(5, 0)),
                new Edge(new PointD(0, 1), new PointD(5, 1))
            };

            _edgeSet.SetValue(_clipper, existingEdges);

            var newEdge = new Edge(new PointD(0, 1), new PointD(5, 1));

            // act 
            _insertEdgeMethod.Invoke(_clipper, new object[] { newEdge });

            // assert
            var edgesAfterInsert = (HashSet<Edge>)_edgeSet.GetValue(_clipper);

            Assert.True(edgesAfterInsert.Contains(new Edge(new PointD(0, 0), new PointD(5, 0))));
            Assert.True(edgesAfterInsert.Contains(new Edge(new PointD(0, 1), new PointD(5, 1))));
            Assert.AreEqual(2, edgesAfterInsert.Count);
        }

        #endregion
    }
}
