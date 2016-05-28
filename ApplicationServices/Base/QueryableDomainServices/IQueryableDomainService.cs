using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Base.Aggregates;
using Domain.Base.Entities;

namespace DomainServices.Base.QueryableDomainServices
{
    public interface IQueryableDomainService<TId, TEntity> : IDisposable
        where TId : struct
        where TEntity : BaseEntity<TId>, IQueryableAggregateRoot
    {
        IQueryable<TEntity> Get();
        IList<TEntity> GetAll();
        TEntity GetByID(TId id);
        IList<TEntity> GetByFilterExpression(Expression<Func<TEntity, bool>> whereExpression);
        IList<TEntity> GetByOrderExpression<TKey>(Expression<Func<TEntity, TKey>> orderExpression);
        IList<TEntity> GetByExpression<TKey>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TKey>> orderExpression);

        /// <summary>
        /// Can be used for Includes which ultimately needs to return a List.Useful only in case of single Include 
        /// scenarios.
        /// </summary>
        /// <param name="subSelector"></param>
        /// <returns></returns>
        IList<TEntity> IncludeList(Expression<Func<TEntity, object>> subSelector);

        /// <summary>
        /// Can be used for multiple Includes.
        /// </summary>
        /// <param name="subSelector"></param>
        /// <returns></returns>
        IQueryable<TEntity> Include(Expression<Func<TEntity, object>> subSelector);
    }
}
