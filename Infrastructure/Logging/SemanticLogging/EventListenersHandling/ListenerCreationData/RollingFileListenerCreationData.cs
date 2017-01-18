using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Sinks;

namespace Infrastructure.Logging.SemanticLogging.EventListenersHandling.ListenerCreationData
{
    public class RollingFileListenerCreationData : IListenerCreationData
    {
        public string FileName { get; set; }

        public int RollSizeKB { get; set; }

        public string TimestampPattern { get; set; }

        public RollFileExistsBehavior RollFileExistsBehavior { get; set; }

        public RollInterval RollInterval { get; set; }

        public IEventTextFormatter Formatter { get; set; }

        public int MaxArchivedFiles { get; set; }

        public bool IsAsync { get; set; }

    }
}
