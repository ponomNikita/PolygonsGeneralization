using System;
using System.Collections.Generic;

namespace PolygonGeneralization.Core
{
    public class AntiClockwiseOrderComparer : IComparer<PointD>
    {
        private readonly PointD _origin;
        private PointD _reference;

        public AntiClockwiseOrderComparer(PointD origin, PointD reference)
        {
            _origin = origin;
            _reference = reference - origin;
        }

        /// <summary>
        /// https://stackoverflow.com/questions/7774241/sort-points-by-angle-from-given-axis
        /// </summary>
        public int Compare(PointD x, PointD y)
        {
            var da = x - _origin;
            var db = y - _origin;

            var detB = Xp(_reference, db);

            if (Math.Abs(detB) < Double.Epsilon && db.X * _reference.X + db.Y * _reference.Y >= 0)
            {
                return 1;
            }

            var detA = Xp(_reference, da);

            if (Math.Abs(detA) < Double.Epsilon && da.X * _reference.X + da.Y * _reference.Y >= 0)
            {
                return -1;
            }

            if (detA * detB >= 0)
                return Xp(da, db) > 0 ? -1 : 1;

            return detA > 0 ? -1 : 1;
        }

        private double Xp(PointD a, PointD b)
        {
            return a.X * b.Y - a.Y * b.X;
        }
    }
}