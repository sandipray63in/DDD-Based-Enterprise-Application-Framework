using System.Collections.Generic;
using Infrastructure.ExceptionHandling.SemanticLogging.EventListenersHandling.ListenerCreationData;

namespace Infrastructure.ExceptionHandling.SemanticLogging.EventListenersHandling
{
    public class ListenerCreationDataAndEventSources
    {
        public IListenerCreationData ListenerCreationData { get; set; }

        public IList<EventSourceAndLevel> EventSourcesAlongWithLevel { get; set; }
    }
}
