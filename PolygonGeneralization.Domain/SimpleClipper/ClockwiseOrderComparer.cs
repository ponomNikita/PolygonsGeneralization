﻿using System;
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
            var sideX = _vectorGeometry.GetSide(_reference, _origin, x);
            var sideY = _vectorGeometry.GetSide(_reference, _origin, y);

            var projectionX = _vectorGeometry.CalculateProjection(_reference, _origin, x, false);
            var projectionY = _vectorGeometry.CalculateProjection(_reference, _origin, y, false);

            if ((sideX > 0 || sideX == 0 && projectionX.Item3 > 0) && (sideY < 0 || sideY == 0 && projectionY.Item3 < 0))
            {
                return -1;
            }
            
            if ((sideX < 0 || sideX == 0 && projectionX.Item3 < 0) && (sideY > 0 || sideY == 0 && projectionY.Item3 > 0))
            {
                return 1;
            }
            
            var det = (x.X - _reference.X) * (y.Y - _reference.Y) - (y.X - _reference.X) * (x.Y - _reference.Y);
            if (det < 0)
                return 1;
            if (det > 0)
                return -1;

            {
                var distanceFromReferenceX = _vectorGeometry.DistanceSqr(x, _reference);
                var distanceFromReferenceY = _vectorGeometry.DistanceSqr(y, _reference);

                return distanceFromReferenceX < distanceFromReferenceY ? -1 : 1;
            }
        }

        
    }
}