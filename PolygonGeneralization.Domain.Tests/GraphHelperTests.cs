using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PolygonGeneralization.Domain.Models;
using PolygonGeneralization.Domain.SimpleClipper;

namespace PolygonGeneralization.Domain.Tests
{
    public class GraphHelperTests
    {
        private readonly GraphHelper _sut = new GraphHelper();

        [Test]
        public void Can_build_correct_ring_graph()
        {
            var path = new List<Point>
            {
                new Point(0, 0),
                new Point(4, 0),
                new Point(4, 4),
                new Point(0, 4)
            };
            
            var vertex3 = new Vertex(path[3]);
            var vertex2 = new Vertex(path[2]);
            var vertex1 = new Vertex(path[1]);
            var vertex0 = new Vertex(path[0]);
            
            vertex3.Neigbours.Add(vertex0);
            vertex2.Neigbours.Add(vertex3);
            vertex1.Neigbours.Add(vertex2);
            vertex0.Neigbours.Add(vertex1);

            var expectedGraph = new List<Vertex>
            {
                vertex0,
                vertex1,
                vertex2,
                vertex3
            };

            var actualGraph = _sut.BuildRing(path);
            
            CollectionAssert.AreEqual(expectedGraph, actualGraph, new GraphComparer());
        }

        [Test]
        public void Can_build_graph()
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
            
            var vertexA3 = new Vertex(pathA[3]);
            var vertexA2 = new Vertex(pathA[2]);
            var vertexA1 = new Vertex(pathA[1]);
            var vertexA0 = new Vertex(pathA[0]);
            
            vertexA3.Neigbours.Add(vertexA0);
            vertexA2.Neigbours.Add(vertexA3);
            vertexA1.Neigbours.Add(vertexA2);
            vertexA0.Neigbours.Add(vertexA1);
            
            var vertexB3 = new Vertex(pathB[3]);
            var vertexB2 = new Vertex(pathB[2]);
            var vertexB1 = new Vertex(pathB[1]);
            var vertexB0 = new Vertex(pathB[0]);
            
            vertexB3.Neigbours.Add(vertexB0);
            vertexB2.Neigbours.Add(vertexB3);
            vertexB1.Neigbours.Add(vertexB2);
            vertexB0.Neigbours.Add(vertexB1);
            
            // Bridge 1
            vertexA2.Neigbours.Add(vertexB1);
            vertexB1.Neigbours.Add(vertexA2);

            // Bridge 2
            vertexA3.Neigbours.Add(vertexB0);
            vertexB0.Neigbours.Add(vertexA3);
            
            var expectedGraph = new List<Vertex>
            {
                vertexA0,
                vertexA1,
                vertexA2,
                vertexA3,
                vertexB0,
                vertexB1,
                vertexB2,
                vertexB3,
            };

            var actualGraph = _sut.BuildGraph(pathA, pathB, 5).ToList();
            
            CollectionAssert.AreEqual(expectedGraph, actualGraph, new GraphComparer());
        }
        
        private static IEnumerable<object[]> GetBridgeTestCases => new[]
        {
            new object[]
            {
                new List<Point>
                    { new Point(0, 0), new Point(4, 0), new Point(4, 4), new Point(0, 4) },
                
                new List<Point>
                    { new Point(6, 0), new Point(10, 0), new Point(10, 4), new Point(6, 4) },
                
                new List<Point>
                    { new Point(4, 4), new Point(6, 4), new Point(4, 0), new Point(6, 0)  }
            },
            
            new object[]
            {
                new List<Point>
                { new Point(0, 0), new Point(2, 2), new Point(0, 4), new Point(-2, 2) },
                
                new List<Point>
                { new Point(5, 0), new Point(7, 2), new Point(5, 4), new Point(3, 2), },
                
                new List<Point>
                    { new Point(2, 2), new Point(3, 2), new Point(0, 0), new Point(5, 0) }
            },
            
            new object[]
            {
                new List<Point>
                    { new Point(0, 5), new Point(10, 5), new Point(10, 8), new Point(0, 8) },
                
                new List<Point>
                    { new Point(0, 0), new Point(11, 0), new Point(11, 3), new Point(0, 3), },
                
                new List<Point>
                    { new Point(0, 5), new Point(0, 3), new Point(10, 5), new Point(10, 3) }
            },
            
            new object[]
            {
                new List<Point>
                    { new Point(0, 0), new Point(4, 0), new Point(4, 4), new Point(0, 4)},
                
                new List<Point>
                    { new Point(5, 3), new Point(8, 3), new Point(8, 7), new Point(5, 7), },
                
                new List<Point>
                    { new Point(4, 4), new Point(5, 4), new Point(4, 3), new Point(5, 3) }
            }
        };
        
        [TestCaseSource(nameof(GetBridgeTestCases))]
        public void GetBridgeTest(List<Point> pathA, 
            List<Point> pathB,
            List<Point> expectedBridge)
        {
            var bridge = _sut.GetBridge(pathA.Select(it => new Vertex(it)).ToList(),
                pathB.Select(it => new Vertex(it)).ToList());
            
            Assert.NotNull(bridge);
            Assert.AreEqual(4, bridge.Length);
            Assert.AreEqual(expectedBridge[0], bridge[0]);
            Assert.AreEqual(expectedBridge[1], bridge[1]);
            Assert.AreEqual(expectedBridge[2], bridge[2]);
            Assert.AreEqual(expectedBridge[3], bridge[3]);
        }

        private class GraphComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                var vertexX = x as Vertex;
                var vertexY = y as Vertex;

                if (vertexX == null || vertexY == null)
                {
                    return 1;
                }

                if (!vertexX.Equals(vertexY))
                {
                    return 1;
                }

                if (vertexX.Neigbours.Count != vertexY.Neigbours.Count)
                {
                    return 1;
                }

                var vertexYneigbours = vertexY.Neigbours.ToList();
                if (vertexX.Neigbours.Where((p, i) => !p.Equals(vertexYneigbours[i])).Any())
                {
                    return 1;
                }

                return 0;
            }
        }
    }
}