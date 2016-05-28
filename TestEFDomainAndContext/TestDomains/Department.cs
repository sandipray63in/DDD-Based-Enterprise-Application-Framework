using Domain.Base.Aggregates;
using Domain.Base.Entities.WithAuditInfo;

namespace TestEFDomainAndContext.TestDomains
{
    /// <summary>
    /// If needed, Data Annotations can be applied for Business Validations/Rules
    /// </summary>
    public class Department : BaseEntityWithAuditInfoAsCommandAggregateRoot<int>, IQueryableAggregateRoot
    {
        public Department(int id) : base(id)
        {

        }

        public string DepartmentName { get; set; }

        public string Loaction { get; set; }
    }
}
