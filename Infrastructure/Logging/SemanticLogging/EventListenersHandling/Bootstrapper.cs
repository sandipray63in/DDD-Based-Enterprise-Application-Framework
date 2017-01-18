using System;
using System.Collections.Immutable;
using Microsoft.Practices.Unity;
using Infrastructure.DI;
using Infrastructure.Logging.SemanticLogging.EventListenersHandling.Listeners;
using Infrastructure.Logging.SemanticLogging.EventListenersHandling.ListenerCreationData;
using Infrastructure.Utilities;

namespace Infrastructure.Logging.SemanticLogging.EventListenersHandling
{
    internal static class Bootstrapper
    {
        private static readonly ImmutableDictionary<ListenerType, Type> _permissibleListenerCreationDataTypeForListenerType;

        static Bootstrapper()
        {
            var _permissibleListenerCreationDataTypeForListenerTypeBuilder = ImmutableDictionary.CreateBuilder<ListenerType, Type>();
            _permissibleListenerCreationDataTypeForListenerTypeBuilder.Add(ListenerType.Console, typeof(ConsoleListenerCreationData));
            _permissibleListenerCreationDataTypeForListenerTypeBuilder.Add(ListenerType.FlatFile, typeof(FlatFileListenerCreationData));
            _permissibleListenerCreationDataTypeForListenerTypeBuilder.Add(ListenerType.RollingFile, typeof(RollingFileListenerCreationData));
            _permissibleListenerCreationDataTypeForListenerType = _permissibleListenerCreationDataTypeForListenerTypeBuilder.ToImmutable();
        }

        internal static void Bootstrap()
        {
            Container.Instance.RegisterType<IListener, ConsoleListener>(ListenerType.Console.ToString());
            Container.Instance.RegisterType<IListener, FlatFileListener>(ListenerType.Console.ToString());
            Container.Instance.RegisterType<IListener, RollingFileListener>(ListenerType.Console.ToString());
        }

        internal static bool CheckForPermissibleBootstrapedListenerType(ListenerData listenerData)
        {
            Type type = null;
            ContractUtility.Requires<ArgumentNullException>(_permissibleListenerCreationDataTypeForListenerType.TryGetValue(listenerData.ListenerType, out type),
                "Immutable Dictionary could not get the value");
            var listenerCreationData = listenerData.ListenerCreationDataAndEventSources.ListenerCreationData;
            ContractUtility.Requires<ArgumentNullException>(listenerCreationData.GetType() == type, 
                string.Format("For Listener Type {0}, the Listener Creation Data must be of type {1}", listenerData.ListenerType.ToString(),
                _permissibleListenerCreationDataTypeForListenerType[listenerData.ListenerType].Name));
            return true;
        }
    }
}
