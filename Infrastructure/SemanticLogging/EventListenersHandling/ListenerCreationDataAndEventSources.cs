using System.Collections.Generic;
using Infrastructure.SemanticLogging.EventListenersHandling.ListenerCreationData;

namespace Infrastructure.SemanticLogging.EventListenersHandling
{
    public class ListenerCreationDataAndEventSources
    {
        public IListenerCreationData ListenerCreationData { get; set; }

        public IList<EventSourceAndLevel> EventSourcesAlongWithLevel { get; set; }
    }
}
