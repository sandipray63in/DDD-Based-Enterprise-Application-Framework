using Domain.Base.Aggregates;

namespace Domain.Base.Entities.Mongo
{
    public abstract class BaseMongoEntityAsQueryableAggregateRoot<TId> : BaseMongoEntity<TId>, IQueryableAggregateRoot
        where TId : struct
    {
        public BaseMongoEntityAsQueryableAggregateRoot(string MongoId, TId id) : base(MongoId, id)
        {

        }
    }
}
