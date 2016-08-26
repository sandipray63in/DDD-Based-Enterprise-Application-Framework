using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using FluentRepoNamespace = FluentRepository;
using TestEFDomainAndContext;
using TestEFDomainAndContext.TestDomains;
using Testing.Respository;

namespace Testing.Integration
{
    /// <summary>
    /// Faced a weird issue(while development), so just keeping a note of it.The issue and the fix is available at -
    /// http://stackoverflow.com/questions/2155930/fixing-the-breakpoint-will-not-currently-be-hit-no-symbols-have-been-loaded-fo .
    /// Also SQL Server doesn't allow transactions while creating and deleting DBs as indicated here - 
    /// http://stackoverflow.com/questions/15168616/create-database-statement-not-allowed-within-multi-statement-transaction.
    /// If noticed properly, the async versions of FluentRepository Tests doesn't require async-await stuffs to be incorporated 
    /// within coded Test cases since internally everything is going on in-memory which is similar to running things 
    /// synchronously(atleast using Effort Framework for Entity Framework).But when actually doing an IO operation like DB interaction,
    /// it's better to use async-await from the start within the test cases itself else one might invite unnecessary weird exception 
    /// scenarios.
    /// </summary>
    [TestClass]
    public class FluentSQLRespositoryAndUnitOfWorkTest : BaseEFRespositoryAndUnitOfWorkTest
    {
        /// <summary>
        /// Since tested locally, MSDTC need not be enabled.
        /// </summary>
        [TestMethod]
        [TestCategory("Medium")]
        public async Task test_sql_fluent_insert_multiple_employees_alongwith_department_with_explicit_transaction_scope_should_be_saved()
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

            var dummyDepartmentFakeDataForFirstTimeDBGeneration = FakeData.GetDepartmentFake(111111);

            //Action
            /// Order of operations of different instances of same type or different types needs to be handled at 
            /// the Business or Service Layer.

            /// The below code is there just to generate the DB using EF context when this current thread runs for the first time. 
            /// This is required since it's not allowed to create a DB within Transactions. 
            /// Seeding of data also becomes part of the transaction and so cannot ultimately create the DB and so seeding the data 
            /// doesn't serve the purpose.
            FluentRepoNamespace.FluentRepository
                               .SetUpCommandRepository(departmentCommandRepository)
                               .Insert(dummyDepartmentFakeDataForFirstTimeDBGeneration)
                               .Delete(dummyDepartmentFakeDataForFirstTimeDBGeneration)
                               .Execute();// here not setting shouldAutomaticallyDisposeAllDisposables = true since that would
                                          // then dispose the departmentCommandRepository which will be required in the next operation

            await FluentRepoNamespace.FluentRepository
                                    .WithDefaultUnitOfWork()
                                    .SetUpCommandRepository(employeeCommandRepository, departmentCommandRepository)
                                    .InsertAsync<Employee>(new List<Employee> { managerEmployeeFake, subEmployeeFake })
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
        public async Task test_sql_fluent_insert_multiple_employees_alongwith_department_with_explicit_transaction_scope_with_exception_thrown_in_between_should_rollback()
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

            var dummyDepartmentFakeDataForFirstTimeDBGeneration = FakeData.GetDepartmentFake(111111);

            //Action
            /// Order of operations of different instances of same type or different types needs to be handled at 
            /// the Business or Service Layer.

            /// The below code is there just to generate the DB using EF context when this current thread runs for the first time. 
            /// This is required since it's not allowed to create a DB within Transactions.
            /// Seeding of data also becomes part of the transaction and so cannot ultimately create the DB and so seeding the data 
            /// doesn't serve the purpose.
            FluentRepoNamespace.FluentRepository
                               .SetUpCommandRepository(departmentCommandRepository)
                               .Insert(dummyDepartmentFakeDataForFirstTimeDBGeneration)
                               .Delete(dummyDepartmentFakeDataForFirstTimeDBGeneration)
                               .Execute();// here not setting shouldAutomaticallyDisposeAllDisposables = true since that would
                                          // then dispose the departmentCommandRepository which will be required in the next operation

            await FluentRepoNamespace.FluentRepository
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

        [TestMethod]
        [TestCategory("Slow")]
        public async Task test_sql_fluent_insert_multiple_employees_alongwith_department_service_with_explicit_transaction_scope_should_save_data()
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

            var dummyDepartmentFakeDataForFirstTimeDBGeneration = FakeData.GetDepartmentFake(111113);

            //Action
            /// Order of operations of different instances of same type or different types needs to be handled at 
            /// the Business or Service Layer.

            /// The below code is there just to generate the DB using EF context when this current thread runs for the first time. 
            /// This is required since it's not allowed to create a DB within Transactions.
            /// Seeding of data also becomes part of the transaction and so cannot ultimately create the DB and so seeding the data 
            /// doesn't serve the purpose.
            FluentRepoNamespace.FluentRepository
                               .SetUpCommandRepository(departmentCommandRepository)
                               .Insert(dummyDepartmentFakeDataForFirstTimeDBGeneration)
                               .Delete(dummyDepartmentFakeDataForFirstTimeDBGeneration)
                               .Execute();// here not setting shouldAutomaticallyDisposeAllDisposables = true since that would
                                          // then dispose the departmentCommandRepository which will be required in the next operation

            await FluentRepoNamespace.FluentRepository
                                     .WithDefaultUnitOfWork()
                                     .SetUpCommandRepository(employeeCommandRepository, departmentCommandRepository)
                                     .InsertAsync<Employee>(new List<Employee> { managerEmployeeFake, subEmployeeFake })
                                     .InsertAsync(departmentFake2)
                                     .SetUpQueryRepository(departmentQueryableRepository)
                                     .Query<Department>(x => x, x => departmentsCount = x.Count())
                                     .ExecuteAsync();

            //Assert
            departmentsCount.Should().Be(2);
        }
    }
}
