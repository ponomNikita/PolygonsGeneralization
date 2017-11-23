using System.Diagnostics;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Infrastructure.Logger;

namespace PolygonGeneralization.Infrastructure.Commands
{
    public abstract class BaseCommand : ICommand
    {
        private ILogger _logger = LoggerFactory.Create();
        private Stopwatch _stopWatch = new Stopwatch();
        private double _time;

        public void Handle()
        {
            if (UseLogging)
                _logger.Log($"{CommandName} started");

            _stopWatch.Reset();
            _stopWatch.Start();

            HandleImpl();

            _stopWatch.Stop();
            _time = _stopWatch.ElapsedMilliseconds/100.0;

            if (UseLogging)
                _logger.Log($"Done{_time} sec");
        }

        public abstract string CommandName { get; }
        public virtual bool UseLogging => true;
        public virtual double Time => _time;
        protected abstract void HandleImpl();
    }
}