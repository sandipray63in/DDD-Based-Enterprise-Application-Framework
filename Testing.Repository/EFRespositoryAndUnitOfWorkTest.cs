using System;
using System.Data.Common;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Effort.Provider;
using FluentAssertions;
using TestEFDomainAndContext;
using TestEFDomainAndContext.TestDomains;
using Testing.Repository;

namespace Testing.Respository
{
    /// <summary>
    /// This library is more of Integration Testing rather than unit testing although it's not directly interacting with any 
    /// real external DB(rather using something like an in-memory DB using the Effort Framework - info regarding Effort 
    /// Framework can be found at http://www.codeproject.com/Articles/460175/Two-strategies-for-testing-Entity-Framework-Effort 
    /// and also at http://effort.codeplex.com/wikipage?title=Tutorials&referringTitle=Home). Here Repository, 
    /// Persiatance and Unit Of Work all together will be tested in an integrated way.
    /// 
    /// In order to make Effort Framework work seamlessly, need to add the corresponding provider in App.Config.
    /// 
    /// Also no mocking framework(MOQ, RhinoMock etc) used here since the intent here is to show how everything is working in an
    /// integrated mode.If one debugs through the tests everything will be easily understandable.Also mocking would lead to using
    /// LINQ to Objects which behaves quite differently sometimes compared to actual EF LINQ to Entities.
    /// 
    /// N.B. ->If Effort Framework is used in any other project alongwith EF6, make sure to download Effort.Ef6 package latest 
    /// version (using Nugget) and not the Effort(without ".EF6" suffix) package.Else it will take hours to fix all weird issues 
    /// when making Effort and EF 6 work together in sync.
    /// 
    /// Added the Test Categories which can be useful for Nightly, Weekly and Monthly Build process.
    /// </summary>
    [TestClass]
    public class EFRespositoryAndUnitOfWorkTest : BaseEFRespositoryAndUnitOfWorkTest
    {
        #region Private Constants(for Unity Name Resolution)

        private const string EF_TEST_CONTEXT_FOR_SQL_CE = "EFTestContextForSQLCE";

        #endregion

        #region Private Fields

        private DbConnection _connection;

        #endregion

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
                var departmentFake = FakeData.GetDepartmentFake();

                //Action
                departmentCommandRepository.Insert(departmentFake);

                //Assert
                departmentQueryableRepository.Single().DepartmentName.Should().Be("Election");
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
                employeeCommandRepository.Insert(new List<Employee> { managerEmployeeFake, subEmployeeFake });

                //Assert
                departmentQueryableRepository.Count().Should().Be(1);
                employeeQueryableRepository.Count().Should().Be(2);
                employeeQueryableRepository.First().EmployeeName.Should().Be("XYZ");
                employeeQueryableRepository.Single(x => x.EmployeeName == "Sandip").Id.Should().Be(2);

                // LINQ to Entities doesn't support the method "Last" and so will throw an exception.That's one of 
                // the differences w.r.t LINQ to Objects.If tested with some mock(s) then the below code won't throw any 
                // exception since internally mocks uses LINQ to Objects (atleast as far as LINQ in general is concerned) 
                // and so true behaviour won't reflect using mocks.

                Action action = () => employeeQueryableRepository.Last();
                action.ShouldThrow<NotSupportedException>();
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
                var departmentFake = FakeData.GetDepartmentFake();

                var employeeFake = FakeData.GetEmployeeFake();
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

                //Action
                /// Order of operations of different instances of same type or different types needs to be handled at 
                /// the Business or Service Layer.
                employeeCommandRepository.Insert(new List<Employee> { managerEmployeeFake, subEmployeeFake });
                departmentCommandRepository.Insert(departmentFake2);
                unitOfWork.Commit();

                //Assert
                departmentQueryableRepository.Count().Should().Be(2);
                employeeQueryableRepository.Count().Should().Be(2);
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
        
        #endregion

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








