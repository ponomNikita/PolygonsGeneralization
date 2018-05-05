using System;
using System.Collections.Generic;
using System.Linq;
using PolygonGeneralization.Domain.Exceptions;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain.SimpleClipper
{
    public class SuperClipper : IClipper
    {
        private readonly VectorGeometry _vectorGeometry = new VectorGeometry();
        private readonly GraphHelper _graphHelper = new GraphHelper();

        public List<Polygon> Union(Polygon a, Polygon b, double minDistance)
        {
            var pathA = a.Paths.First().Points;
            var pathB = b.Paths.First().Points;

            try
            {
                var massCenterA = a.MassCenter ?? _vectorGeometry.GetMassCenter(pathA.ToArray());
                var massCenterB = b.MassCenter ?? _vectorGeometry.GetMassCenter(pathB.ToArray());

                var increasedPathA = _vectorGeometry.IncreaseContour(pathA.ToArray(), massCenterB, - minDistance / 2);
                var increasedPathB = _vectorGeometry.IncreaseContour(pathB.ToArray(), massCenterA, - minDistance / 2);

                var union = UnionPaths(increasedPathA, increasedPathB, minDistance);

                var original = _vectorGeometry.IncreaseContour(union, minDistance / 2);

                var paths = new List<Path> { new Path(original.ToArray()) };
                paths.AddRange(a.Paths.Skip(1).Union(b.Paths.Skip(1)));

                var result = new Polygon(paths.ToArray());

                return new List<Polygon> { result };
            }
            catch (PolygonGeneralizationException e)
            {
                return new List<Polygon> { a, b };
            }
        }

        public IEnumerable<Point> UnionPaths(
            IEnumerable<Point> pathA,
            IEnumerable<Point> pathB, double minDistance)
        {
            var graph = BuildGraph(pathA, pathB, minDistance);

            var current = graph.OrderBy(it => it.X).ThenBy(it => it.Y).First();

            var start = new Point(current.X, current.Y);

            var resultPoint = new List<Point>();
            Point prev = null;
            var maxPointCount = (pathA.Count() + pathB.Count()) * 2;
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

                    var minAntiClockwisePoint = current.Neigbours.First(it => !it.Equals(prev) && !it.Equals(current));
                    foreach (var item in current.Neigbours.Where(it => !it.Equals(prev) && !it.Equals(current)))
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

            return resultPoint;
        }

        private IEnumerable<Vertex> BuildGraph(IEnumerable<Point> pathA, IEnumerable<Point> pathB, double minDistance)
        {
            var ringA = _graphHelper.BuildRing(pathA.ToList());
            var ringB = _graphHelper.BuildRing(pathB.ToList());
            var intersections = new List<Vertex>();

            for (var i = 0; i < ringA.Count; i++)
            {
                var startIndexI = i;
                var endIndexI = i == ringA.Count - 1 ? 0 : i + 1;

                for (var j = 0; j < ringB.Count; j++)
                {
                    var startIndexJ = j;
                    var endIndexJ = j == ringB.Count - 1 ? 0 : j + 1;

                    var intersection = _vectorGeometry.GetIntersection(ringA[startIndexI], ringA[endIndexI],
                        ringB[startIndexJ], ringB[endIndexJ]);

                    if (intersection != null)
                    {
                        var newItem = new Vertex(intersection);

                        newItem.Neigbours.Add(ringA[startIndexI]);
                        newItem.Neigbours.Add(ringB[startIndexJ]);
                        newItem.Neigbours.Add(ringA[endIndexI]);
                        newItem.Neigbours.Add(ringB[endIndexJ]);

                        ringA[startIndexI].Neigbours.Remove(ringA[endIndexI]);
                        ringA[endIndexI].Neigbours.Remove(ringA[startIndexI]);
                        ringB[startIndexJ].Neigbours.Remove(ringB[endIndexJ]);
                        ringB[endIndexJ].Neigbours.Remove(ringB[startIndexJ]);

                        ringA[startIndexI].Neigbours.Add(newItem);
                        ringA[endIndexI].Neigbours.Add(newItem);
                        ringB[startIndexJ].Neigbours.Add(newItem);
                        ringB[endIndexJ].Neigbours.Add(newItem);

                        intersections.Add(newItem);
                    }
                }
            }

            if (!intersections.Any())
            {
                throw new PolygonGeneralizationException("No intersections");
            }

            var result = new HashSet<Vertex>();
            result.AddRange(ringA);
            result.AddRange(ringB);
            result.AddRange(intersections);

            return result;
        }
    }
}