using System;

namespace PolygonGeneralization.Domain.Interfaces
{
    public interface ILogger
    {
        void Log(string log);
        string GetLog();

        void AddEventHandler(EventHandler handler);
    }
}