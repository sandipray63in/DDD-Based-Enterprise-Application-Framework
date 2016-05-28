using Domain.Base.Aggregates;

namespace Domain.Base.Entities.Mongo.WithAuditInfo
{
    public abstract class BaseMongoEntityWithAuditInfoAsQueryableAggregateRoot<TId> : BaseMongoEntityWithAuditInfo<TId>, IQueryableAggregateRoot
        where TId : struct
    {
        public BaseMongoEntityWithAuditInfoAsQueryableAggregateRoot(string MongoId, TId id) : base(MongoId, id)
        {

        }
    }
}
