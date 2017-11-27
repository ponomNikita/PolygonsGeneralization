using System.Diagnostics;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Infrastructure.Logger;

namespace PolygonGeneralization.Infrastructure.Commands
{
    public abstract class BaseCommand : ICommand
    {
        private readonly ILogger _logger = LoggerFactory.Create();
        private readonly Stopwatch _stopWatch = new Stopwatch();
        private string _time;

        public void Handle()
        {
            if (UseLogging)
                _logger.Log($"{CommandName} started");

            _stopWatch.Reset();
            _stopWatch.Start();

            HandleImpl();

            _stopWatch.Stop();
            _time = _stopWatch.Elapsed.ToString("mm\\:ss\\.ff");

            if (UseLogging)
                _logger.Log($"Done {_time}");
        }

        public abstract string CommandName { get; }
        public virtual bool UseLogging => true;
        public virtual string Time => _time;
        protected abstract void HandleImpl();
    }
}