using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Respository.Testing;
using TestEFDomainAndContext;
using TestEFDomainAndContext.TestDomains;
using FluentAssertions;
using Repository.Command;
using Repository.UnitOfWork;
using Repository.Base;

namespace Repository.Testing
{
    /// <summary>
    /// Using SQL Lite instead of SQL CE since SQL CE has issues related to TransactionScope as 
    /// discussed at  
    /// TODO - provide the link
    /// and
    /// TODO - provide the link
    /// N.B. -> Also both SQL CE and SQL Lite doesn't support DateTimeOffset
    /// 
    /// TODO - Need to install EntityFramework.SQLLite nuget package in this project and 
    /// the WCF Service project(alongwith setting up the connection strings etc) and run the 
    /// below tests.
    /// </summary>
    [TestClass]
    public class SQLLiteEFRespositoryAndUnitOfWorkTest : BaseEFRespositoryAndUnitOfWorkTest
    {
        #region Test Methods

        [TestMethod]
        [TestCategory("Fast")]
        public void test_insert_single_department_without_any_explicit_transaction_scope_should_be_saved()
        {
            //Arrange
            using (var departmentCommandRepository = GetCommandRepositoryInstance<Department>())
            using (var departmentQueryableRepository = GetQueryableRepositoryInstance<Department>())
            {
                //Arrange
                var departmentFake = GetDepartmentFake();

                //Action
                departmentCommandRepository.Insert(departmentFake);

                //Assert
                departmentQueryableRepository.Single().DepartmentName.Should().Be("Election");

                ///Need to delete this record for the next run to run without any issues since 
                ///SQL Lite will persist the data and if the same data is tried to be inserted again 
                ///duplicate key error will be thrown.
                departmentCommandRepository.Delete(departmentFake);
            };
        }

        /// <summary>
        /// Cascading inserts needs to be taken care at the Business or Service layer (as per the defined architecture)
        /// in production code.
        /// </summary>
        [TestMethod]
        [TestCategory("Medium")]
        public void test_insert_multiple_employees_without_any_explicit_transaction_scope_should_be_saved()
        {
            //Arrange
            using (var departmentCommandRepository = GetCommandRepositoryInstance<Department>())
            using (var employeeCommandRepository = GetCommandRepositoryInstance<Employee>())
            using (var departmentQueryableRepository = GetQueryableRepositoryInstance<Department>())
            using (var employeeQueryableRepository = GetQueryableRepositoryInstance<Employee>())
            {
                //Arrange
                var departmentFake = GetDepartmentFake();

                var managerEmployeeFake = GetEmployeeFake();
                managerEmployeeFake.EmployeeName = "XYZ";
                managerEmployeeFake.DeptID = departmentFake.Id;
                managerEmployeeFake.Department = departmentFake;

                var subEmployeeFake = GetEmployeeFake(2);
                subEmployeeFake.DeptID = departmentFake.Id;
                subEmployeeFake.Department = departmentFake;
                subEmployeeFake.ManagerId = managerEmployeeFake.Id;
                subEmployeeFake.Manager = managerEmployeeFake;

                //Action
                employeeCommandRepository.Insert(new List<Employee> { managerEmployeeFake, subEmployeeFake });

                //Assert
                departmentQueryableRepository.Count().Should().Be(1);
                employeeQueryableRepository.Count().Should().Be(2);
                employeeQueryableRepository.First().EmployeeName.Should().Be("XYZ");
                employeeQueryableRepository.Single(x => x.EmployeeName == "Sandip").Id.Should().Be(2);

                ///Need to delete this record for the next run to run without any issues since 
                ///SQL Lite will persist the data and if the same data is tried to be inserted again, 
                ///duplicate key error will be thrown.
                ///Also, since Cascade delete is set to true in EmployeeMap, deleting the department
                ///instance should automatically take care of underlying employee objects.
                departmentCommandRepository.Delete(departmentFake);
            };
        }

        [TestMethod]
        [TestCategory("Medium")]
        public void test_delete_of_department_should_delete_the_underlying_employee()
        {
            //Arrange
            using (var departmentCommandRepository = GetCommandRepositoryInstance<Department>())
            using (var employeeCommandRepository = GetCommandRepositoryInstance<Employee>())
            using (var employeeQueryableRepository = GetQueryableRepositoryInstance<Employee>())
            {
                //Arrange
                var departmentFake = GetDepartmentFake();

                var employeeFake = GetEmployeeFake();
                employeeFake.EmployeeName = "XYZ";
                employeeFake.DeptID = departmentFake.Id;
                employeeFake.Department = departmentFake;

                //Action
                //Employee insert will automatically insert the department object before inserting this employee object 
                //since department object is marked as required in the employee map.
                employeeCommandRepository.Insert(employeeFake);

                // Should delete the employee object automatically since the map is defined so 
                // (WillCascadeOnDelete is set to true).
                departmentCommandRepository.Delete(departmentFake);

                //Assert
                employeeQueryableRepository.Count().Should().Be(0);
            };
        }

