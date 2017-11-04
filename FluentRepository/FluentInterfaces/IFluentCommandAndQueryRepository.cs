using System.Collections.Generic;
using Domain.Base.Aggregates;
using Repository.Base;

namespace FluentRepository.FluentInterfaces
{
    public interface IFluentCommandAndQueryRepository
    {
        /// <summary>
        /// Set Up the Command Repository
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="commandRepository"></param>
        /// <returns></returns>
        IFluentCommandRepository SetUpCommandRepository<TEntity>(ICommandRepository<TEntity> commandRepository)
            where TEntity : class, ICommandAggregateRoot;

        /// <summary>
        /// Set Up array of Command Repositories
        /// </summary>
        /// <param name="commandRepositories"></param>
        /// <returns></returns>
        IFluentCommandRepository SetUpCommandRepository(params dynamic[] commandRepositories);

        /// <summary>
        /// Set Up enumerable of Command Repositories
        /// </summary>
        /// <param name="commandRepositories"></param>
        /// <returns></returns>
        IFluentCommandRepository SetUpCommandRepository(IEnumerable<dynamic> commandRepositories);

        /// <summary>
        /// Set Up the Query Repository
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="queryRepository"></param>
        /// <returns></returns>
        IFluentQueryRepository SetUpQueryRepository<TEntity>(IQueryableRepository<TEntity> queryRepository)
            where TEntity : class, IQueryableAggregateRoot;

        /// <summary>
        /// Set Up array of Query Repositories
        /// </summary>
        /// <param name="queryRepositories"></param>
        /// <returns></returns>
        IFluentQueryRepository SetUpQueryRepository(params dynamic[] queryRepositories);

        /// <summary>
        /// Set Up enumerable of Query Repositories
        /// </summary>
        /// <param name="queryRepositories"></param>
        /// <returns></returns>
        IFluentQueryRepository SetUpQueryRepository(IEnumerable<dynamic> queryRepositories);
    }
}
