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
            var bridge = new Vertex[4];
            
            var minDistance = Double.MaxValue;
            Vertex minVertexA1 = null;
            Vertex minVertexB1 = null;
            Vertex minVertexA2 = null;
            Vertex minVertexB2 = null;
            
            Vertex minVertexC1 = null;
            Vertex minVertexD1 = null;
            Vertex minVertexC2 = null;
            Vertex minVertexD2 = null;
            
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

                    if (minimalDistance.Item3 <= minDistance)
                    {
                        if (bridge[0] == null && bridge[1] == null || 
                            bridge[0] != null && bridge[1] != null &&
                        !bridge[0].Equals(minimalDistance.Item1) &&
                            !bridge[1].Equals(minimalDistance.Item2))
                        {
                            bridge[2] = bridge[0];
                            bridge[3] = bridge[1];

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
                                minVertexC1 = minVertexA1;
                                minVertexC2 = minVertexA2;
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
                                minVertexD1 = minVertexB1;
                                minVertexD2 = minVertexB2;
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
                minVertexA1.Neigbours.Remove(bridge[0]);
                minVertexA2.Neigbours.Remove(bridge[0]);
                bridge[0].Neigbours.Add(minVertexA1);
                bridge[0].Neigbours.Add(minVertexA2);
            }

            if (minVertexB1 != null && minVertexB2 != null)
            {
                minVertexB1.Neigbours.Remove(bridge[1]);
                minVertexB2.Neigbours.Remove(bridge[1]);
                bridge[1].Neigbours.Add(minVertexB1);
                bridge[1].Neigbours.Add(minVertexB2);
            }

            if (minVertexC1 != null && minVertexC2 != null)
            {
                minVertexC1.Neigbours.Remove(bridge[2]);
                minVertexC2.Neigbours.Remove(bridge[2]);
                bridge[2].Neigbours.Add(minVertexC1);
                bridge[2].Neigbours.Add(minVertexC2);
            }

            if (minVertexD1 != null && minVertexD2 != null)
            {
                minVertexD1.Neigbours.Remove(bridge[3]);
                minVertexD2.Neigbours.Remove(bridge[3]);
                bridge[3].Neigbours.Add(minVertexD1);
                bridge[3].Neigbours.Add(minVertexD2);
            }
            
            bridge[0].Neigbours.Add(bridge[1]);
            bridge[1].Neigbours.Add(bridge[0]);
            bridge[2].Neigbours.Add(bridge[3]);
            bridge[3].Neigbours.Add(bridge[2]);

            return bridge;
        }
        
        /*public Vertex[] GetBridge(List<Vertex> pathA, List<Vertex> pathB)
        {
            var bridge = new Vertex[4];
            
            var minDistance = Double.MaxValue;
            Vertex minVertexA1 = null;
            Vertex minVertexB1 = null;
            
            foreach (var pFromA in pathA)
            {
                for (var i = 0; i < pathB.Count; i++)
                {
                    var vertexA = pathB[i];
                    var vertexB = i != pathB.Count - 1 ? pathB[i + 1] : pathB[0];
                    var projection = _vectorGeometry.CalculateProjection(vertexA.Point, vertexB.Point, pFromA.Point);
                    
                    if (projection.Item2 < minDistance)
                    {
                        if (projection.Item3 < 1 && projection.Item3 > 0)
                        {
                            bridge[1] = new Vertex(projection.Item1);
                            minVertexA1 = vertexA;
                            minVertexB1 = vertexB;
                        }
                        else {
                            
                            minVertexA1 = null;
                            minVertexB1 = null;
                            
                            if (projection.Item1.Equals(vertexA.Point))
                            {
                                bridge[1] = vertexA;
                            }
                            else
                            {
                                bridge[1] = vertexB;   
                            }
                        }
                        
                        bridge[0] = pFromA;
                        minDistance = projection.Item2;
                    }
                }
            }

            if (minVertexA1 != null && minVertexB1 != null) // Если bridge[1] не на концах отрезка
            {
                minVertexA1.Neigbours.Remove(minVertexB1);
                minVertexB1.Neigbours.Remove(minVertexA1);
                
                bridge[1].Neigbours.Add(minVertexA1);
                bridge[1].Neigbours.Add(minVertexB1);
                
                if (bridge[1].Neigbours.Any())
                {
                    foreach (var neigbour in bridge[1].Neigbours)
                    {
                        neigbour.Neigbours.Add(bridge[1]);
                    }
                }
            }

            bridge[0].Neigbours.Add(bridge[1]);
            bridge[1].Neigbours.Add(bridge[0]);
            
            minDistance = Double.MaxValue;
            Vertex minVertexA2 = null;
            Vertex minVertexB2 = null;
            
            foreach (var pFromB in pathB)
            {
                if (pFromB.Point.Equals(bridge[1].Point))
                {
                    continue;
                }
                
                for (var i = 0; i < pathA.Count; i++)
                {
                    if (pathA[i].Point.Equals(bridge[0].Point))
                    {
                        continue;
                    }
                    
                    var vertexA = pathA[i];
                    var vertexB = i != pathA.Count - 1 ? pathA[i + 1] : pathA[0];

                    var projection = _vectorGeometry.CalculateProjection(vertexA.Point, vertexB.Point, pFromB.Point);
                    if (projection.Item2 < minDistance)
                    {
                        if (projection.Item3 > 0 && projection.Item3 < 1)
                        {
                            bridge[2] = new Vertex(projection.Item1);
                            minVertexA2 = vertexA;
                            minVertexB2 = vertexB;
                        }
                        else
                        {
                            minVertexA2 = null;
                            minVertexB2 = null;
                            
                            if (projection.Item1.Equals(vertexA.Point))
                            {
                                bridge[2] = vertexA;
                            }
                            else
                            {
                                bridge[2] = vertexB;
                            }
                        }
                        
                        bridge[3] = pFromB;
                        minDistance = projection.Item2;
                    }
                }
            }


            if (minVertexA2 != null && minVertexB2 != null)
            {
                minVertexA2.Neigbours.Remove(minVertexB2);
                minVertexB2.Neigbours.Remove(minVertexA2);
                
                bridge[2].Neigbours.Add(minVertexA2);
                bridge[2].Neigbours.Add(minVertexB2);
            
                if (bridge[2].Neigbours.Any())
                {
                    foreach (var neigbour in bridge[2].Neigbours)
                    {
                        neigbour.Neigbours.Add(bridge[2]);
                    }
                }
            }
            
            bridge[2].Neigbours.Add(bridge[3]);
            bridge[3].Neigbours.Add(bridge[2]);

            /*if (bridge.Distinct().Count() != 4)
            {
                throw new PolygonGeneralizationException("Bridge has less then 4 points");
            }#1#
            
            return bridge;
        }*/
    }
}