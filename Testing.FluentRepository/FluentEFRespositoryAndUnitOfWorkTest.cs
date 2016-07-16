using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using FluencyRepository = FluentRepository;
using Testing.Respository;
using TestEFDomainAndContext.TestDomains;

namespace Testing.FluentRepository
{
    /// <summary>
    /// The fluent way of writing the code needs some more lines of code to be written(as shown below)
    /// but is much more intuitive and readable, compared to the normal Repository way of doing 
    /// things as shown in the test cases written in Testing.Repository project library.
    /// </summary>
    [TestClass]
    public class FluentEFRespositoryAndUnitOfWorkTest : BaseEFRespositoryAndUnitOfWorkTest
    {
        [TestMethod]
        [TestCategory("Fast")]
        public void test_insert_single_department_without_any_explicit_transaction_scope_should_be_saved()
        {
            //Arrange
            using (var departmentCommandRepository = GetCommandRepositoryInstance<Department>())
            using (var departmentQueryableRepository = GetQueryableRepositoryInstance<Department>())
            {
                ///Arrange
                var departmentFake = GetDepartmentFake();
                Department departmentAfterDataInsert = null;

                ///Action
                FluencyRepository.FluentRepository
                                .SetUpCommandRepository(() => departmentCommandRepository)
                                .Insert(departmentFake)
                                .SetUpNewQueryRepository(() => departmentQueryableRepository)
                                .RunQuery<Department>(x => x.Single(), x => departmentAfterDataInsert = x)
                                .Execute();

                ///Assert
                departmentAfterDataInsert.DepartmentName.Should().Be("Election");
            };
        }

        protected override void RegisterDepartmentCommandService()
        {
            throw new NotImplementedException();
        }

        protected override void RegisterEFTestContext()
        {
            throw new NotImplementedException();
        }
    }
}
