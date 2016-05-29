using System;
using Domain.Base.Aggregates;

namespace Domain.Base.Entities
{
    public abstract class BaseEntityAsSoftDeleteableCommandAggregateRoot<TId> : BaseEntityAsCommandAggregateRoot<TId>, ISoftDeleteableCommandAggregateRoot
        where TId : struct
    {
        public BaseEntityAsSoftDeleteableCommandAggregateRoot(TId id) : base(id)
        {

        }

        #region ISoftDeleteableCommandAggregateRoot

        public DateTimeOffset? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }

        #endregion
    }
}
