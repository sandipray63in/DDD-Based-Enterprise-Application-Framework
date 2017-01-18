using System.Diagnostics.Tracing;

namespace Infrastructure.Logging.SemanticLogging.CrossCuttingEventSources
{
    [EventSource(Name = "MessageLogEvents")]
    public class MessageLogEvents : EventSource
    {
        public static readonly MessageLogEvents Log = new MessageLogEvents();

        private const int messageLogged = 1;

        [Event(messageLogged, Message = @"{0}", Level = EventLevel.Verbose)]
        public void LogMessage(string messsage)
        {
            if (IsEnabled()) WriteEvent(messageLogged, messsage);
        }
    }
}
