using System;
using Domain.Base.Aggregates;
using Domain.Base.AddOnObjects;
using Domain.Base.Entities.Composites;

namespace TestEFDomainAndContext.TestDomains
{
    /// <summary>
    /// If needed, Data Annotations can be applied for Business Validations/Rules
    /// </summary>
    public class Employee : BaseEntityComposite<int,AuditInfo>,ICommandAggregateRoot, IQueryableAggregateRoot
    {
        /// <summary>
        /// Entity Framework needs a default constructor while fetching data 
        /// from external data source
        /// </summary>
        public Employee() { }

        public Employee(int id) : base(id)
        {
        }

        public string EmployeeName { get; set; }

        public string Job { get; set; }

        public int? ManagerId { get; set; }

        public Employee Manager { get; set; }

        public DateTimeOffset HireDate { get; set; } = DateTimeOffset.UtcNow;

        public int DeptID { get; set; }

        public Department Department { get; set; }

    }
}
