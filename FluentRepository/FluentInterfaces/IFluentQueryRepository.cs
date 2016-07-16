using System;
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
        IFluentQueries SetUpQueryPersistance<TEntity>(Func<IQuery<TEntity>> queryFunc)
            where TEntity : class, IQueryableAggregateRoot;
    }
}
