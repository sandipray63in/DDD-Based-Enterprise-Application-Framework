using System.Diagnostics.Tracing;

namespace Infrastructure.Logging.SemanticLogging.CrossCuttingEventSources
{
    [EventSource(Name = "WarningLogEvents")]
    public class WarningLogEvents : EventSource
    {
        public static readonly WarningLogEvents Log = new WarningLogEvents();

        private const int warningMessageLogged = 1;

        [Event(warningMessageLogged, Message = @"{0}", Level = EventLevel.Warning)]
        public void LogWarning(string messsage)
        {
            if (IsEnabled()) WriteEvent(warningMessageLogged, messsage);
        }
    }
}
