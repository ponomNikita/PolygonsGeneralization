
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NUnit.Framework;

namespace PolygonGeneralization.Core.Tests
{
    [TestFixture]
    public class ClipperTests
    {
        private Clipper _clipper;

        private List<List<PointD>> _subject;
        private List<List<PointD>> _clipping;

        private MethodInfo _insertEdgeMethod;
        private MethodInfo _buildModelMethod;
        private MethodInfo _markEdgesMethod;
        private FieldInfo _edgeSet;
        private FieldInfo _pointsSet;
        private HashSet<Edge> _expectedEdgesAfterBuildModel;
        
        [SetUp]
        public void SetUp()
        {
            _subject = new List<List<PointD>>
            {
                new List<PointD>()
                {
                    new PointD(6, 6),
                    new PointD(0, 6),
                    new PointD(0, 0),
                    new PointD(6, 0),
                },

                new List<PointD>()
                {
                    new PointD(4, 4),
                    new PointD(4, 2),
                    new PointD(2, 2),
                    new PointD(2, 4),
                }
            };

            _clipping = new List<List<PointD>>
            {
                new List<PointD>()
                {
                    new PointD(9, 9),
                    new PointD(3, 9),
                    new PointD(3, 3),
                    new PointD(9, 3),
                }
            };

            _insertEdgeMethod = typeof(Clipper).GetMethod("InsertEdge", BindingFlags.Instance | BindingFlags.NonPublic);
            _buildModelMethod = typeof(Clipper).GetMethod("BuildModel", BindingFlags.Instance | BindingFlags.NonPublic);
            _markEdgesMethod = typeof(Clipper).GetMethod("MarkEdges", BindingFlags.Instance | BindingFlags.NonPublic);

            _edgeSet = typeof(Clipper).GetField("_edgesSet", BindingFlags.Instance | BindingFlags.NonPublic);
            _pointsSet = typeof(Clipper).GetField("_pointsSet", BindingFlags.Instance | BindingFlags.NonPublic);

            _clipper = new Clipper(_subject, _clipping);

            _expectedEdgesAfterBuildModel = new HashSet<Edge>
            {
                new Edge(new PointD(6, 6), new PointD(3, 6), true, false),
                new Edge(new PointD(3, 6), new PointD(0, 6), true, false),
                new Edge(new PointD(0, 6), new PointD(0, 0), true, false),
                new Edge(new PointD(0, 0), new PointD(6, 0), true, false),
                new Edge(new PointD(6, 0), new PointD(6, 3), true, false),
                new Edge(new PointD(6, 3), new PointD(6, 6), true, false),

                new Edge(new PointD(4, 4), new PointD(4, 3), true, false),
                new Edge(new PointD(4, 3), new PointD(4, 2), true, false),
                new Edge(new PointD(4, 2), new PointD(2, 2), true, false),
                new Edge(new PointD(2, 2), new PointD(2, 4), true, false),
                new Edge(new PointD(2, 4), new PointD(3, 4), true, false),
                new Edge(new PointD(3, 4), new PointD(4, 4), true, false),


                new Edge(new PointD(9, 9), new PointD(3, 9), false, true),
                new Edge(new PointD(3, 9), new PointD(3, 6), false, true),
                new Edge(new PointD(3, 6), new PointD(3, 4), false, true),
                new Edge(new PointD(3, 4), new PointD(3, 3), false, true),
                new Edge(new PointD(3, 3), new PointD(4, 3), false, true),
                new Edge(new PointD(4, 3), new PointD(6, 3), false, true),
                new Edge(new PointD(6, 3), new PointD(9, 3), false, true),
                new Edge(new PointD(9, 3), new PointD(9, 9), false, true),

            };
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
            _pointsSet.SetValue(_clipper, new HashSet<PointD>());

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
            _pointsSet.SetValue(_clipper, new HashSet<PointD>());

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
            _pointsSet.SetValue(_clipper, new HashSet<PointD>());

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
            _pointsSet.SetValue(_clipper, new HashSet<PointD>());

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
            _pointsSet.SetValue(_clipper, new HashSet<PointD>());

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
            _pointsSet.SetValue(_clipper, new HashSet<PointD>());

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
            _pointsSet.SetValue(_clipper, new HashSet<PointD>());

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

        #region BuildModelTests

        [Test]
        public void BuildModelTest()
        {
            var expectedEdgesList = _expectedEdgesAfterBuildModel.ToList();

            _buildModelMethod.Invoke(_clipper, new object[0]);

            var edgesAfterBuild = (HashSet<Edge>)_edgeSet.GetValue(_clipper);
            var pointsAfterBuild = (HashSet<PointD>)_pointsSet.GetValue(_clipper);


            Assert.AreEqual(16, pointsAfterBuild.Count);
            Assert.AreEqual(20, edgesAfterBuild.Count);
            Assert.AreEqual(expectedEdgesList.Count, edgesAfterBuild.Count);

            foreach (var expectedEdge in expectedEdgesList)
            {
                var edge = edgesAfterBuild.FirstOrDefault(e => e.Equals(expectedEdge));

                Assert.NotNull(edge);
                Assert.AreEqual(expectedEdge.IsFromSubject, edge.IsFromSubject, $"edge[({edge.A.X},{edge.A.Y}); ({edge.B.X},{edge.B.Y})]");
                Assert.AreEqual(expectedEdge.IsFromClipping, edge.IsFromClipping, $"edge[({edge.A.X},{edge.A.Y}); ({edge.B.X},{edge.B.Y})]");
            }
        }

        [Test]
        public void MarkEdgesTest()
        {
            var expectedEdgesList = new List<Edge>
            {
                new Edge(new PointD(3, 6), new PointD(0, 6), true, false) { IsResult = true },
                new Edge(new PointD(0, 6), new PointD(0, 0), true, false) { IsResult = true },
                new Edge(new PointD(0, 0), new PointD(6, 0), true, false) { IsResult = true },
                new Edge(new PointD(6, 0), new PointD(6, 3), true, false) { IsResult = true },
                
                new Edge(new PointD(4, 3), new PointD(4, 2), true, false) { IsResult = true },
                new Edge(new PointD(4, 2), new PointD(2, 2), true, false) { IsResult = true },
                new Edge(new PointD(2, 2), new PointD(2, 4), true, false) { IsResult = true },
                new Edge(new PointD(2, 4), new PointD(3, 4), true, false) { IsResult = true },


                new Edge(new PointD(9, 9), new PointD(3, 9), false, true) { IsResult = true },
                new Edge(new PointD(3, 9), new PointD(3, 6), false, true) { IsResult = true },
                new Edge(new PointD(3, 4), new PointD(3, 3), false, true) { IsResult = true },
                new Edge(new PointD(3, 3), new PointD(4, 3), false, true) { IsResult = true },
                new Edge(new PointD(6, 3), new PointD(9, 3), false, true) { IsResult = true },
                new Edge(new PointD(9, 3), new PointD(9, 9), false, true) { IsResult = true },

            };

            _edgeSet.SetValue(_clipper, _expectedEdgesAfterBuildModel);

            _markEdgesMethod.Invoke(_clipper, new object[0]);

            var edgesAfterMarking = (HashSet<Edge>)_edgeSet.GetValue(_clipper);

            Assert.AreEqual(14, edgesAfterMarking.Count);
            Assert.AreEqual(expectedEdgesList.Count, edgesAfterMarking.Count);

            foreach (var expectedEdge in expectedEdgesList)
            {
                var edge = edgesAfterMarking.FirstOrDefault(e => e.Equals(expectedEdge));

                Assert.NotNull(edge);
                Assert.AreEqual(expectedEdge.IsFromSubject, edge.IsFromSubject, $"edge[({edge.A.X},{edge.A.Y}); ({edge.B.X},{edge.B.Y})]");
                Assert.AreEqual(expectedEdge.IsFromClipping, edge.IsFromClipping, $"edge[({edge.A.X},{edge.A.Y}); ({edge.B.X},{edge.B.Y})]");
                Assert.AreEqual(expectedEdge.IsResult, edge.IsResult, $"edge[({edge.A.X},{edge.A.Y}); ({edge.B.X},{edge.B.Y})]");
            }

            Assert.True(expectedEdgesList.All(e => e.IsResult));
        }

        #endregion
    }
}
