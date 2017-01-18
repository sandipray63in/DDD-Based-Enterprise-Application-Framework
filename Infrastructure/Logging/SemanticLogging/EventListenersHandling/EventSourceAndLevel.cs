using System.Diagnostics.Tracing;

namespace Infrastructure.Logging.SemanticLogging.EventListenersHandling
{
    public class EventSourceAndLevel
    {
        public EventSource EventSource { get; set; }

        public EventLevel EventLevel { get; set; }
    }
}
