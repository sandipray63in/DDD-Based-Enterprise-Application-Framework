using System;
using System.Diagnostics.Tracing;
using System.Linq;
using Infrastructure.Logging.SemanticLogging.EventListenersHandling.ListenerCreationData;
using Infrastructure.Utilities;

namespace Infrastructure.Logging.SemanticLogging.EventListenersHandling.Listeners
{
    internal abstract class BaseListener : IListener
    {
        public void RegisterListener(ListenerCreationDataAndEventSources listenerCreationDataAndEventSources)
        {
            ContractUtility.Requires<ArgumentNullException>(listenerCreationDataAndEventSources.IsNotNull(), "listenerCreationDataAndEventSources instance cannot be null");

            var listenerCreationData = listenerCreationDataAndEventSources.ListenerCreationData;
            var eventListener = GetListener(listenerCreationData);
         
            listenerCreationDataAndEventSources.EventSourcesAlongWithLevel.ToList().ForEach(x =>
                {
                    eventListener.EnableEvents(x.EventSource, x.EventLevel, EventKeywords.All);
                }
            );
        }

        protected abstract EventListener GetListener(IListenerCreationData listenerCreationData);
    }
}
