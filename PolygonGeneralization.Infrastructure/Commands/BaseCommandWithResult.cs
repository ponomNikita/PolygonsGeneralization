using System.Diagnostics;
using System.Threading.Tasks;
using PolygonGeneralization.Domain.Interfaces;
using PolygonGeneralization.Infrastructure.Logger;

namespace PolygonGeneralization.Infrastructure.Commands
{
    public abstract class BaseCommandWithResult<TResult>: ICommand<TResult>
    {
        private readonly ILogger _logger = LoggerFactory.Create();
        private readonly Stopwatch _stopWatch = new Stopwatch();
        private string _time;

        public TResult Execute()
        {
            if (UseLogging)
                _logger.Log($"{CommandName} started");

            _stopWatch.Reset();
            _stopWatch.Start();

            var result = HandleImpl();

            _stopWatch.Stop();
            _time = _stopWatch.Elapsed.ToString("mm\\:ss\\.ff");

            if (UseLogging)
                _logger.Log($"Done {_time}");

            return result;
        }

        public abstract string CommandName { get; }
        public virtual bool UseLogging => true;
        public virtual string Time => _time;
        protected abstract TResult HandleImpl();
    }
}