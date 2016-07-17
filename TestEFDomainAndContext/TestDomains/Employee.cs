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

        /// <summary>
        /// ID is needed if there is one to many mapping 
        /// 
        /// For one to one mapping just reference the objects on both the sides 
        /// and use HasOptional/WithOptional or HasRequired/WithRequired
        /// </summary>
        public int DeptID { get; set; }

        public Department Department { get; set; }

    }
}
