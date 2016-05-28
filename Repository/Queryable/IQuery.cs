using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Base.Aggregates;

namespace Repository.Queryable
{
    public interface IQuery<TEntity> : IDisposable, IQueryable<TEntity> where TEntity : IQueryableAggregateRoot
    {
        IQueryable<TEntity> Include(Expression<Func<TEntity, object>> subSelector);

        /// <summary>
        /// Useful in case of complex joins and complex subqueries
        /// </summary>
        /// <param name="getQuery"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IEnumerable<TEntity> GetWithRawSQL(String getQuery, params object[] parameters);
    }
}
