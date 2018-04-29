using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using PolygonGeneralization.Domain.Exceptions;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain.SimpleClipper
{
    public class GraphHelper
    {
        private readonly VectorGeometry _vectorGeometry = new VectorGeometry();
         
        public HashSet<Vertex> BuildGraph(
            List<Point> pathA,
            List<Point> pathB)
        {
            var graphA = BuildRing(pathA);
            var graphB = BuildRing(pathB);

            var bridge = GetBridge(graphA, graphB);
            
            var resultGraph = new HashSet<Vertex>();
            
            foreach (var item in graphA)
            {
                resultGraph.Add(item);
            }
            
            foreach (var item in graphB)
            {
                resultGraph.Add(item);
            }
            
            foreach (var vertex in bridge)
            {
                resultGraph.Add(vertex);
            }

            return resultGraph;
        }

        public List<Vertex> BuildRing(List<Point> pathA)
        {
            var graph = new List<Vertex>();

            Vertex current = null;
            Vertex next = null;
            
            for (var i = 0; i < pathA.Count; i++)
            {
                current = i == 0 ? new Vertex(pathA[i]) : next;
                next = i != pathA.Count - 1 ? new Vertex(pathA[i + 1]) : graph[0];
                
                current.Neigbours.Add(next);
                
                graph.Add(current);
            }

            return graph;
        }

        public Vertex[] GetBridge(List<Vertex> pathA, List<Vertex> pathB)
        {
            var firstBridge = GetBridge(pathA, pathB, null, null);
            var secondBridge = GetBridge(pathA, pathB, firstBridge[0], firstBridge[1]);
            return new [] {firstBridge[0], firstBridge[1], secondBridge[0], secondBridge[1]};
        }

        private Vertex[] GetBridge(List<Vertex> pathA, List<Vertex> pathB, Vertex besidesA, Vertex besidesB)
        {
            var bridge = new Vertex[2];

            var minDistance = Double.MaxValue;
            Vertex minVertexA1 = null;
            Vertex minVertexB1 = null;
            Vertex minVertexA2 = null;
            Vertex minVertexB2 = null;

            for (var i = 0; i < pathA.Count; i++)
            {
                var startIndexI = i;
                var endIndexI = i == pathA.Count - 1 ? 0 : i + 1;

                for (var j = 0; j < pathB.Count; j++)
                {
                    var startIndexJ = j;
                    var endIndexJ = j == pathB.Count - 1 ? 0 : j + 1;

                    var minimalDistance = _vectorGeometry.GetMinDistance(pathA[startIndexI],
                        pathA[endIndexI],
                        pathB[startIndexJ],
                        pathB[endIndexJ]);

                    if (minimalDistance.Item3 <= minDistance && 
                        besidesA != minimalDistance.Item1 && 
                        besidesB != minimalDistance.Item1 &&
                        besidesA != minimalDistance.Item2 && 
                        besidesB != minimalDistance.Item2)
                    {
                        if (bridge[0] == null && bridge[1] == null ||
                            bridge[0] != null && !bridge[0].Equals(minimalDistance.Item1) ||
                            bridge[1] != null && !bridge[1].Equals(minimalDistance.Item2))
                        {
                            var resultVertex1 = minimalDistance.Item1 as Vertex;
                            if (resultVertex1 != null)
                            {
                                bridge[0] = resultVertex1;
                                minVertexA1 = null;
                                minVertexA2 = null;
                            }
                            else
                            {
                                bridge[0] = new Vertex(minimalDistance.Item1);
                                minVertexA1 = pathA[startIndexI];
                                minVertexA2 = pathA[endIndexI];
                            }

                            var resultVertex2 = minimalDistance.Item2 as Vertex;
                            if (resultVertex2 != null)
                            {
                                bridge[1] = resultVertex2;
                                minVertexB1 = null;
                                minVertexB2 = null;
                            }
                            else
                            {
                                bridge[1] = new Vertex(minimalDistance.Item2);
                                minVertexB1 = pathB[startIndexJ];
                                minVertexB2 = pathB[endIndexJ];
                            }

                            minDistance = minimalDistance.Item3;
                        }
                    }
                }
            }

            if (minVertexA1 != null && minVertexA2 != null)
            {
                /*minVertexA1.Neigbours.Remove(minVertexA2);
                minVertexA2.Neigbours.Remove(minVertexA1);*/
                bridge[0].Neigbours.Add(minVertexA1);
                bridge[0].Neigbours.Add(minVertexA2);
                minVertexA1.Neigbours.Add(bridge[0]);
                minVertexA2.Neigbours.Add(bridge[0]);
            }

            if (minVertexB1 != null && minVertexB2 != null)
            {
                /*minVertexB1.Neigbours.Remove(minVertexB2);
                minVertexB2.Neigbours.Remove(minVertexB1);*/
                minVertexB1.Neigbours.Add(bridge[1]);
                minVertexB2.Neigbours.Add(bridge[1]);
                bridge[1].Neigbours.Add(minVertexB1);
                bridge[1].Neigbours.Add(minVertexB2);
            }
            bridge[0].Neigbours.Add(bridge[1]);
            bridge[1].Neigbours.Add(bridge[0]);

            return bridge;
        }
    }
}