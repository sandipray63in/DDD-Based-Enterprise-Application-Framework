using System;
using Infrastructure.Logging.SemanticLogging.CrossCuttingEventSources;

namespace Infrastructure.Logging.Loggers
{
    public class SemanticLogger : ILogger
    {
        public virtual void LogException(Exception ex)
        {
            ExceptionLogEvents.Log.LogException(ex.ToString());
        }

        public virtual void LogMessage(string message)
        {
            MessageLogEvents.Log.LogMessage(message);
        }
    }
}
