using System;
using System.ServiceModel;
using Domain.Base.Aggregates;
using Repository.Base;
using Infrastructure.Utilities;
using Infrastructure.WCFExtensibility.UnityIntegration;

namespace TestWebService
{
    /// <summary>
    /// For Generic WCF Service Implementation refer - http://www.codeproject.com/Articles/290148/Pattern-for-Creating-Generic-WCF-Services 
    /// and http://weblogs.asp.net/zareian/how-to-write-a-generic-service-in-wcf. Also it's not of much use to use generic methods
    /// within, as indicated here - http://stackoverflow.com/questions/2906694/wcf-service-generic-methods.
    /// 
    /// Creating an SVC class for each entity type is hectic and so ideally in some production environment,
    /// this should be taken care at the WCF pipeline level itself(using some Reflection/Expression trees mechanism) and
    /// some marker interface(if required), so that, even if tomorrow some new Entity gets introduced, 
    /// no change(or minimal changes) will be required for that change i.e all these will lead to better maintainability.
    /// 
    /// At the client end also, some Factory based on TEntity type should be used ideally.
    /// 
    /// N.B. ->  WCF OData Services is another viable option.
    ///
    /// Again, there should be a Service Registry for Services Management, as suggested here -
    /// http://www.infoq.com/articles/net-service-registry but this being just a Test Service, not 
    /// being developed via that route.Another related article that can be helpful in this area is -
    /// http://ftp.wso2.org/~workshops/2011/10/20/WSO2-Registry-Atos-Final.pdf. A sample project 
    /// dealing with all these in a much larger scale is .NET Stock Trader(especially, Configuration 
    /// Manager related stuffs) which is available at - https://msdn.microsoft.com/en-us/vstudio/bb499684.aspx
    /// One fork of .NET Stock Trader Application is the Apache Stonehenge Stock Trader project 
    /// available at - https://cwiki.apache.org/confluence/display/STONEHENGE/Stonehenge+StockTrader+Sample+Application
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    [UnityServiceBehavior]
    public class TestWebService<TEntity> : ITestWebService<TEntity> where TEntity : class, ICommandAggregateRoot
    {
        private ICommandRepository<TEntity> _commandRepository;

        public TestWebService(ICommandRepository<TEntity> commandRepository)
        {
            _commandRepository = commandRepository;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void Insert(TEntity item)
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            _commandRepository.Insert(item);
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void Update(TEntity item)
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            _commandRepository.Update(item);
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void Delete(TEntity item)
        {
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            _commandRepository.Delete(item);
        }

        //
        //Similarly other repository methods can be added here
        //
    }
}