using System;
using System.Diagnostics.Tracing;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Infrastructure.Logging.SemanticLogging.EventListenersHandling.ListenerCreationData;
using Infrastructure.Utilities;

namespace Infrastructure.Logging.SemanticLogging.EventListenersHandling.Listeners
{
    internal class FlatFileListener : BaseListener
    {
        protected override EventListener GetListener(IListenerCreationData listenerCreationData)
        {
            ContractUtility.Requires<ArgumentNullException>(listenerCreationData.IsNotNull(), "Listener Creation Data cannot be null");
            var flatFileListenerCreationData = listenerCreationData as FlatFileListenerCreationData;
            return FlatFileLog.CreateListener(flatFileListenerCreationData.FileName, flatFileListenerCreationData.EventTextFormatter, flatFileListenerCreationData.IsAsync);
        }
    }
}
