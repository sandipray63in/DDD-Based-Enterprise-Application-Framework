
namespace Infrastructure.Logging.SemanticLogging.EventListenersHandling
{
    public enum ListenerType { Console, FlatFile, RollingFile }

    public class ListenerData
    {
        public ListenerType ListenerType { get; set; }

        public ListenerCreationDataAndEventSources ListenerCreationDataAndEventSources { get; set; }
    }
}
