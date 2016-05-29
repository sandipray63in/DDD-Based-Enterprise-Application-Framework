using System;
using Domain.Base.Aggregates;

namespace Domain.Base.Entities.WithAuditInfo
{
    public abstract class BaseEntityWithAuditInfoAsSoftDeleteableCommandAggregateRoot<TId> : BaseEntityWithAuditInfoAsCommandAggregateRoot<TId>, ISoftDeleteableCommandAggregateRoot
        where TId : struct
    {
        public BaseEntityWithAuditInfoAsSoftDeleteableCommandAggregateRoot(TId id) : base(id)
        {

        }

        #region ISoftDeleteableCommandAggregateRoot

        public DateTimeOffset? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }

        #endregion
    }
}
