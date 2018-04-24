using System;

namespace PolygonGeneralization.Domain.Interfaces
{
    public interface ILogger
    {
        void Log(string log);
        string GetLog();

        void Clear();

        void AddEventHandler(EventHandler handler);
    }
}