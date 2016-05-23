using EFDomainMaps.Base.EFBase;
using TestEFDomainAndContext.TestDomains;

namespace TestEFDomainAndContext.TestEFDomainMaps
{
    public class DepartmentMap : BaseIdentityAndAuditableAggregateRootMap<Department>
    {
        #region Actual Columns Constants

        private const string DEPARTMENT_NAME_COLUMN_NAME = "col_department_name";
        private const string LOCATION_COLUMN_NAME = "col_dept_location";

        #endregion

        protected override string IDColumnName { get; } = "col_dept_id";

        protected override void SetEntitySpecificProperties()
        {
            Property(p => p.DepartmentName).HasColumnName(DEPARTMENT_NAME_COLUMN_NAME);
            Property(p => p.Loaction).HasColumnName(LOCATION_COLUMN_NAME);
        }

        protected override string TableName { get; } = "tbl_Dept";
    }
}
