using System;

namespace Domain.Base.Mongo
{
    public abstract class BaseMongoIdentityAndAuditableAndSoftDeleteableCommandAggregateRoot : BaseMongoIdentityAndAuditableCommandAggregateRoot
    {
        public virtual bool IsDeleted { get; set; } = true;

        public virtual DateTimeOffset? DeletedOn { get; set; }
    }
}
