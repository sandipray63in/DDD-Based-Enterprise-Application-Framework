using Domain.Base.Aggregates;

namespace Domain.Base.Entities
{
    public abstract class BaseEntityAsQueryableAggregateRoot<TId> : BaseEntity<TId>, IQueryableAggregateRoot
        where TId : struct
    {
        public BaseEntityAsQueryableAggregateRoot(TId id) : base(id)
        {

        }
    }
}
