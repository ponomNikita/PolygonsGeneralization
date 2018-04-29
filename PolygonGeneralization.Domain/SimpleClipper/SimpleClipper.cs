using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using PolygonGeneralization.Domain.Exceptions;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain.SimpleClipper
{
    public class SimpleClipper : IClipper
    {
        private readonly VectorGeometry _vectorGeometry = new VectorGeometry();
        private readonly GraphHelper _graphHelper = new GraphHelper();
        
        public List<Polygon> Union(Polygon a, Polygon b)
        {
            var pathA = a.Paths.First().Points;
            var pathB = b.Paths.First().Points;

            try
            {
                var union = UnionPaths(pathA, pathB);

                var paths = new List<Path> {union};
                paths.AddRange(a.Paths.Skip(1).Union(b.Paths.Skip(1)));
            
                var result = new Polygon(paths.ToArray());

                return new List<Polygon>{result};
            }
            catch (PolygonGeneralizationException e)
            {
                return new List<Polygon>{ a, b };
            }
        }

        public Path UnionPaths(
            List<Point> pathA, 
            List<Point> pathB)
        {
            var graph = _graphHelper.BuildGraph(pathA, pathB);

            var current = graph.OrderBy(it => it.X).ThenBy(it => it.Y).First();
            
            var start = new Point(current.X, current.Y);

            var resultPoint = new List<Point>();
            Point prev = null;
            var maxPointCount = (pathA.Count + pathB.Count) * 2;
            var iteration = 0;
            
            do
            {
                if (++iteration > maxPointCount)
                {
                    throw new PolygonGeneralizationException("Looping was happened");
                }
                
                resultPoint.Add(current);
                if (current.Neigbours.Count == 1)
                {
                    prev = current;
                    current = current.Neigbours.Single();
                }
                else
                {
                    var comparer = prev == null 
                        ? new ClockwiseOrderComparer(new Point(Double.MaxValue, current.Y), current)
                        : new ClockwiseOrderComparer(current, prev);
                    
                    var minAntiClockwisePoint = current.Neigbours.First(it => !it.Equals(prev));
                    foreach (var item in current.Neigbours.Where(it => !it.Equals(prev)))
                    {
                        if (item.Equals(prev))
                        {
                            continue;
                        }
                        if (comparer.Compare(item, minAntiClockwisePoint) < 0)
                        {
                            minAntiClockwisePoint = item;
                        }    
                    }
                    
                    prev = current;
                    current = minAntiClockwisePoint;
                }
                
            } while (!current.Equals(start));

            return new Path(resultPoint.ToArray());
        }

        
    }
}