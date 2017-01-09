
namespace Infrastructure.ExceptionHandling.SemanticLogging.EventListenersHandling.Listeners
{
    internal interface IListener
    {
        void RegisterListener(ListenerCreationDataAndEventSources listenerCreationDataAndEventSources);
    }
}
