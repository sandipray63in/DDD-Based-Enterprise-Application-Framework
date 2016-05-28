using Domain.Base;
using Domain.Base.Aggregates;

namespace Repository.Base
{
    public interface ICommandElasticSearchableRepository<TEntity> : ICommandRepository<TEntity> where TEntity : ICommandAggregateRoot,IElasticSearchable
    {

    }
}
