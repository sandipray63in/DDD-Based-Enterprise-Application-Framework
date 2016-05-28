using Domain.Base.Aggregates;

namespace Domain.Base.Entities.Mongo
{
    public abstract class BaseMongoEntityAsCommandAggregateRoot<TId> : BaseMongoEntity<TId>, ICommandAggregateRoot
        where TId : struct
    {
        public BaseMongoEntityAsCommandAggregateRoot(string MongoId, TId id) : base(MongoId,id)
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
