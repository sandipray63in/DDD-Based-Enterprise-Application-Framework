using System;
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
        /// <param name="commandRepositoryFunc"></param>
        /// <returns></returns>
        IFluentCommandRepository SetUpCommandRepository<TEntity>(Func<ICommandRepository<TEntity>> commandRepositoryFunc)
            where TEntity : class, ICommandAggregateRoot;

        /// <summary>
        /// Set Up the Query Repository
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="queryRepositoryFunc"></param>
        /// <returns></returns>
        IFluentQueryRepository SetUpQueryRepository<TEntity>(Func<IQueryableRepository<TEntity>> queryRepositoryFunc)
            where TEntity : class, IQueryableAggregateRoot; 
    }
}
