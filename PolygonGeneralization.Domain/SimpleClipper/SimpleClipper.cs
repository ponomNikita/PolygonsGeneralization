using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain.SimpleClipper
{
    public class SimpleClipper : IClipper
    {
        private readonly GraphHelper _graphHelper = new GraphHelper();
        
        public Task<Polygon> Union(Polygon a, Polygon b)
        {
            var pathA = a.Paths.First().Points;
            var pathB = b.Paths.First().Points;

            var minDistance = Double.MaxValue;
            var firstPair = new Tuple<Point, Point>(pathA.First(), pathB.First());
            foreach (var pointFromA in pathA)
            {
                foreach (var pointFormB in pathB)
                {
                    var distance = DistanceSqr(pointFromA, pointFormB);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        firstPair = new Tuple<Point, Point>(pointFromA, pointFormB);
                    }
                }
            }
            
            minDistance = Double.MaxValue;
            var secondPair = new Tuple<Point, Point>(pathA.First(), pathB.First());
            foreach (var pointFromA in pathA)
            {
                foreach (var pointFormB in pathB)
                {
                    if (pointFromA.Equals(firstPair.Item1) || pointFormB.Equals(firstPair.Item2))
                        continue;
                    
                    var distance = DistanceSqr(pointFromA, pointFormB);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        firstPair = new Tuple<Point, Point>(pointFromA, pointFormB);
                    }
                }
            }

            var union = UnionPaths(pathA, pathB, firstPair, secondPair);

            var paths = new List<Path> {union};
            paths.AddRange(a.Paths.Skip(1).Union(b.Paths.Skip(1)));
            
            var result = new Polygon(paths.ToArray());

            return Task.FromResult(result);
        }

        private double DistanceSqr(Point a, Point b)
        {
            return a.X * b.X + a.Y * b.Y;
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
                        ? new AntiClockwiseOrderComparer(current.Point, 
                        new Point(Double.MaxValue, current.Point.Y))
                        : new AntiClockwiseOrderComparer(prev, current.Point);
                    
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