using Domain.Base.Aggregates;

namespace Domain.Base.Entities.WithAuditInfo
{
    public abstract class BaseEntityWithAuditInfoAsQueryableAggregateRoot<TId> : BaseEntityWithAuditInfo<TId>, IQueryableAggregateRoot
        where TId : struct
    {
        public BaseEntityWithAuditInfoAsQueryableAggregateRoot(TId id) : base(id)
        {

        }
    }
}
