using System.Diagnostics.Tracing;

namespace Infrastructure.Logging.SemanticLogging.CrossCuttingEventSources
{
    [EventSource(Name = "ExceptionLogEvents")]
    public class ExceptionLogEvents : EventSource
    {
        public static readonly ExceptionLogEvents Log = new ExceptionLogEvents();

        private const int exceptionOccurred = 1;

        [Event(exceptionOccurred, Message = @"Exception Occurred:\r\n {0}", Level = EventLevel.Verbose)]
        public void LogException(string exception)
        {
            if (IsEnabled()) WriteEvent(exceptionOccurred, exception);
        }
    }
}