        /// <summary>
        /// Since tested locally, MSDTC need not be enabled.
        /// </summary>
        [TestMethod]
        [TestCategory("Medium")]
        public void test_insert_multiple_employees_alongwith_department_with_explicit_transaction_scope_should_be_saved()
        {
            //Arrange
            var unitOfWork = GetUnitOfWorkInstance();
            using (var departmentCommandRepository = GetCommandRepositoryInstance<Department>(unitOfWork))
            using (var employeeCommandRepository = GetCommandRepositoryInstance<Employee>(unitOfWork))
            using (var departmentQueryableRepository = GetQueryableRepositoryInstance<Department>())
            using (var employeeQueryableRepository = GetQueryableRepositoryInstance<Employee>())
            {
                //Arrange
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

                //Action
                /// Order of operations of different instances of same type or different types needs to be handled at 
                /// the Business or Service Layer.
                employeeCommandRepository.Insert(new List<Employee> { managerEmployeeFake, subEmployeeFake });
                departmentCommandRepository.Insert(departmentFake2);
                unitOfWork.Commit();

                //Assert
                departmentQueryableRepository.Count().Should().Be(2);
                employeeQueryableRepository.Count().Should().Be(2);

                ///Need to delete this record for the next run to run without any issues since 
                ///SQL Lite will persist the data and if the same data is tried to be inserted again, 
                ///duplicate key error will be thrown.
                ///Also, since Cascade delete is set to true in EmployeeMap, deleting the department
                ///instance should automatically take care of underlying employee objects.
                departmentCommandRepository.Delete(departmentFake);
                departmentCommandRepository.Delete(departmentFake2);
            };
        }

        /// <summary>
        /// Since tested locally, MSDTC need not be enabled.
        /// </summary>
        [TestMethod]
        [TestCategory("Medium")]
        public void test_insert_multiple_employees_alongwith_department_with_explicit_transaction_scope_with_exception_thrown_in_between_should_rollback()
        {
            //Arrange
            var unitOfWorkWithExceptionToBeThrown = GetUnitOfWorkInstance(true);
            using (var departmentCommandRepository = GetCommandRepositoryInstance<Department>(unitOfWorkWithExceptionToBeThrown))
            using (var employeeCommandRepository = GetCommandRepositoryInstance<Employee>(unitOfWorkWithExceptionToBeThrown))
            using (var departmentQueryableRepository = GetQueryableRepositoryInstance<Department>())
            using (var employeeQueryableRepository = GetQueryableRepositoryInstance<Employee>())
            {
                //Arrange
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

                //Action
                /// Order of operations of different instances of same type or different types needs to be handled at 
                /// the Business or Service Layer.
                employeeCommandRepository.Insert(new List<Employee> { managerEmployeeFake, subEmployeeFake });
                departmentCommandRepository.Insert(departmentFake2);
                unitOfWorkWithExceptionToBeThrown.Commit();

                //Assert
                departmentQueryableRepository.Count().Should().Be(0);
                employeeQueryableRepository.Count().Should().Be(0);
            };
        }

        [TestMethod]
        [TestCategory("Slow")]
        public void test_insert_multiple_employees_alongwith_department_service_with_explicit_transaction_scope_should_be_saved()
        {
            //Arrange
            var unitOfWork = GetUnitOfWorkInstance();
            using (var departmentCommandRepository = GetDepartmentCommandServiceRepositoryInstance(unitOfWork))
            using (var employeeCommandRepository = GetCommandRepositoryInstance<Employee>(unitOfWork))
            using (var departmentQueryableRepository = GetQueryableRepositoryInstance<Department>())
            using (var employeeQueryableRepository = GetQueryableRepositoryInstance<Employee>())
            {
                //Arrange
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

                //Action
                /// Order of operations of different instances of same type or different types needs to be handled at 
                /// the Business or Service Layer.
                employeeCommandRepository.Insert(new List<Employee> { managerEmployeeFake, subEmployeeFake });
                departmentCommandRepository.Insert(departmentFake2);
                unitOfWork.Commit();

                //Assert
                departmentQueryableRepository.Count().Should().Be(2);
                employeeQueryableRepository.Count().Should().Be(2);

                ///Need to delete this record for the next run to run without any issues since 
                ///SQL Lite will persist the data and if the same data is tried to be inserted again, 
                ///duplicate key error will be thrown.
                ///Also, since Cascade delete is set to true in EmployeeMap, deleting the department
                ///instance should automatically take care of underlying employee objects.
                departmentCommandRepository.Delete(departmentFake);
                departmentCommandRepository.Delete(departmentFake2);
            };
        }

