using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Base.AddOnObjects
{
    public class AuditInfo : IAddOnObject
    {
        public string CreatedBy { get; set; }

        [NotMapped]
        public virtual bool PreserveCreatedOn { get; } = true;

        public DateTimeOffset CreatedOn { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTimeOffset? LastUpdateOn { get; set; }
    }
}
