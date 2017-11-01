using System;
using System.Collections;
using System.Collections.Generic;

namespace MultiKeyDictionary
{
    public class MultiKeyDictionary<TKey1, TKey2, TEntity> : IEnumerable
    {
        public MultiKeyDictionary()
        {
        }

        public int Count { get; }

        public bool Remove(TEntity entity)
        {
            throw  new NotImplementedException();
        }

        public TEntity Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public TEntity FindByKey1(TKey1 key)
        {
            throw new NotImplementedException();
        }

        public TEntity FindByKey2(TKey2 key)
        {
            throw new NotImplementedException();
        }

        public IList<TEntity> ToList()
        {
            throw new NotImplementedException();
        }

        public IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
