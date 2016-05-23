using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Base
{
    public abstract class BaseAuditableEntity
    {
        public string CreatedBy { get; set; }

        [NotMapped]
        public virtual bool PreserveCreatedOn { get; } = true;

        public DateTimeOffset CreatedOn { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTimeOffset? LastUpdateOn { get; set; }
    }
}
