using Domain.Base;

namespace Repository.Base
{
    public interface ICommandElasticSearchableRepository<TEntity> : ICommandRepository<TEntity> where TEntity : ICommandAggregateRoot,IElasticSearchable
    {

    }
}
