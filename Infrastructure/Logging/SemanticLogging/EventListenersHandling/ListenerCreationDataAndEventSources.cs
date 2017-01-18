using System.Collections.Generic;
using Infrastructure.Logging.SemanticLogging.EventListenersHandling.ListenerCreationData;

namespace Infrastructure.Logging.SemanticLogging.EventListenersHandling
{
    public class ListenerCreationDataAndEventSources
    {
        public IListenerCreationData ListenerCreationData { get; set; }

        public IList<EventSourceAndLevel> EventSourcesAlongWithLevel { get; set; }
    }
}
