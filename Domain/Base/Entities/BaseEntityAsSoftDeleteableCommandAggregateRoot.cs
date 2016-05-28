using System;
using Domain.Base.Aggregates;

namespace Domain.Base.Entities
{
    public abstract class BaseEntityAsSoftDeleteableCommandAggregateRoot<TId> : BaseEntity<TId>, ISoftDeleteableCommandAggregateRoot
        where TId : struct
    {
        public BaseEntityAsSoftDeleteableCommandAggregateRoot(TId id) : base(id)
        {

        }

        #region ICommandAggregateRoot Members

        /// <summary>
        /// Gets wether the current aggregate can be saved
        /// </summary>
        public virtual bool CanBeSaved { get; } = true;

        /// <summary>
        /// Gets wether the current aggregate can be deleted
        /// </summary>
        public virtual bool CanBeDeleted { get; } = true;

        #endregion

        #region ISoftDeleteableCommandAggregateRoot

        public DateTimeOffset? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }

        #endregion
    }
}
