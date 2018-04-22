using System.Collections.Generic;
using System.Linq;
using PolygonGeneralization.Domain.Models;

namespace PolygonGeneralization.Domain.LinearGeneralizer
{
    public class LinearGeneralizer : ILinearGeneralizer
    {
        private GeneralizerOptions _options;
        private readonly VectorGeometry _vectorGeometry = new VectorGeometry();
        private double _maxDifference;

        public LinearGeneralizer(GeneralizerOptions options)
        {
            _options = options;
            _maxDifference = options.MaxDifferenceInPercent / 100;
        }

        public Point[] Simplify(Point[] points)
        {
            var mutablePointList = points.Select(it => new MutableEntity<Point>(it)).ToList();

            for (var i = 0; i < mutablePointList.Count; i++)
            {
                var prevIndex = GetPrevIndex(mutablePointList, i);
                var nextIndex = GetNextIndex(mutablePointList, i);

                var distance1 = _vectorGeometry.Distance(mutablePointList[prevIndex].Entity,
                    mutablePointList[i].Entity);
                var distance2 = _vectorGeometry.Distance(mutablePointList[i].Entity,
                    mutablePointList[nextIndex].Entity);
                var distance12 = _vectorGeometry.Distance(mutablePointList[prevIndex].Entity,
                    mutablePointList[nextIndex].Entity);

                if (!IsSignificant(distance12, distance1, distance2))
                {
                    mutablePointList[i].Remove();
                }
            }

            mutablePointList.RemoveAll(it => it.IsRemoved);

            var result = mutablePointList.Select(it => it.Entity).ToArray();
            
            return result.Length == points.Length ? result : Simplify(result);
        }

        private bool IsSignificant(double distance12, params double[] distances)
        {
            if (distances.Length < 2)
                return false;
            
            return distances.Sum() > distance12 + distance12 * _maxDifference;
        }

        private int GetNextIndex(IEnumerable<object> objects, int index)
        {
            return index == objects.Count() - 1 ? 0 : index + 1;
        }

        private int GetPrevIndex(IEnumerable<object> objects, int index)
        {
            return index == 0 ? objects.Count() - 1 : index - 1;
        }
    }
}