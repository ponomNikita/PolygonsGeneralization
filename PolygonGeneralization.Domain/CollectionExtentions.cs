using System.Collections.Generic;

namespace PolygonGeneralization.Domain
{
    public static class CollectionExtentions
    {
        public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                set.Add(item);
            }
        }
    }
}