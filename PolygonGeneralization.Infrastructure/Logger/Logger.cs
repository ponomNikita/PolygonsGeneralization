using System;
using System.Text;
using PolygonGeneralization.Domain.Interfaces;

namespace PolygonGeneralization.Infrastructure.Logger
{
    public class Logger : ILogger
    {
        private readonly StringBuilder _logs;

        public event EventHandler AddLogEvent;

        public Logger()
        {
            _logs = new StringBuilder();
        }
        public void Log(string log)
        {
            if (_logs.Length == 1000)
                _logs.Clear();

            _logs.AppendLine($"{DateTime.UtcNow}: {log}");
            OnAddLogEvent();
        }

        public string GetLog()
        {
            return _logs.ToString();
        }

        public void Clear()
        {
            _logs.Clear();
            OnAddLogEvent();
        }

        public void AddEventHandler(EventHandler handler)
        {
            AddLogEvent += handler;
        }

        protected virtual void OnAddLogEvent()
        {
            AddLogEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}