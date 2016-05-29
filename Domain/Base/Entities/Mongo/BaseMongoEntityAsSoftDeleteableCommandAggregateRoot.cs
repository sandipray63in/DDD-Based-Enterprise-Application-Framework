using System;
using Domain.Base.Aggregates;

namespace Domain.Base.Entities.Mongo
{
    public abstract class BaseMongoEntityAsSoftDeleteableCommandAggregateRoot<TId> : BaseMongoEntityAsCommandAggregateRoot<TId>, ISoftDeleteableCommandAggregateRoot
        where TId : struct
    {
        public BaseMongoEntityAsSoftDeleteableCommandAggregateRoot(string MongoId, TId id) : base(MongoId, id)
        {

        }

        #region ISoftDeleteableCommandAggregateRoot

        public DateTimeOffset? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }

        #endregion
    }
}
