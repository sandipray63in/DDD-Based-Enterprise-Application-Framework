using System;
using System.Diagnostics.Tracing;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Infrastructure.ExceptionHandling.SemanticLogging.EventListenersHandling.ListenerCreationData;
using Infrastructure.Utilities;

namespace Infrastructure.ExceptionHandling.SemanticLogging.EventListenersHandling.Listeners
{
    internal class ConsoleListener : BaseListener
    {
        protected override EventListener GetListener(IListenerCreationData listenerCreationData)
        {
            ContractUtility.Requires<ArgumentNullException>(listenerCreationData.IsNotNull(), "Listener Creation Data cannot be null");
            var consoleListenerCreationData = listenerCreationData as ConsoleListenerCreationData;
            return ConsoleLog.CreateListener(consoleListenerCreationData.EventTextFormatter, consoleListenerCreationData.ConsoleColorMapper);
        }
    }
}
