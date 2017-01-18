using System;

namespace Infrastructure.Logging.Loggers
{
    public interface ILogger
    {
        void LogException(Exception ex);
        void LogMessage(string message);
    }
}
