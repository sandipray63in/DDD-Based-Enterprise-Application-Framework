using System.Collections.Immutable;
using Infrastructure.Logging.Loggers;

namespace Infrastructure.Logging
{
    public enum LoggerType
    {
        Default,
        SemanticLogging
    }

    public static class LoggerFactory
    {
        private static readonly ImmutableDictionary<LoggerType, ILogger> loggers;

        static LoggerFactory()
        {
            var loggerBuilder = ImmutableDictionary.CreateBuilder<LoggerType, ILogger>();
            loggerBuilder.Add(LoggerType.Default, new SemanticLogger());
            loggerBuilder.Add(LoggerType.SemanticLogging, new SemanticLogger());
            loggers = loggerBuilder.ToImmutable();
        }

        public static ILogger GetLogger(LoggerType loggerType)
        {
            return loggers[loggerType];
        }
    }
}
