using System;
using System.Collections.Generic;
using Domain.Base.Aggregates;
using Repository.Base;

namespace FluentRepository.FluentInterfaces
{
    public interface IFluentQueries : IFluentSetUpAndExecution
    {
        IFluentQueries Query<TEntity>(Func<IQueryableRepository<TEntity>, TEntity> queryableRepositoryOperation, Action<TEntity> operationToExecuteBeforeNextOperation = null)
            where TEntity : class, IQueryableAggregateRoot;

        IFluentQueries Query<TEntity>(Func<IQueryableRepository<TEntity>, IEnumerable<TEntity>> queryableRepositoryOperation, Action<IEnumerable<TEntity>> operationToExecuteBeforeNextOperation = null)
            where TEntity : class, IQueryableAggregateRoot;

        /// <summary>
        /// Can be useful for queries which doesn't return TEntity or IEnumerable<TEntity> e.g Count or Avg queries will 
        /// return some int or float or some other type of values.
        /// </summary>
        IFluentQueries Query<TEntity,TIntermediateType>(Func<IQueryableRepository<TEntity>, TIntermediateType> queryableRepositoryOperation, Action<TIntermediateType> operationToExecuteBeforeNextOperation = null)
            where TEntity : class, IQueryableAggregateRoot;
    }
}
