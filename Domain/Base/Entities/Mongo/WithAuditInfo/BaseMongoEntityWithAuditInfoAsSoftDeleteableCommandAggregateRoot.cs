using System;
using Domain.Base.Aggregates;

namespace Domain.Base.Entities.Mongo.WithAuditInfo
{
    public abstract class BaseMongoEntityWithAuditInfoAsSoftDeleteableCommandAggregateRoot<TId> : BaseMongoEntityWithAuditInfoAsCommandAggregateRoot<TId>, ISoftDeleteableCommandAggregateRoot
        where TId : struct
    {
        public BaseMongoEntityWithAuditInfoAsSoftDeleteableCommandAggregateRoot(string MongoId, TId id) : base(MongoId, id)
        {

        }

        #region ISoftDeleteableCommandAggregateRoot

        public DateTimeOffset? DeletedOn { get; set; }

        public bool IsDeleted { get; set; }

        #endregion
    }
}
