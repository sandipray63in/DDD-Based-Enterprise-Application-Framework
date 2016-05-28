using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Base.Aggregates;

namespace Repository.Base
{
    public interface IQueryableRepository<TEntity> : IQueryable<TEntity>, IDisposable where TEntity : IQueryableAggregateRoot
    {
        void RunQuery(Func<IQueryableRepository<TEntity>, TEntity> queryableRepositoryOperation, Action<TEntity> operationToExecuteBeforeNextOperation = null);

        void RunQuery(Func<IQueryableRepository<TEntity>, IEnumerable<TEntity>> queryableRepositoryOperation, Action<IEnumerable<TEntity>> operationToExecuteBeforeNextOperation = null);

        /// <summary>
        /// Can be useful for queries which doesn't return TEntity or IEnumerable<TEntity> e.g Count or Avg queries will 
        /// return some int or float or some other type of values.
        /// </summary>
        void RunQuery<TIntermediateType>(Func<IQueryableRepository<TEntity>, TIntermediateType> queryableRepositoryOperation, Action<TIntermediateType> operationToExecuteBeforeNextOperation = null);

        IQueryable<TEntity> Include(Expression<Func<TEntity, object>> subSelector);

        #region Raw SQL

        IEnumerable<TEntity> GetWithRawSQL(String getQuery, params object[] parameters);

        #endregion
    }
}
