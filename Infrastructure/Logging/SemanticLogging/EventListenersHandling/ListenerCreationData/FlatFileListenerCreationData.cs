using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;

namespace Infrastructure.Logging.SemanticLogging.EventListenersHandling.ListenerCreationData
{
    public class FlatFileListenerCreationData : IListenerCreationData
    {
        public string FileName { get; set; }

        public IEventTextFormatter EventTextFormatter { get; set; }

        public bool IsAsync { get; set; }

    }
}