        /// <summary>
        /// Since tested locally, MSDTC need not be enabled.
        /// </summary>
        [TestMethod]
        [TestCategory("Medium")]
        public void test_insert_multiple_employees_alongwith_department_with_explicit_transaction_scope_for_sql_ce_should_be_saved()
        {
            //Arrange
            var unitOfWork = GetUnitOfWorkInstance();
            using (var departmentCommandRepository = GetDepartmentCommandServiceRepositoryInstance(unitOfWork))
            using (var employeeCommandRepository = GetCommandRepositoryInstance<Employee>(unitOfWork))
            using (var departmentQueryableRepository = GetQueryableRepositoryInstance<Department>())
            using (var employeeQueryableRepository = GetQueryableRepositoryInstance<Employee>())
            {
                //Arrange
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

                //Action
                /// Order of operations of different instances of same type or different types needs
                /// to be handled at the Business or Service Layer.
                employeeCommandRepository.Insert(new List<Employee> { managerEmployeeFake, subEmployeeFake });
                departmentCommandRepository.Insert(departmentFake2);
                unitOfWork.Commit();

                //Assert
                departmentQueryableRepository.Count().Should().Be(2);
                employeeQueryableRepository.Count().Should().Be(2);

                ///Need to delete this record for the next run to run without any issues since 
                ///SQL Lite will persist the data and if the same data is tried to be inserted again, 
                ///duplicate key error will be thrown.
                ///Also, since Cascade delete is set to true in EmployeeMap, deleting the department
                ///instance should automatically take care of underlying employee objects.
                departmentCommandRepository.Delete(departmentFake);
                departmentCommandRepository.Delete(departmentFake2);
            };
        }

        [TestMethod]
        [TestCategory("Medium")]
        public void test_bulk_insert_multiple_employees_without_any_transaction_should_be_saved()
        {
            //Arrange
            using (var departmentCommandRepository = GetCommandRepositoryInstance<Department>())
            using (var employeeCommandRepository = GetCommandRepositoryInstance<Employee>())
            using (var departmentQueryableRepository = GetQueryableRepositoryInstance<Department>())
            using (var employeeQueryableRepository = GetQueryableRepositoryInstance<Employee>())
            {
                //Arrange
                var departmentFake = GetDepartmentFake();

                var managerEmployeeFake = GetEmployeeFake();
                managerEmployeeFake.EmployeeName = "XYZ";
                managerEmployeeFake.DeptID = departmentFake.Id;
                managerEmployeeFake.Department = departmentFake;

                var subEmployeeFake = GetEmployeeFake(2);
                subEmployeeFake.DeptID = departmentFake.Id;
                subEmployeeFake.Department = departmentFake;
                subEmployeeFake.ManagerId = managerEmployeeFake.Id;
                subEmployeeFake.Manager = managerEmployeeFake;

                //Action
                employeeCommandRepository.BulkInsert(new List<Employee> { managerEmployeeFake, subEmployeeFake });

                //Assert
                departmentQueryableRepository.Count().Should().Be(1);
                employeeQueryableRepository.Count().Should().Be(2);
                employeeQueryableRepository.First().EmployeeName.Should().Be("XYZ");
                employeeQueryableRepository.Single(x => x.EmployeeName == "Sandip").Id.Should().Be(2);

                ///Need to delete this record for the next run to run without any issues since 
                ///SQL Lite will persist the data and if the same data is tried to be inserted again, 
                ///duplicate key error will be thrown.
                ///Also, since Cascade delete is set to true in EmployeeMap, deleting the department
                ///instance should automatically take care of underlying employee objects.
                departmentCommandRepository.Delete(departmentFake);
            };
        }

        #endregion

        #region Overrides

        protected override void RegisterEFTestContext()
        {
            _container.RegisterType<EFTestContext>(new InjectionConstructor());
        }

        protected override void RegisterDepartmentCommandService()
        {
            var name = typeof(Department).Name + SERVICE_SUFFIX;
            _container.RegisterType<ICommand<Department>, DepartmentTestServiceBasedOnSQLCECommand>(name, new InjectionConstructor());
            var context = _container.Resolve<EFTestContext>();
            var command = _container.Resolve<ICommand<Department>>(new ParameterOverride("dbContext", context));
            var injectionConstructor = new InjectionConstructor(typeof(BaseUnitOfWork), command);
            _container.RegisterType<ICommandRepository<Department>, CommandRepository<Department>>(name, injectionConstructor);
        }

        #endregion
    }
}
