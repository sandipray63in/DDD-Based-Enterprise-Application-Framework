using Domain.Base.Aggregates;
using Repository.Queryable;

namespace FluentRepository.FluentInterfaces
{
    public interface IFluentQueryRepository : IFluentQueries
    {
        /// <summary>
        /// Set up Query Persistance instance if needed
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        IFluentQueries SetUpQueryPersistance<TEntity>(IQuery<TEntity> query)
            where TEntity : class, IQueryableAggregateRoot;
    }
}
