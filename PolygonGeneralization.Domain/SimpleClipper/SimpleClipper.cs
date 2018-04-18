using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain.SimpleClipper
{
    public class SimpleClipper : IClipper
    {
        private readonly VectorGeometry _vectorGeometry = new VectorGeometry();
        private readonly GraphHelper _graphHelper = new GraphHelper();
        
        public Task<Polygon> Union(Polygon a, Polygon b)
        {
            var pathA = a.Paths.First().Points;
            var pathB = b.Paths.First().Points;

            Tuple<Point, Point> secondPair;
            var bridge = GetBridge(pathA, pathB);

            if (bridge.Length != 4 && bridge.Distinct().Count() != 4)
            {
                return Task.FromResult((Polygon)null);
            }
            
            var union = UnionPaths(pathA, pathB, new Tuple<Point, Point>(bridge[0], bridge[1]),
                new Tuple<Point, Point>(bridge[2], bridge[3]));

            var paths = new List<Path> {union};
            paths.AddRange(a.Paths.Skip(1).Union(b.Paths.Skip(1)));
            
            var result = new Polygon(paths.ToArray());

            return Task.FromResult(result);
        }

        public Point[] GetBridge(List<Point> pathA, List<Point> pathB)
        {
            var bridge = new Point[4];
            
            var minDistance = Double.MaxValue;
            foreach (var pFromA in pathA)
            {
                for (var i = 0; i < pathB.Count; i++)
                {
                    var projection = i != pathB.Count - 1
                        ? _vectorGeometry.CalculateProjection(pFromA, pathB[i], pathB[i + 1])
                        : _vectorGeometry.CalculateProjection(pFromA, pathB[i], pathB[0]);
                    if (projection.Item2 < minDistance)
                    {
                        bridge[0] = pFromA;
                        bridge[1] = projection.Item1;
                    }
                }
            }
            
            minDistance = Double.MaxValue;
            foreach (var pFromB in pathB)
            {
                for (var i = 0; i < pathA.Count; i++)
                {
                    var projection = i != pathB.Count - 1
                        ? _vectorGeometry.CalculateProjection(pFromB, pathA[i], pathA[i + 1])
                        : _vectorGeometry.CalculateProjection(pFromB, pathA[i], pathA[0]);
                    if (projection.Item2 < minDistance)
                    {
                        bridge[2] = projection.Item1;
                        bridge[3] = pFromB;
                    }
                }
            }

            return bridge;
        }

        public Path UnionPaths(
            List<Point> pathA, 
            List<Point> pathB, 
            Tuple<Point, Point> firstPair,
            Tuple<Point, Point> secondPair)
        {
            var graph = _graphHelper.BuildGraph(pathA, pathB, firstPair, secondPair);

            var current = graph.OrderBy(it => it.Point.X).ThenBy(it => it.Point.Y).First();
            
            var start = new Point(current.Point.X, current.Point.Y);

            var resultPoint = new List<Point>();
            Point prev = null;
            
            do
            {
                resultPoint.Add(current.Point);
                if (current.Neigbours.Count == 1)
                {
                    prev = current.Point;
                    current = current.Neigbours.Single();
                }
                else
                {
                    
                    var comparer = prev == null 
                        ? new ClockwiseOrderComparer(current.Point, 
                        new Point(Double.MaxValue, current.Point.Y))
                        : new ClockwiseOrderComparer(prev, current.Point);
                    
                    var minAntiClockwisePoint = current.Neigbours[0];
                    foreach (var item in current.Neigbours)
                    {
                        if (comparer.Compare(minAntiClockwisePoint.Point, item.Point) > 0)
                        {
                            minAntiClockwisePoint = item;
                        }    
                    }
                    
                    prev = current.Point;
                    current = minAntiClockwisePoint;
                }
                
            } while (!current.Point.Equals(start));

            return new Path(resultPoint.ToArray());
        }

        
    }
}