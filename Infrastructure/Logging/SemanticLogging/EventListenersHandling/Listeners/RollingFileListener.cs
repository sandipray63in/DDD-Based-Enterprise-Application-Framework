using System;
using System.Diagnostics.Tracing;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Infrastructure.Logging.SemanticLogging.EventListenersHandling.ListenerCreationData;
using Infrastructure.Utilities;

namespace Infrastructure.Logging.SemanticLogging.EventListenersHandling.Listeners
{
    internal class RollingFileListener : BaseListener
    {
        protected override EventListener GetListener(IListenerCreationData listenerCreationData)
        {
            ContractUtility.Requires<ArgumentNullException>(listenerCreationData.IsNotNull(), "Listener Creation Data cannot be null");
            var rollingFileListenerCreationData = listenerCreationData as RollingFileListenerCreationData;
            return RollingFlatFileLog.CreateListener(rollingFileListenerCreationData.FileName, rollingFileListenerCreationData.RollSizeKB,
                rollingFileListenerCreationData.TimestampPattern, rollingFileListenerCreationData.RollFileExistsBehavior,
                rollingFileListenerCreationData.RollInterval, rollingFileListenerCreationData.Formatter, rollingFileListenerCreationData.MaxArchivedFiles,
                rollingFileListenerCreationData.IsAsync);
        }
    }
}
