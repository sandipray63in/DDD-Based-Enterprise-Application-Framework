using System;

namespace Domain.Base
{
    public abstract class BaseIdentityAndAuditableAndSoftDeleteableCommandAggregateRoot : BaseIdentityAndAuditableCommandAggregateRoot, ISoftDeleteableAggregateRoot
    {
        public virtual bool IsDeleted { get; set; } = true;

        public virtual DateTimeOffset? DeletedOn { get; set; }
    }
}
