using Domain.Base.Aggregates;

namespace Domain.Base.Entities
{
    public abstract class BaseEntityAsCommandAggregateRoot<TId> : BaseEntity<TId>, ICommandAggregateRoot
        where TId : struct
    {
        public BaseEntityAsCommandAggregateRoot(TId id) : base(id)
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
    }
}
