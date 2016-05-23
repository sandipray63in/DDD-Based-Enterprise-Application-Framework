
namespace Infrastructure.SemanticLogging.EventListenersHandling.Listeners
{
    internal interface IListener
    {
        void RegisterListener(ListenerCreationDataAndEventSources listenerCreationDataAndEventSources);
    }
}
