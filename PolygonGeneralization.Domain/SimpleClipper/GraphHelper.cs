using System;
using System.Collections.Generic;
using System.Linq;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain.SimpleClipper
{
    public class GraphHelper
    {
        public HashSet<Edge> BuildGraph(
            List<Point> pathA,
            List<Point> pathB,
            Tuple<Point, Point> firstPair,
            Tuple<Point, Point> secondPair)
        {
            var graphA = BuildRing(pathA);
            var graphB = BuildRing(pathB);

            var graph = new HashSet<Edge>();
            var vertexA = new Vertex(firstPair.Item1);
            var vertexB = new Vertex(firstPair.Item2);
            var vertexC = new Vertex(secondPair.Item1);
            var vertexD = new Vertex(secondPair.Item2);

            var relatedForVertexA = graphA.Where(it => it.End.Equals(vertexA) || it.Start.Equals(vertexA)).ToList();
            var relatedForVertexB = graphB.Where(it => it.End.Equals(vertexB) || it.Start.Equals(vertexB)).ToList();
            var relatedForVertexC = graphA.Where(it => it.End.Equals(vertexC) || it.Start.Equals(vertexC)).ToList();
            var relatedForVertexD = graphB.Where(it => it.End.Equals(vertexD) || it.Start.Equals(vertexD)).ToList();

            vertexA.Edges.AddRange(relatedForVertexA);
            vertexB.Edges.AddRange(relatedForVertexB);
            vertexC.Edges.AddRange(relatedForVertexC);
            vertexD.Edges.AddRange(relatedForVertexD);

            var bridgeA = new Edge(vertexA, vertexB);
            var bridgeB = new Edge(vertexC, vertexD);

            foreach (var edge in relatedForVertexA)
            {
                if (edge.Start.Equals(bridgeA.Start))
                {
                    edge.Start.Edges.Add(bridgeA);
                }
                else if (edge.End.Equals(bridgeA.Start))
                {
                    edge.End.Edges.Add(bridgeA);
                }
            }

            foreach (var edge in relatedForVertexB)
            {
                if (edge.Start.Equals(bridgeA.End))
                {
                    edge.Start.Edges.Add(bridgeA);
                }
                else if (edge.End.Equals(bridgeA.End))
                {
                    edge.End.Edges.Add(bridgeA);
                }
            }

            foreach (var edge in relatedForVertexC)
            {
                if (edge.Start.Equals(bridgeB.Start))
                {
                    edge.Start.Edges.Add(bridgeB);
                }
                else if (edge.End.Equals(bridgeB.Start))
                {
                    edge.End.Edges.Add(bridgeB);
                }
            }

            foreach (var edge in relatedForVertexD)
            {
                if (edge.Start.Equals(bridgeB.End))
                {
                    edge.Start.Edges.Add(bridgeB);
                }
                else if (edge.End.Equals(bridgeB.End))
                {
                    edge.End.Edges.Add(bridgeB);
                }
            }

            graph.Add(bridgeA);
            graph.Add(bridgeB);

            foreach (var edge in graphA)
            {
                graph.Add(edge);
            }

            foreach (var edge in graphB)
            {
                graph.Add(edge);
            }

            return graph;
        }

        public List<Edge> BuildRing(List<Point> pathA)
        {
            var graph = new List<Edge>();

            for (var i = pathA.Count - 1; i > 0; i--)
            {
                var end = new Vertex(pathA[i]);
                var start = new Vertex(pathA[i - 1]);

                if (i != pathA.Count - 1)
                {
                    end.Edges.Add(graph.Last());
                }

                graph.Add(new Edge(start, end));
            }

            graph.First().End.Edges.Add(graph.Last());
            return graph;
        }
    }
}