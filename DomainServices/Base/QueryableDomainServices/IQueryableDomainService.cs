using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Base.Aggregates;
using Domain.Base.Entities;

namespace DomainServices.Base.QueryableDomainServices
{
    public interface IQueryableDomainService<TEntity, TId> : IDisposable
        where TEntity : BaseEntity<TId>, IQueryableAggregateRoot
        where TId : struct
    {
        IQueryable<TEntity> Get();
        IEnumerable<TEntity> GetAll();
        TEntity GetByID(TId id);
        IEnumerable<TEntity> GetByFilterExpression(Expression<Func<TEntity, bool>> whereExpression);
        IEnumerable<TEntity> GetByOrderExpression<TKey>(Expression<Func<TEntity, TKey>> orderExpression);
        IEnumerable<TEntity> GetByExpression<TKey>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TKey>> orderExpression);

        /// <summary>
        /// Can be used for Includes which ultimately needs to return an Enumerable.Useful only in case of single Include 
        /// scenarios.
        /// </summary>
        /// <param name="subSelector"></param>
        /// <returns></returns>
        IEnumerable<TEntity> IncludeList(Expression<Func<TEntity, object>> subSelector);

        /// <summary>
        /// Can be used for multiple Includes.
        /// </summary>
        /// <param name="subSelector"></param>
        /// <returns></returns>
        IQueryable<TEntity> Include(Expression<Func<TEntity, object>> subSelector);
    }
}
