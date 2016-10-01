using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Effort.Provider;
using FluentAssertions;
using FluentRepoNamespace = FluentRepository;
using Testing.Respository;
using TestEFDomainAndContext;
using TestEFDomainAndContext.TestDomains;
using Testing.Repository;

namespace Testing.FluentRepository
{
    /// <summary>
    /// The fluent way of writing the code is much more intuitive and readable, compared to 
    /// the normal Repository way of doing things as shown in the test cases written in 
    /// Testing.Repository project library.Also, disposal of all the 
    /// IDisposables(UnitOfWork, Repositories etc) are taken care automatically on calling 
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
            var departmentFake = FakeData.GetDepartmentFake();
            Department departmentAfterDataInsert = null;

            ///Action
            FluentRepoNamespace.FluentRepository
                               .SetUpCommandRepository(departmentCommandRepository)
                               .InsertAsync(departmentFake)
                               .SetUpQueryRepository(departmentQueryableRepository)
                               .Query<Department>(x => x.Single(), x => departmentAfterDataInsert = x)
                               .ExecuteAsync(shouldAutomaticallyDisposeAllDisposables:true);

            ///Assert
            departmentAfterDataInsert.DepartmentName.Should().Be("Election");
        }

        /// <summary>
        /// Cascading inserts needs to be taken care at the Business or Service layer (as per the defined architecture)
        /// in production code.
        /// </summary>
        [TestMethod]
        [TestCategory("Medium")]
        public void test_fluent_insert_multiple_employees_without_any_explicit_transaction_scope_should_be_saved()
        {
            //Arrange
            var departmentCommandRepository = GetCommandRepositoryInstance<Department>();
            var employeeCommandRepository = GetCommandRepositoryInstance<Employee>();
            var departmentQueryableRepository = GetQueryableRepositoryInstance<Department>();
            var employeeQueryableRepository = GetQueryableRepositoryInstance<Employee>();
            var departmentFake = FakeData.GetDepartmentFake();

            var managerEmployeeFake = FakeData.GetEmployeeFake();
            managerEmployeeFake.EmployeeName = "XYZ";
            managerEmployeeFake.DeptID = departmentFake.Id;
            managerEmployeeFake.Department = departmentFake;

            var subEmployeeFake = FakeData.GetEmployeeFake(2);
            subEmployeeFake.DeptID = departmentFake.Id;
            subEmployeeFake.Department = departmentFake;
            subEmployeeFake.ManagerId = managerEmployeeFake.Id;
            subEmployeeFake.Manager = managerEmployeeFake;

            var departmentsCount = 0;
            var employeesCount = 0;
            var firstEmployeeName = string.Empty;
            var employeeSandipID = 0;

            //Action
            //Considering cascading inserts, here the order in which the list is inserted with different employee 
            //objects is important. Since department object is defined as required in the EmployeMap class,
            //Effort(just like EF) automatically inserts the department object before inserting the employee objects.
            //But again in EmployeeMap class, Manager is marked as optional, so manager and the employees under him or her needs to 
            //be explicitly inserted in proper order.Also here, EF doesn't ensure that if managerEmployeeFake gets 
            //saved successfully and then subEmployeeFake doesn't get saved, then all the operations will be 
            //rolled back (at the DB level).

            //And all these(inserting department object first, then manager object and then sub emloyee object), happens 
            // internally within the same transaction (just like EF6) and one doesn't need to use the UnitOfWork class 
            // (available in this project).

            FluentRepoNamespace.FluentRepository
                               .SetUpCommandRepository(employeeCommandRepository)
                               .Insert<Employee>(new List<Employee> { managerEmployeeFake, subEmployeeFake })
                               .SetUpQueryRepository(departmentQueryableRepository, employeeQueryableRepository)
                               .Query<Department>(x => x, x => departmentsCount = x.Count())
                               .Query<Employee>(x => x, x =>
                               {
                                   employeesCount = x.Count();
                                   firstEmployeeName = x.First().EmployeeName;
                                   employeeSandipID = x.Single(y => y.EmployeeName == "Sandip").Id;
                               })
                               .Execute(true);

            //Assert
            departmentsCount.Should().Be(1);
            employeesCount.Should().Be(2);
            firstEmployeeName.Should().Be("XYZ");
            employeeSandipID.Should().Be(2);

        }

        [TestMethod]
        [TestCategory("Medium")]
        public void test_fluent_delete_of_department_should_delete_the_underlying_employee()
        {
            //Arrange
            var departmentCommandRepository = GetCommandRepositoryInstance<Department>();
            var employeeCommandRepository = GetCommandRepositoryInstance<Employee>();
            var employeeQueryableRepository = GetQueryableRepositoryInstance<Employee>();
            var departmentFake = FakeData.GetDepartmentFake();

            var employeeFake = FakeData.GetEmployeeFake();
            employeeFake.EmployeeName = "XYZ";
            employeeFake.DeptID = departmentFake.Id;
            employeeFake.Department = departmentFake;

            var employeesCount = 0;

            //Action
            //Employee insert will automatically insert the department object before inserting this employee object 
            //since department object is marked as required in the employee map.
            // Should delete the employee object automatically since the map is defined so 
            // (WillCascadeOnDelete is set to true).
            FluentRepoNamespace.FluentRepository
                               .SetUpCommandRepository(employeeCommandRepository, departmentCommandRepository)
                               .Insert(employeeFake)
                               .DeleteAsync(departmentFake)
                               .SetUpQueryRepository(employeeQueryableRepository)
                               .Query<Employee>(x => x, x => employeesCount = x.Count())
                               .ExecuteAsync(shouldAutomaticallyDisposeAllDisposables: true);

            //Assert
            employeesCount.Should().Be(0);
        }

        /// <summary>
        /// Since tested locally, MSDTC need not be enabled.
        /// </summary>
        [TestMethod]
        [TestCategory("Medium")]
        public void test_fluent_insert_multiple_employees_alongwith_department_with_explicit_transaction_scope_should_be_saved()
        {
            //Arrange
            var departmentCommandRepository = GetCommandRepositoryInstance<Department>();
            var employeeCommandRepository = GetCommandRepositoryInstance<Employee>();
            var departmentQueryableRepository = GetQueryableRepositoryInstance<Department>();
            var employeeQueryableRepository = GetQueryableRepositoryInstance<Employee>();
            var departmentFake = FakeData.GetDepartmentFake();
            var departmentFake2 = FakeData.GetDepartmentFake(2);

            var managerEmployeeFake = FakeData.GetEmployeeFake();
            managerEmployeeFake.EmployeeName = "XYZ";
            managerEmployeeFake.DeptID = departmentFake.Id;
            managerEmployeeFake.Department = departmentFake;

            var subEmployeeFake = FakeData.GetEmployeeFake(2);
            subEmployeeFake.DeptID = departmentFake.Id;
            subEmployeeFake.Department = departmentFake;
            subEmployeeFake.ManagerId = managerEmployeeFake.Id;
            subEmployeeFake.Manager = managerEmployeeFake;

            var departmentsCount = 0;
            var employeesCount = 0;

            //Action
            /// Order of operations of different instances of same type or different types needs to be handled at 
            /// the Business or Service Layer.
            FluentRepoNamespace.FluentRepository
                               .WithDefaultUnitOfWork()
                               .SetUpCommandRepository(employeeCommandRepository, departmentCommandRepository)
                               .Insert<Employee>(new List<Employee> { managerEmployeeFake, subEmployeeFake })
                               .InsertAsync(departmentFake2)
                               .SetUpQueryRepository(departmentQueryableRepository, employeeQueryableRepository)
                               .Query<Department>(x => x, x => departmentsCount = x.Count())
                               .Query<Employee>(x => x, x => employeesCount = x.Count())
                               .ExecuteAsync(shouldAutomaticallyDisposeAllDisposables: true);

            //Assert
            departmentsCount.Should().Be(2);
            employeesCount.Should().Be(2);
        }

        /// <summary>
        /// Since tested locally, MSDTC need not be enabled.
        /// </summary>
        [TestMethod]
        [TestCategory("Medium")]
        public void test_fluent_insert_multiple_employees_alongwith_department_with_explicit_transaction_scope_with_exception_thrown_in_between_should_rollback()
        {
            //Arrange
            var unitOfWorkWithExceptionToBeThrown = GetUnitOfWorkInstance(true);
            var departmentCommandRepository = GetCommandRepositoryInstance<Department>();
            var employeeCommandRepository = GetCommandRepositoryInstance<Employee>();
            var departmentQueryableRepository = GetQueryableRepositoryInstance<Department>();
            var employeeQueryableRepository = GetQueryableRepositoryInstance<Employee>();
            var departmentFake = FakeData.GetDepartmentFake();
            var departmentFake2 = FakeData.GetDepartmentFake(2);

            var managerEmployeeFake = FakeData.GetEmployeeFake();
            managerEmployeeFake.EmployeeName = "XYZ";
            managerEmployeeFake.DeptID = departmentFake.Id;
            managerEmployeeFake.Department = departmentFake;

            var subEmployeeFake = FakeData.GetEmployeeFake(2);
            subEmployeeFake.DeptID = departmentFake.Id;
            subEmployeeFake.Department = departmentFake;
            subEmployeeFake.ManagerId = managerEmployeeFake.Id;
            subEmployeeFake.Manager = managerEmployeeFake;

            var departmentsCount = 0;
            var employeesCount = 0;

            //Action
            /// Order of operations of different instances of same type or different types needs to be handled at 
            /// the Business or Service Layer.

            FluentRepoNamespace.FluentRepository
                               .WithUnitOfWork(unitOfWorkWithExceptionToBeThrown)
                               .SetUpCommandRepository(employeeCommandRepository, departmentCommandRepository)
                               .InsertAsync<Employee>(new List<Employee> { managerEmployeeFake, subEmployeeFake })
                               .Insert(departmentFake2)
                               .SetUpQueryRepository(departmentQueryableRepository, employeeQueryableRepository)
                               .Query<Department>(x => x, x => departmentsCount = x.Count())
                               .Query<Employee>(x => x, x => employeesCount = x.Count())
                               .ExecuteAsync(shouldAutomaticallyDisposeAllDisposables: true);

            //Assert
            departmentsCount.Should().Be(0);
            employeesCount.Should().Be(0);
        }

        #region Overrides

        protected override void RegisterEFTestContext()
        {
            EffortProviderConfiguration.RegisterProvider();
            /// create a new DbConnection using Effort(at runtime, the type of the object created is EffortConnection)
            _connection = Effort.DbConnectionFactory.CreateTransient();
            _container.RegisterType<EFTestContext>(new InjectionConstructor(_connection));
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            _connection.Dispose();
        }

        #endregion
    }
}
