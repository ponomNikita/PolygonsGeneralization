using System;

namespace PolygonGeneralization.Infrastructure.Logger
{
    public interface ILogger
    {
        void Log(string log);
        string GetLog();

        void AddEventHandler(EventHandler handler);
    }
}