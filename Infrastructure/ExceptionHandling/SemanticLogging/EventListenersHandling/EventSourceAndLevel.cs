using System.Diagnostics.Tracing;

namespace Infrastructure.ExceptionHandling.SemanticLogging.EventListenersHandling
{
    public class EventSourceAndLevel
    {
        public EventSource EventSource { get; set; }

        public EventLevel EventLevel { get; set; }
    }
}
