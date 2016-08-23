using System;
using System.Transactions;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Domain.Base.Aggregates;
using Domain.Base.Entities;
using Repository;
using Repository.Base;
using Repository.Command;
using Repository.Queryable;
using Infrastructure.UnitOfWork;
using UnitOfWork = Infrastructure.UnitOfWork;
using Infrastructure.DI;
using TestEFDomainAndContext;
using TestEFDomainAndContext.TestDomains;

namespace Testing.Respository
{
    [TestClass]
    public abstract class BaseEFRespositoryAndUnitOfWorkTest
    {
        #region Protected Constants(for Unity Name Resolution)

        protected const string REPOSITORY_WITHOUT_UNIT_OF_WORK = "RepositoryWithoutUnitOfWork";
        protected const string REPOSITORY_WITH_UNIT_OF_WORK = "RepositoryWithUnitOfWork";
        protected const string WITH_EXCEPTION_TO_BE_THROWN_FOR_ROLLBACK_TEST_SUFFIX = "WithExceptionToBeThrownForRollbackTest";
        protected const string SERVICE_SUFFIX = "Service";

        #endregion

        #region Protected Fields

        protected IUnityContainer _container;

        #endregion

        #region SetUp

        /// <summary>
        /// Ideally, in production code all the unity container registrations should be done via config file.
        /// </summary>
        [TestInitialize]
        public void SetupTest()
        {
            _container = Container.Instance;
            RegisterEFTestContext();

            RegisterCommandRepository<Employee>();
            RegisterCommandRepository<Employee>(true);
            RegisterCommandRepository<Department>();
            RegisterCommandRepository<Department>(true);

            RegisterQueryableRepository<Employee>();
            RegisterQueryableRepository<Department>();

            RegisterUnitOfWorkInstance();
            RegisterUnitOfWorkInstance(true);

            RegisterDepartmentCommandService();
        }

        #endregion

        #region Clean Up

        [TestCleanup]
        public void CleanupTest()
        {
            //Dispose everything related to MS Unity DI Container
            _container.Dispose();
            Cleanup();
        }

        #endregion

        #region Private Methods

        //The solution to handle multiple overloaded constructors using unity is available at
        // http://stackoverflow.com/questions/4059991/microsoft-unity-how-to-specify-a-certain-parameter-in-constructor

        private void RegisterCommandRepository<TEntity>(bool isUnitOfWorkRequired = false) where TEntity : BaseEntity<int>, ICommandAggregateRoot
        {
            _container.RegisterType<ICommand<TEntity>, EntityFrameworkCodeFirstCommand<int,TEntity>>();
            var context = _container.Resolve<EFTestContext>();
            var command = _container.Resolve<ICommand<TEntity>>(new ParameterOverride("dbContext", context));
            var respositoryName = isUnitOfWorkRequired ? REPOSITORY_WITH_UNIT_OF_WORK : REPOSITORY_WITHOUT_UNIT_OF_WORK;
            var injectionConstructor = !isUnitOfWorkRequired ? new InjectionConstructor(command) : new InjectionConstructor(typeof(IUnitOfWork), command);
            _container.RegisterType<ICommandRepository<TEntity>, CommandRepository<TEntity>>(respositoryName, injectionConstructor);
        }

        private void RegisterQueryableRepository<TEntity>() where TEntity : class, IQueryableAggregateRoot
        {
            _container.RegisterType<IQuery<TEntity>, EntityFrameworkCodeFirstQueryable<TEntity>>();
            var context = _container.Resolve<EFTestContext>();
            var query = _container.Resolve<IQuery<TEntity>>(new ParameterOverride("dbContext", context));
            var injectionConstructor = new InjectionConstructor(query);
            _container.RegisterType<IQueryableRepository<TEntity>, QueryableRepository<TEntity>>(injectionConstructor);
        }

        private void RegisterUnitOfWorkInstance(bool isExceptionToBeThrownForRollBackTesting = false)
        {
            var toBeResolvedName = typeof(UnitOfWork.UnitOfWork).Name;
            if (!isExceptionToBeThrownForRollBackTesting)
            {
                _container.RegisterType<IUnitOfWork, UnitOfWork.UnitOfWork>(toBeResolvedName, new InjectionConstructor(IsolationLevel.ReadCommitted, TransactionScopeOption.RequiresNew));
            }
            else
            {
                toBeResolvedName += WITH_EXCEPTION_TO_BE_THROWN_FOR_ROLLBACK_TEST_SUFFIX;
                Func<bool> actionToThrowException = () => { throw new Exception("Rollback Test Exception"); };
                _container.RegisterType<IUnitOfWork, UnitOfWork.UnitOfWork>(toBeResolvedName, new InjectionConstructor(actionToThrowException,false, IsolationLevel.ReadCommitted, TransactionScopeOption.RequiresNew));
            }
        }

        #endregion

        #region Protected Members

        protected virtual void RegisterEFTestContext() { }

        protected virtual void RegisterDepartmentCommandService() { }

        protected virtual void Cleanup() { }

        protected ICommandRepository<TEntity> GetCommandRepositoryInstance<TEntity>(IUnitOfWork unitOfWork = null) where TEntity : ICommandAggregateRoot
        {
            var respositoryName = unitOfWork.IsNotNull() ? REPOSITORY_WITH_UNIT_OF_WORK : REPOSITORY_WITHOUT_UNIT_OF_WORK;
            var repository = unitOfWork.IsNull() ? _container.Resolve<ICommandRepository<TEntity>>(respositoryName) : _container.Resolve<ICommandRepository<TEntity>>(respositoryName, new ParameterOverride("unitOfWork", unitOfWork));
            return repository;
        }

        protected IQueryableRepository<TEntity> GetQueryableRepositoryInstance<TEntity>() where TEntity : IQueryableAggregateRoot
        {
            return _container.Resolve<IQueryableRepository<TEntity>>();
        }

        protected ICommandRepository<Department> GetDepartmentCommandServiceRepositoryInstance(IUnitOfWork unitOfWork = null)
        {
            var name = typeof(Department).Name + SERVICE_SUFFIX;
            var repository = unitOfWork.IsNull() ? _container.Resolve<ICommandRepository<Department>>(name) : _container.Resolve<ICommandRepository<Department>>(name, new ParameterOverride("unitOfWork", unitOfWork));
            return repository;
        }

        protected IUnitOfWork GetUnitOfWorkInstance(bool isExceptionToBeThrownForRollBackTesting = false)
        {
            var toBeResolvedName = typeof(UnitOfWork.UnitOfWork).Name;
            if (isExceptionToBeThrownForRollBackTesting)
            {
                toBeResolvedName += WITH_EXCEPTION_TO_BE_THROWN_FOR_ROLLBACK_TEST_SUFFIX;
            }
            return _container.Resolve<IUnitOfWork>(toBeResolvedName);
        }

        #endregion

        #region Fakes i.e. Test Data

        protected Department GetDepartmentFake(int id = 1)
        {
            return FakeData.GetDepartmentFake(id);
        }

        protected Employee GetEmployeeFake(int id = 1)
        {
            return FakeData.GetEmployeeFake(id);
        }

        #endregion
    }
}
