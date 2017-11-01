using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MultiKeyDictionary
{
    public class MultiKeyDictionary<TKey1, TKey2, TEntity> : ICollection<TEntity>
        where TEntity :  IHasKey<TKey1>, IHasKey<TKey2>
    {
        private Dictionary<TKey1, TEntity> _dictByKey1 = new Dictionary<TKey1, TEntity>();
        private Dictionary<TKey2, TEntity> _dictByKey2 = new Dictionary<TKey2, TEntity>();
        private Dictionary<TEntity, TEntity> _dictByEntity = new Dictionary<TEntity, TEntity>();

        public int Count => _dictByKey1.Count;
        public bool IsReadOnly => false;

        private object _syncRoot = new object();
        public object SyncRoot { get { return _syncRoot; } }
        public bool IsSynchronized => true;
        
        public bool Remove(TEntity entity)
        {
            var key1 = (entity as IHasKey<TKey1>).GetKey();
            var key2 = (entity as IHasKey<TKey2>).GetKey();

            return _dictByKey1.Remove(key1) &&
                   _dictByKey2.Remove(key2) && 
                   _dictByEntity.Remove(entity);
        }

        public void Add(TEntity entity)
        {
            var key1 = (entity as IHasKey<TKey1>).GetKey();
            var key2 = (entity as IHasKey<TKey2>).GetKey();

            if (!_dictByKey1.ContainsKey(key1) &&
                !_dictByKey2.ContainsKey(key2) &&
                !_dictByEntity.ContainsKey(entity))
            {
                _dictByKey1.Add(key1, entity);
                _dictByKey2.Add(key2, entity);
                _dictByEntity.Add(entity, entity);
            }
        }

        public void Clear()
        {
            _dictByEntity.Clear();
            _dictByKey1.Clear();
            _dictByKey2.Clear();
        }

        public TEntity FindByKey1(TKey1 key)
        {
            if (_dictByKey1.ContainsKey(key))
            {
                return _dictByKey1[key];
            }

            return default(TEntity);
        }

        public TEntity FindByKey2(TKey2 key)
        {
            if (_dictByKey2.ContainsKey(key))
            {
                return _dictByKey2[key];
            }

            return default(TEntity);
        }

        public List<TEntity> ToList()
        {
            return _dictByEntity.Values.ToList();
        }

        public bool Contains(TEntity entity)
        {
            return entity != null && _dictByEntity.ContainsKey(entity);
        }

        IEnumerator<TEntity> IEnumerable<TEntity>.GetEnumerator()
        {
            return _dictByEntity.Values.GetEnumerator();
        }

        public IEnumerator GetEnumerator()
        {
            return _dictByEntity.Values.GetEnumerator();
        }
        public void CopyTo(TEntity[] array, int arrayIndex)
        {
            _dictByEntity.Values.CopyTo(array, arrayIndex);
        }
    }
}
