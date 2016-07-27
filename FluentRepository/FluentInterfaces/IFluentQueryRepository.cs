using System.Collections.Generic;
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

        /// <summary>
        /// Set up Query Persistances if needed
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        IFluentQueries SetUpQueryPersistance(params dynamic[] queries);

        /// <summary>
        /// Set up Query Persistances if needed
        /// </summary>
        /// <param name="queries"></param>
        /// <returns></returns>
        IFluentQueries SetUpQueryPersistance(IList<dynamic> queries);
    }
}
