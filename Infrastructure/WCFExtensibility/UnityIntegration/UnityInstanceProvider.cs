using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.Unity;
using Infrastructure.Extensions;
using Infrastructure.Utilities;

namespace Infrastructure.WCFExtensibility.UnityIntegration
{
    public class UnityInstanceProvider : IInstanceProvider
    {
        private const int MAX_DEPTH_TO_CHECK_FOR_BASE_TYPE = 10;

        private IUnityContainer _container;
        private Type _serviceType;
        private string _serviceName;

        public UnityInstanceProvider(IUnityContainer container, Type type)
        {
            ContractUtility.Requires<ArgumentNullException>(container.IsNotNull(), "container cannot be null.");
            ContractUtility.Requires<ArgumentNullException>(type.IsNotNull(), "type cannot be null.");
            _container = container;
            _serviceType = type;
        }

        public UnityInstanceProvider(IUnityContainer container,Type type,string serviceName) : this(container,type)
        {
            ContractUtility.Requires<ArgumentNullException>(serviceName.IsNullOrWhiteSpace(), "name cannot be null or empty");
            _serviceName = serviceName;
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            if (_serviceName.IsNullOrWhiteSpace())
            {
                return _container.Resolve(_serviceType);
            }
            else
            {
                return _container.Resolve(_serviceType, _serviceName);
            }
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            var instanceType = instance.GetType();
            if (_container.IsTypeOrSomeBaseTypeRegistered(instance, _serviceName, MAX_DEPTH_TO_CHECK_FOR_BASE_TYPE))
            {
                _container.Teardown(instance);
            }
        }
    }
}
