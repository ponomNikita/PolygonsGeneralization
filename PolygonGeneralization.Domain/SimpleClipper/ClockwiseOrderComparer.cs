using System;
using System.Collections.Generic;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain.SimpleClipper
{
    public class ClockwiseOrderComparer : IComparer<Point>
    {
        private readonly VectorGeometry _vectorGeometry;
        private readonly Point _origin;
        private readonly Point _reference;

        public ClockwiseOrderComparer(Point origin, Point reference)
        {
            _origin = origin;
            _reference = reference;
            _vectorGeometry = new VectorGeometry();
        }

        public int Compare(Point x, Point y)
        {
            var sideX = _vectorGeometry.GetSide(_origin, _reference, x);
            var sideY = _vectorGeometry.GetSide(_origin, _reference, y);

            if (sideX >= 0 && sideY < 0)
            {
                return -1;
            }
            
            if (sideX < 0 && sideY >= 0)
            {
                return 1;
            }

            /*var projectionX = _vectorGeometry.CalculateProjection(_origin, _reference, x, false);
            var projectionY = _vectorGeometry.CalculateProjection(_origin, _reference, y, false);

            if (Math.Abs(projectionX.Item2 - projectionY.Item2) < Double.Epsilon)
            {
                if (Math.Abs(projectionX.Item2) < Double.Epsilon &&
                    Math.Abs(projectionY.Item2) < Double.Epsilon)
                {
                    if (projectionX.Item3 > 0 && projectionY.Item3 > 0)
                    {
                        return projectionX.Item3 > projectionY.Item3 ? -1 : 1;
                    }
                    
                    return projectionX.Item3 > projectionY.Item3 ? 1 : -1;
                }
                
                var xProjLength = _vectorGeometry.DistanceSqr(_origin, projectionX.Item1);
                var yProjLength = _vectorGeometry.DistanceSqr(_origin, projectionY.Item1);

                return xProjLength > yProjLength ? -1 : 1;
            }*/
            
            var det = (x.X - _reference.X) * (y.Y - _reference.Y) - (y.X - _reference.X) * (x.Y - _reference.Y);
            if (det < 0)
                return -1;
            if (det > 0)
                return 1;

            {
                var distanceFromReferenceX = _vectorGeometry.DistanceSqr(x, _reference);
                var distanceFromReferenceY = _vectorGeometry.DistanceSqr(y, _reference);

                return distanceFromReferenceX < distanceFromReferenceY ? 1 : -1;
            }
        }

        
    }
}