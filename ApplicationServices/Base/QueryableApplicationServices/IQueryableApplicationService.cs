using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Base;

namespace ApplicationServices.Base.QueryableApplicationServices
{
    public interface IQueryableApplicationService<TEntity> : IDisposable where TEntity : BaseIdentityAndAuditableQueryableAggregateRoot
    {
        IQueryable<TEntity> Get();
        IList<TEntity> GetAll();
        TEntity GetByID(int id);
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
