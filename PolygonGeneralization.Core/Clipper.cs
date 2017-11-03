using System.Collections.Generic;
using System.Linq;

namespace PolygonGeneralization.Core
{
    using Paths = List<List<PointD>>;
    using Path = List<PointD>;
    public class Clipper
    {
        private Paths _subject;
        private Paths _clipping;
        private Paths _solution;

        // TODO Make hash structure with access to element by two keys (pointA, pointB)
        private HashSet<PointD> _pointsSet;
        private HashSet<Edge> _edgesSet;

        private readonly object _clipperLock = new object();
        private bool _isInProgress;
       
        public Clipper(Paths subject, Paths clipping)
        {
            _subject = subject;
            _clipping = clipping;
        }

        public Clipper()
        {
        }

        public bool IsInProgress()
        {
            return _isInProgress;
        }

        /// <summary>
        /// Only union implemented now
        /// </summary>
        public void Execute()
        {
            lock (_clipperLock)
            {
                _isInProgress = true;

                BuildModel();
                MarkEdges();
                BuildSolution();

                _isInProgress = false;
            }
        }

        public void SetSubject(Paths subject)
        {
            _subject = subject;
        }

        public void SetClipping(Paths clipping)
        {
            _clipping = clipping;
        }

        public Paths GetSolution()
        {
            return _solution;
        }

        private void BuildSolution()
        {
            while (_edgesSet.Count > 0)
            {
                // Строим контуры начиная с внешнего
                BuildContour();
            }
        }

        private void BuildContour()
        {
            var initialPoint = _edgesSet.OrderBy(e => e.A.X).ThenBy(e => e.A.Y).First().A;

            var initial = GetNextRightEdge(new Edge(new PointD(initialPoint.X, initialPoint.Y + 1), initialPoint),
                _edgesSet);
            
            Path contour = new Path();

            Edge current = initial;

            while (true)
            {
                _edgesSet.Remove(current);
                contour.Add(current.A);

                if (current.B.Equals(initial.A))
                    break;

                current = GetNextRightEdge(current, _edgesSet);
            }

            _solution.Add(contour);
        }

        private Edge GetNextRightEdge(Edge edge, HashSet<Edge> edgesSet)
        {
            var pointsForSortByClockwise = _edgesSet.Where(e => e.A == edge.B).Select(e => e.B).ToList();

            SortPointsByAntiClockwiseOrder(edge, pointsForSortByClockwise);

            var mostRightPoint = pointsForSortByClockwise.First();

            return edgesSet.FirstOrDefault(e => e.A == edge.B && e.B == mostRightPoint);
        }

        private void SortPointsByAntiClockwiseOrder(Edge edge, List<PointD> points)
        {
            points.Sort(new AntiClockwiseOrderComparer(edge.B, edge.A));
        }

        private void MarkEdges()
        {
            foreach (var edge in _edgesSet)
            {
                if (edge.IsFromSubject && !edge.IsFromClipping && !edge.IsInside(_clipping) ||
                    edge.IsFromClipping && !edge.IsFromSubject && !edge.IsInside(_subject))
                {
                    edge.IsResult = true;
                }
            }

            _edgesSet.RemoveWhere(e => !e.IsResult);
        }

        private void BuildModel()
        {
            if (_solution == null)
                _solution = new Paths();

            if (_pointsSet == null)
                _pointsSet = new HashSet<PointD>();

            if (_edgesSet == null)
                _edgesSet = new HashSet<Edge>();

            _solution.Clear();
            _pointsSet.Clear();
            _edgesSet.Clear();

            BuildFrom(_subject, true);
            BuildFrom(_clipping, false);
        }

        private void BuildFrom(Paths countours, bool isSubject)
        {
            foreach (var contour in countours)
            {
                var size = contour.Count;
                for (int i = 0; i < size; i++)
                {
                    _pointsSet.Add(contour[i]);
                    var newEdge = i == size - 1
                        ? new Edge(contour[i], contour[0])
                        : new Edge(contour[i], contour[i + 1]);

                    newEdge.IsFromSubject = isSubject; // TODO возможно второе поле лишнее
                    newEdge.IsFromClipping = !isSubject;

                    InsertEdge(newEdge);
                }
            }
        }

        private void InsertEdge(Edge newEdge)
        {
            bool success = true;

            var edgesList = _edgesSet.ToList();

            foreach (var existing in edgesList)
            {
                var subdivision = newEdge.GetSubdivision(existing);

                var existingEdgeSubdivision = existing.GetSubdivision(newEdge);
                if (existingEdgeSubdivision.Item2 != null)
                {
                    existingEdgeSubdivision.Item1.IsFromSubject = existing.IsFromSubject;
                    existingEdgeSubdivision.Item1.IsFromClipping = existing.IsFromClipping;
                    existingEdgeSubdivision.Item2.IsFromSubject = existing.IsFromSubject;
                    existingEdgeSubdivision.Item2.IsFromClipping = existing.IsFromClipping;

                    _edgesSet.Remove(existing);
                    _edgesSet.Add(existingEdgeSubdivision.Item1);
                    _edgesSet.Add(existingEdgeSubdivision.Item2);
                }

                if (subdivision.Item2 != null)
                {
                    subdivision.Item1.IsFromSubject = newEdge.IsFromSubject;
                    subdivision.Item1.IsFromClipping = newEdge.IsFromClipping;
                    subdivision.Item2.IsFromSubject = newEdge.IsFromSubject;
                    subdivision.Item2.IsFromClipping = newEdge.IsFromClipping;

                    InsertEdge(subdivision.Item1);
                    InsertEdge(subdivision.Item2);

                    success = false;
                    break;
                }
            }

            if (success)
            {
                _edgesSet.Add(newEdge);
                _pointsSet.Add(newEdge.A);
                _pointsSet.Add(newEdge.B);
            }
        }
    }
}
