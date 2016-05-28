using Domain.Base.Aggregates;

namespace Domain.Base.Entities.Mongo.WithAuditInfo
{
    public abstract class BaseMongoEntityWithAuditInfoAsCommandAggregateRoot<TId> : BaseMongoEntityWithAuditInfo<TId>, ICommandAggregateRoot
        where TId : struct
    {
        public BaseMongoEntityWithAuditInfoAsCommandAggregateRoot(string MongoId, TId id) : base(MongoId, id)
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
