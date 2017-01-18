using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Infrastructure.DI;
using Infrastructure.Utilities;

namespace Infrastructure.Logging.SemanticLogging.EventListenersHandling.Listeners
{
    /// <summary>
    /// Here Logging is implemented based on Enterprise Library Semantic Logging.
    /// For info on Enterprise Library Semantic Logging visit http://damienbod.com/2013/06/07/enterprise-library-basic-semantic-logging-database/ 
    /// and http://www.shujaat.net/2013/09/semantic-logging-application-block-in.html. 
    /// More articles on SLAB are available at http://damienbod.com/category/net/enterprise-library/slab/
    /// 
    /// Ideally, there should be just one time registration of all the Event Sources with the Event Listeners during application start.
    /// 
    /// Whatever done in this project regarding Semantic Logging is developing a small framework around SLAB in-proc stuffs.One can also opt 
    /// to go via the out of proc route as suggested here - http://www.shujaat.net/2013/07/semantic-out-process-logging-using.html. 
    /// Out of proc seems much more easier(whatever done here - everything is already available in the Windows Service and that also 
    /// for sure in a better and configurable way).Anyways, for the time being keeping the Semantic Logging in-proc stuffs as well.
    /// 
    /// For more info on whether to use out of proc or in proc for SLAB, read - "In-Process or Out-of-Process?" section of 
    /// https://msdn.microsoft.com/en-us/library/dn440729(v=pandp.60).aspx
    /// </summary>
    public class ListenerRegistration
    {
        static ListenerRegistration()
        {
            Bootstrapper.Bootstrap();
        }

        public void RegisterListenerDataWithListener(ListenerData listenerData)
        {
            ContractUtility.Requires<ArgumentNullException>(listenerData.ListenerCreationDataAndEventSources.IsNotNull(), "listenerData.ListenerCreationDataAndEventSources instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(listenerData.ListenerCreationDataAndEventSources.ListenerCreationData.IsNotNull(), "The " + 
                "listenerData.ListenerCreationDataAndEventSources.ListenerCreationData instance cannot be null");
            if (Bootstrapper.CheckForPermissibleBootstrapedListenerType(listenerData))
            {
                var listener = Container.Instance.Resolve<IListener>(listenerData.ListenerType.ToString());
                listener.RegisterListener(listenerData.ListenerCreationDataAndEventSources);
            }
        }

        public void RegisterListenerDataWithListeners(IList<ListenerData> listenerDataList)
        {
            ContractUtility.Requires<ArgumentNullException>(listenerDataList.IsNotNull(), "listenerDataList instance cannot be null");
            ContractUtility.Requires<ArgumentOutOfRangeException>(listenerDataList.IsNotEmpty(), "listenerDataList must contain atleast 1 Listener Data");

            listenerDataList.Where(x => x.IsNotNull()).ToList().ForEach(x =>
                {
                    RegisterListenerDataWithListener(x);
                }
            );
        }
    }
}
