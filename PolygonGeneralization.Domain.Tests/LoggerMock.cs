using System;
using PolygonGeneralization.Domain.Interfaces;

namespace PolygonGeneralization.Domain.Tests
{
    public class LoggerMock : ILogger
    {
        public void Log(string log)
        {
        }

        public string GetLog()
        {
            return null;
        }

        public void Clear()
        {
        }

        public void AddEventHandler(EventHandler handler)
        {
        }
    }
}