using Domain.Base.AddOnObjects;
using Domain.Base.Aggregates;
using Domain.Base.Entities.Composites;

namespace TestEFDomainAndContext.TestDomains
{
    /// <summary>
    /// If needed, Data Annotations can be applied for Business Validations/Rules
    /// </summary>
    public class Department : BaseEntityComposite<int, AuditInfo>, ICommandAggregateRoot, IQueryableAggregateRoot
    {
        /// <summary>
        /// Entity Framework needs a default constructor while fetching data 
        /// from external data source
        /// </summary>
        public Department(){ }

        public Department(int id) : base(id)
        {

        }

        public string DepartmentName { get; set; }

        public string Loaction { get; set; }
    }
}
