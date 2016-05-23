using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.Unity;
using Infrastructure.DI;

namespace Infrastructure.WCFExtensibility.UnityIntegration
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class UnityServiceBehavior : Attribute, IServiceBehavior
    {
        private const string METADATA_EXCHANGE_CONTRACT_NAME = "IMetadataExchange";
        private const string HTTP_GET_HELP_PAGE_AND_METADATA_CONTRACT = "IHttpGetHelpPageAndMetadataContract";
        private static IUnityContainer _container;

        static UnityServiceBehavior()
        {
            _container = Container.Instance;
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            //Do Nothing
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            //Do Nothing
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            serviceHostBase.ChannelDispatchers.ForEach(channelDispatcherBase =>
                {
                    var serviceType = serviceDescription.ServiceType;
                    var channelDispatcher = channelDispatcherBase as ChannelDispatcher;
                    channelDispatcher.Endpoints.ForEach(endpointDispatcher =>
                        {
                            var contractName = endpointDispatcher.ContractName;
                            if (contractName != METADATA_EXCHANGE_CONTRACT_NAME && contractName != HTTP_GET_HELP_PAGE_AND_METADATA_CONTRACT)
                            {
                                ////TODO - Need to come up of a way to pass the type name only when 
                                //// the name is registered with Unity DI Container.
                                endpointDispatcher.DispatchRuntime.InstanceProvider = new UnityInstanceProvider(_container, serviceType);
                            }
                        }
                    );
                }
            );
        }
    }
}
