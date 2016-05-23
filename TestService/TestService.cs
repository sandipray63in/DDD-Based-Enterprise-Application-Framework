using System;
using System.Data.Common;
using System.ServiceModel;
using Microsoft.Practices.Unity;
using Domain.Base;
using Repository;
using Repository.Base;
using Repository.Command;
using Infrastructure.DI;
using Infrastructure.Utilities;
using TestEFDomainAndContext;

namespace TestService
{
    /// <summary>
    /// This Service was developed to be used alongwith Effort Framework but EffortConnection related 
    /// DBContext doesn't persist data across cross App Domains.Was needed to test Unit Of Work 
    /// explicit Transaction handling.Anyways, still keeping it.
    /// 
    /// For Generic WCF Service Implementation refer - http://www.codeproject.com/Articles/290148/Pattern-for-Creating-Generic-WCF-Services 
    /// and http://weblogs.asp.net/zareian/how-to-write-a-generic-service-in-wcf. Also it's not of much use to use generic methods
    /// within, as indicated here - http://stackoverflow.com/questions/2906694/wcf-service-generic-methods.
    /// 
    /// Creating an SVC class for each entity type is hectic and so ideally in some production environment,
    /// this should be taken care at the WCF pipeline level itself(using some Reflection/Expression trees mechanism) and
    /// some marker interface(if required), so that even if tomorrow some new Entity gets introduced, 
    /// no change(or minimal changes) will be required for that change i.e all these will lead to better maintainability.
    /// 
    /// At the client end also, some Factory based on TEntity type should be used ideally.
    /// 
    ///N.B. ->  WCF OData Services is another viable option.
    /// 
    /// Also, in production environment, one should use Unity Registrations via config using UnityBehaviourExtensionElement
    /// (present in the Infrastructure Library) or atleast using some Custom WCF ServiceHostFactory(just like done for
    /// TestServiceBasedOnSQLCE).
    /// 
    /// Again, there should be a Service Registry for Services Management, as suggested here -
    /// http://www.infoq.com/articles/net-service-registry but this being just a Test Service, not 
    /// being developed via that route.Another related article that can be helpful in this area is -
    /// http://ftp.wso2.org/~workshops/2011/10/20/WSO2-Registry-Atos-Final.pdf. A sample project 
    /// dealing with all these in a much larger scale is .NET Stock Trader(especially, Configuration 
    /// Manager related stuffs) which is available at - https://msdn.microsoft.com/en-us/vstudio/bb499684.aspx
    /// One fork of .NET Stock Trader Application is the Apache Stonehenge project available here -
    /// https://cwiki.apache.org/confluence/display/STONEHENGE/Stonehenge+StockTrader+Sample+Application
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class TestService<TEntity> : ITestService<TEntity> where TEntity : class, ICommandAggregateRoot
    {
        private const string EFFORT_CONNECTION_BASED_EF_TEST_CONEXT = "EffortConnectionBasedEFTestContext";

        public TestService()
        {
            var i = 0;
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void Insert(DbConnection dbConnection, TEntity item)
        {
            ContractUtility.Requires<ArgumentNullException>(dbConnection.IsNotNull(), "dbConnection instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            InvokeViaContainerSetUp(dbConnection, x => x.Insert(item));
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void Update(DbConnection dbConnection, TEntity item)
        {
            ContractUtility.Requires<ArgumentNullException>(dbConnection.IsNotNull(), "dbConnection instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            InvokeViaContainerSetUp(dbConnection, x => x.Update(item));
        }

        [OperationBehavior(TransactionScopeRequired = true)]
        public void Delete(DbConnection dbConnection, TEntity item)
        {
            ContractUtility.Requires<ArgumentNullException>(dbConnection.IsNotNull(), "dbConnection instance cannot be null");
            ContractUtility.Requires<ArgumentNullException>(item.IsNotNull(), "item cannot be null");
            InvokeViaContainerSetUp(dbConnection, x => x.Delete(item));
        }

        //
        //Similarly other repository methods can be added here
        //

        private void InvokeViaContainerSetUp(DbConnection dbConnection, Action<ICommandRepository<TEntity>> action)
        {
            using (var container = Container.Instance)
            {
                RegisterCommandRepository(container,dbConnection);
                using (var commandRepository = container.Resolve<ICommandRepository<TEntity>>(EFFORT_CONNECTION_BASED_EF_TEST_CONEXT))
                {
                    action(commandRepository);
                }
            }
        }

        private void RegisterCommandRepository(IUnityContainer container, DbConnection dbConnection) 
        {
            container.RegisterType<EFTestContext>(EFFORT_CONNECTION_BASED_EF_TEST_CONEXT,new InjectionConstructor(dbConnection));
            container.RegisterType<ICommand<TEntity>, EntityFrameworkCodeFirstCommand<TEntity>>(EFFORT_CONNECTION_BASED_EF_TEST_CONEXT);
            var context = container.Resolve<EFTestContext>(EFFORT_CONNECTION_BASED_EF_TEST_CONEXT);
            var command = container.Resolve<ICommand<TEntity>>(EFFORT_CONNECTION_BASED_EF_TEST_CONEXT, new ParameterOverride("dbContext", context));
            container.RegisterType<ICommandRepository<TEntity>, CommandRepository<TEntity>>(EFFORT_CONNECTION_BASED_EF_TEST_CONEXT, new InjectionConstructor(command));
        }
    }
}