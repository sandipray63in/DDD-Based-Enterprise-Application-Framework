#if TEST
using System.Data.Entity;

namespace TestEFDomainAndContext
{
    internal class EmployeeDepartmentDBInitializer : CreateDatabaseIfNotExists<EFTestContext>
    {
        protected override void Seed(EFTestContext context)
        {
            var department = FakeData.GetDepartmentFake(1000);
            var employee = FakeData.GetEmployeeFake(1000);
            context.Departments.Add(department);
            context.Employees.Add(employee);
            base.Seed(context);
        }
    }
}

#endif
