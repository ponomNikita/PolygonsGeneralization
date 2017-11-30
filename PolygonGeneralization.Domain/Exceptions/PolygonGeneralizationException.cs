using System;

namespace PolygonGeneralization.Domain.Exceptions
{
    public class PolygonGeneralizationException : Exception
    {
        public PolygonGeneralizationException(string message)
            : base(message)
        {}
    }
}