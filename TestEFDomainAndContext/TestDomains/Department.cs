using Domain.Base;

namespace TestEFDomainAndContext.TestDomains
{
    /// <summary>
    /// If needed, Data Annotations can be applied for Business Validations/Rules
    /// </summary>
    public class Department : BaseIdentityAndAuditableCommandAggregateRoot, IQueryableAggregateRoot
    {
        public string DepartmentName { get; set; }

        public string Loaction { get; set; }
    }
}
