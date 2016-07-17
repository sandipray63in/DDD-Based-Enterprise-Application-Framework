using DomainContextsAndMaps.Base.EFBase;
using TestEFDomainAndContext.TestDomains;

namespace TestEFDomainAndContext.TestEFDomainMaps
{
    public class EmployeeMap : BaseEntityWithAuditInfoRootMap<int, Employee>
    {
        #region Actual Columns Constants

        private const string EMPLOYEE_NAME_COLUMN_NAME = "col_employee_name";
        private const string JOB_COLUMN_NAME = "col_emp_job";
        private const string MANAGER_ID_COLUMN_NAME = "col_mgr_id";
        private const string HIRE_DATE_COLUMN_NAME = "col_hire_date";
        private const string DEPARTMENT_ID_COLUMN_NAME = "col_dept_id";

        #endregion

        protected override string IDColumnName { get; } = "col_emp_id";

        protected override void SetSpecificPropertiesForEntity()
        {
            Property(p => p.EmployeeName).HasColumnName(EMPLOYEE_NAME_COLUMN_NAME);
            Property(p => p.Job).HasColumnName(JOB_COLUMN_NAME);
            Property(p => p.ManagerId).HasColumnName(MANAGER_ID_COLUMN_NAME);
            HasOptional(p => p.Manager).WithMany().HasForeignKey(p => p.ManagerId);
            Property(p => p.HireDate).HasColumnName(HIRE_DATE_COLUMN_NAME);
            Property(p => p.DeptID).HasColumnName(DEPARTMENT_ID_COLUMN_NAME);
            HasRequired(p => p.Department).WithMany().HasForeignKey(p => p.DeptID).WillCascadeOnDelete(true);
        }

        protected override string TableName { get; } = "tbl_Employee";
    }
}
