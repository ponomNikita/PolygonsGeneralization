using System;
using System.CodeDom;
using System.Collections;

namespace PolygonGeneralization.Domain
{
    public class MutableEntity<T>
    {
        public MutableEntity(T entity)
        {
            Entity = entity;
        }
        
        public bool IsRemoved { get; private set; }

        public T Entity { get; }

        public void Remove()
        {
            IsRemoved = true;
        }
    }
}