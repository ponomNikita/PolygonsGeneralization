using PolygonGeneralization.Domain.Interfaces;

namespace PolygonGeneralization.Infrastructure.Logger
{
    public static class LoggerFactory
    {
        private static readonly ILogger Instance;
        static LoggerFactory()
        {
            Instance = new Logger();
        }
        public static ILogger Create()
        {
            return Instance;
        }
    }
}