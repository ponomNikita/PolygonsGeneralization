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
            
            var verex3 = new Vertex(path[3]);
            var verex2 = new Vertex(path[2]);
            var verex1 = new Vertex(path[1]);
            var verex0 = new Vertex(path[0]);
            
            var edge0 = new Edge(verex2, verex3);
            var edge1 = new Edge(verex1, verex2);
            var edge2 = new Edge(verex0, verex1);
            var edge3 = new Edge(verex3, verex0);
            
            verex3.Edges.Add(edge2);
            verex2.Edges.Add(edge0);
            verex1.Edges.Add(edge1);
            verex0.Edges.Add(edge2);

            var expectedGraph = new List<Edge>
            {
                edge0,
                edge1,
                edge2,
                edge3
            };

            var actualGraph = _sut.BuildRing(path);
            
            CollectionAssert.AreEqual(expectedGraph, actualGraph, new GraphComparer());
        }

        private class GraphComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                var edgeX = x as Edge;
                var edgeY = y as Edge;

                if (edgeX == null || edgeY == null)
                {
                    return 1;
                }

                if (!edgeX.End.Equals(edgeY.End) || !edgeX.Start.Equals(edgeY.Start))
                {
                    return 1;
                }

                if (edgeX.End.Edges.Count != edgeY.End.Edges.Count ||
                    edgeX.Start.Edges.Count != edgeY.Start.Edges.Count)
                {
                    return 1;
                }
                
                if (edgeX.Start.Edges.Where((t, i) => t.GetHashCode() != edgeY.Start.Edges[i].GetHashCode()).Any() ||
                    edgeX.End.Edges.Where((t, i) => t.GetHashCode() != edgeY.End.Edges[i].GetHashCode()).Any() )
                {
                    return 1;
                }

                return 0;
            }
        }
    }
}