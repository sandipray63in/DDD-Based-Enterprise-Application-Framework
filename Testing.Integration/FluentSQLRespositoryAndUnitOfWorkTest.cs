using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using FluentAssertions;
using FluentRepoNamespace = FluentRepository;
using TestEFDomainAndContext.TestDomains;
using Testing.Respository;

namespace Testing.Integration
{
    /// <summary>
    /// Faced a weird issue(while development), so just keeping a note of it.The issue and the fix is available at -
    /// http://stackoverflow.com/questions/2155930/fixing-the-breakpoint-will-not-currently-be-hit-no-symbols-have-been-loaded-fo .
    /// Also SQL Server doesn't allow transactions while creating and deleting DBs as indicated here - 
    /// http://stackoverflow.com/questions/15168616/create-database-statement-not-allowed-within-multi-statement-transaction
    /// </summary>
    [TestClass]
    public class FluentSQLRespositoryAndUnitOfWorkTest : BaseEFRespositoryAndUnitOfWorkTest
    {
        private DbConnection _connection;

        /// <summary>
        /// Since tested locally, MSDTC need not be enabled.
        /// </summary>
        [TestMethod]
        [TestCategory("Medium")]
        public void test_sql_fluent_insert_multiple_employees_alongwith_department_with_explicit_transaction_scope_should_be_saved()
        {
            //Arrange
            var departmentCommandRepository = GetCommandRepositoryInstance<Department>();
            var employeeCommandRepository = GetCommandRepositoryInstance<Employee>();
            var departmentQueryableRepository = GetQueryableRepositoryInstance<Department>();
            var employeeQueryableRepository = GetQueryableRepositoryInstance<Employee>();
            var departmentFake = GetDepartmentFake();
            var departmentFake2 = GetDepartmentFake(2);

            var managerEmployeeFake = GetEmployeeFake();
            managerEmployeeFake.EmployeeName = "XYZ";
            managerEmployeeFake.DeptID = departmentFake.Id;
            managerEmployeeFake.Department = departmentFake;

            var subEmployeeFake = GetEmployeeFake(2);
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
        public void test_sql_fluent_insert_multiple_employees_alongwith_department_with_explicit_transaction_scope_with_exception_thrown_in_between_should_rollback()
        {
            //Arrange
            var unitOfWorkWithExceptionToBeThrown = GetUnitOfWorkInstance(true);
            var departmentCommandRepository = GetCommandRepositoryInstance<Department>();
            var employeeCommandRepository = GetCommandRepositoryInstance<Employee>();
            var departmentQueryableRepository = GetQueryableRepositoryInstance<Department>();
            var employeeQueryableRepository = GetQueryableRepositoryInstance<Employee>();
            var departmentFake = GetDepartmentFake();
            var departmentFake2 = GetDepartmentFake(2);

            var managerEmployeeFake = GetEmployeeFake();
            managerEmployeeFake.EmployeeName = "XYZ";
            managerEmployeeFake.DeptID = departmentFake.Id;
            managerEmployeeFake.Department = departmentFake;

            var subEmployeeFake = GetEmployeeFake(2);
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
            departmentsCount.Should().Be(2);
            employeesCount.Should().Be(2);
        }

        [TestMethod]
        [TestCategory("Slow")]
        public void test_sql_fluent_insert_multiple_employees_alongwith_department_service_with_explicit_transaction_scope_should_save_data()
        {
            //Arrange
            var departmentCommandRepository = GetCommandRepositoryInstance<Department>();
            var employeeCommandRepository = GetCommandRepositoryInstance<Employee>();
            var departmentQueryableRepository = GetQueryableRepositoryInstance<Department>();
            var employeeQueryableRepository = GetQueryableRepositoryInstance<Employee>();
            var departmentFake = GetDepartmentFake();
            var departmentFake2 = GetDepartmentFake(2);

            var managerEmployeeFake = GetEmployeeFake();
            managerEmployeeFake.EmployeeName = "XYZ";
            managerEmployeeFake.DeptID = departmentFake.Id;
            managerEmployeeFake.Department = departmentFake;

            var subEmployeeFake = GetEmployeeFake(2);
            subEmployeeFake.DeptID = departmentFake.Id;
            subEmployeeFake.Department = departmentFake;
            subEmployeeFake.ManagerId = managerEmployeeFake.Id;
            subEmployeeFake.Manager = managerEmployeeFake;

            var departmentsCount = 0;

            //Action
            /// Order of operations of different instances of same type or different types needs to be handled at 
            /// the Business or Service Layer.
            FluentRepoNamespace.FluentRepository
                               .WithDefaultUnitOfWork()
                               .SetUpCommandRepository(employeeCommandRepository, departmentCommandRepository)
                               .Insert<Employee>(new List<Employee> { managerEmployeeFake, subEmployeeFake })
                               .Insert(departmentFake2)
                               .SetUpQueryRepository(departmentQueryableRepository)
                               .Query<Department>(x => x, x => departmentsCount = x.Count())
                               .Execute();

            //Assert
            departmentsCount.Should().Be(2);
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            if (_connection != null)
            {
                _connection.Dispose();
            }
        }
    }
}
