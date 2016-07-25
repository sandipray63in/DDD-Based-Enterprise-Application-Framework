using System.Data.Common;
using System.Linq;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Effort.Provider;
using FluentAssertions;
using FluentRepoNamespace = FluentRepository;
using Repository;
using Repository.Base;
using Repository.Command;
using Repository.UnitOfWork;
using Testing.Respository;
using TestEFDomainAndContext;
using TestEFDomainAndContext.TestDomains;

namespace Testing.FluentRepository
{
    /// <summary>
    /// The fluent way of writing the code is much more intuitive and readable, compared to 
    /// the normal Repository way of doing things as shown in the test cases written in 
    /// Testing.Repository project library.Also, disposal of all the 
    /// IDisposables(UnitOfWork, repositories etc) are taken care automatically on calling 
    /// the Execute method if the paremeter of Execute method viz. shouldAutomaticallyDisposeAllDisposables 
    /// is set to true.
    /// </summary>
    [TestClass]
    public class FluentEFRespositoryAndUnitOfWorkTest : BaseEFRespositoryAndUnitOfWorkTest
    {
        private DbConnection _connection;

        [TestMethod]
        [TestCategory("Medium")]
        public void test_fluent_insert_single_department_without_any_explicit_transaction_scope_should_be_saved()
        {
            ///Arrange
            var departmentCommandRepository = GetCommandRepositoryInstance<Department>();
            var departmentQueryableRepository = GetQueryableRepositoryInstance<Department>();
            var departmentFake = GetDepartmentFake();
            Department departmentAfterDataInsert = null;

            ///Action
            FluentRepoNamespace.FluentRepository
                               .SetUpCommandRepository(departmentCommandRepository)
                               .Insert(departmentFake)
                               .SetUpQueryRepository(departmentQueryableRepository)
                               .Query<Department>(x => x.Single(), x => departmentAfterDataInsert = x)
                               .Execute(true);

            ///Assert
            departmentAfterDataInsert.DepartmentName.Should().Be("Election");
        }

        #region Overrides

        protected override void RegisterEFTestContext()
        {
            EffortProviderConfiguration.RegisterProvider();
            /// create a new DbConnection using Effort(at runtime, the type of the object created is EffortConnection)
            _connection = Effort.DbConnectionFactory.CreateTransient();
            _container.RegisterType<EFTestContext>(new InjectionConstructor(_connection));
        }

        protected override void RegisterDepartmentCommandService()
        {
            var name = typeof(Department).Name + SERVICE_SUFFIX;
            _container.RegisterType<ICommand<Department>, DepartmentTestServiceCommand>(name, new InjectionConstructor(_connection));
            var context = _container.Resolve<EFTestContext>();
            var command = _container.Resolve<ICommand<Department>>(name, new ParameterOverride("dbContext", context));
            var injectionConstructor = new InjectionConstructor(typeof(BaseUnitOfWork), command);
            _container.RegisterType<ICommandRepository<Department>, CommandRepository<Department>>(name, injectionConstructor);
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            _connection.Dispose();
        }

        #endregion
    }
}
