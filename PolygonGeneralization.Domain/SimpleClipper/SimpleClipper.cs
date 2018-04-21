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
            
            var union = UnionPaths(pathA, pathB);

            var paths = new List<Path> {union};
            paths.AddRange(a.Paths.Skip(1).Union(b.Paths.Skip(1)));
            
            var result = new Polygon(paths.ToArray());

            return Task.FromResult(result);
        }

        public Path UnionPaths(
            List<Point> pathA, 
            List<Point> pathB)
        {
            var graph = _graphHelper.BuildGraph(pathA, pathB);

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
                        ? new ClockwiseOrderComparer(new Point(Double.MaxValue, current.Point.Y), current.Point)
                        : new ClockwiseOrderComparer(current.Point, prev);
                    
                    var minAntiClockwisePoint = current.Neigbours.First();
                    foreach (var item in current.Neigbours)
                    {
                        if (item.Point.Equals(prev))
                        {
                            continue;
                        }
                        if (comparer.Compare(minAntiClockwisePoint.Point, item.Point) < 0)
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