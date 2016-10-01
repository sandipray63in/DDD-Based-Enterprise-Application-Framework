using TestEFDomainAndContext.TestDomains;

namespace Testing.Repository
{
    public static class FakeData
    {
        public static Department GetDepartmentFake(int id = 1)
        {
            var department = new Department(id);
            department.DepartmentName = "Election";
            department.Loaction = "Centre";
            department.T1Data.CreatedBy = "Sandip";
            department.T1Data.LastUpdatedBy = "Sandip";
            return department;
        }

        public static Employee GetEmployeeFake(int id = 1)
        {
            var employee = new Employee(id);
            employee.EmployeeName = "Sandip";
            employee.Job = "Software Development";
            employee.T1Data.CreatedBy = "Sandip";
            employee.T1Data.LastUpdatedBy = "Sandip";
            return employee;
        }
    }
}
