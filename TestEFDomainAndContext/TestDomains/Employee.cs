using System;
using Domain.Base;

namespace TestEFDomainAndContext.TestDomains
{
    /// <summary>
    /// If needed, Data Annotations can be applied for Business Validations/Rules
    /// </summary>
    public class Employee : BaseIdentityAndAuditableCommandAggregateRoot, IQueryableAggregateRoot
    {
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
