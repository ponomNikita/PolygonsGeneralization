using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain.SimpleClipper
{
    public class GraphHelper
    {
        public HashSet<Vertex> BuildGraph(
            List<Point> pathA,
            List<Point> pathB,
            Tuple<Point, Point> firstPair,
            Tuple<Point, Point> secondPair)
        {
            var graphA = BuildRing(pathA);
            var graphB = BuildRing(pathB);

            var bridge1A = graphA.Find(v => v.Point.Equals(firstPair.Item1));
            var bridge1B = graphB.Find(v => v.Point.Equals(firstPair.Item2));

            var bridge2A = graphA.Find(v => v.Point.Equals(secondPair.Item1));
            var bridge2B = graphB.Find(v => v.Point.Equals(secondPair.Item2));
            
            bridge1A.Neigbours.Add(bridge1B);
            bridge1B.Neigbours.Add(bridge1A);
            
            bridge2A.Neigbours.Add(bridge2B);
            bridge2B.Neigbours.Add(bridge2A);
            
            var resultGraph = new HashSet<Vertex>();
            
            foreach (var item in graphA)
            {
                resultGraph.Add(item);
            }
            
            foreach (var item in graphB)
            {
                resultGraph.Add(item);
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
    }
}