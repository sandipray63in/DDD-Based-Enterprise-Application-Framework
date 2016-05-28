using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Base.ValueObjects
{
    public class AuditInfo : BaseValueObject<AuditInfo>
    {
        public string CreatedBy { get; set; }

        [NotMapped]
        public virtual bool PreserveCreatedOn { get; } = true;

        public DateTimeOffset CreatedOn { get; set; }

        public string LastUpdatedBy { get; set; }

        public DateTimeOffset? LastUpdateOn { get; set; }

        protected override IEnumerable<object> GetAttributesToIncludeInEqualityCheck()
        {
            return new List<Object> { this.CreatedBy, this.CreatedOn, this.LastUpdatedBy, this.LastUpdateOn };
        }
    }
}
