using System;
using System.Threading.Tasks;
using Infrastructure.Logging.Loggers;

namespace Infrastructure.Extensions
{
    public static class AsyncLoggerExtensions
    {
        public static async Task LogExceptionAsync(this ILogger logger, Exception ex)
        {
            await Task.Run(() => logger.LogException(ex));
        }

        public static async Task LogMessageAsync(this ILogger logger, string message)
        {
            await Task.Run(() => logger.LogMessage(message));
        }

        public static async Task LogWarningAsync(this ILogger logger, string message)
        {
            await Task.Run(() => logger.LogWarning(message));
        }
    }
}
