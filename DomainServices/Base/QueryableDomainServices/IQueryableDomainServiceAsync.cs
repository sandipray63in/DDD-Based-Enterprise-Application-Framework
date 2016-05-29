using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Domain.Base.Aggregates;
using Domain.Base.Entities;

namespace DomainServices.Base.QueryableDomainServices
{
    public interface IQueryableDomainServiceAsync<TId, TEntity> : IDisposable
        where TId : struct
        where TEntity : BaseEntity<TId>, IQueryableAggregateRoot
    {
        Task<IQueryable<TEntity>> GetAsync(CancellationToken token = default(CancellationToken));
        Task<IList<TEntity>> GetAllAsync(CancellationToken token = default(CancellationToken));
        Task<TEntity> GetByIDAsync(TId id, CancellationToken token = default(CancellationToken));
        Task<IList<TEntity>> GetByFilterExpressionAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken token = default(CancellationToken));
        Task<IList<TEntity>> GetByOrderExpressionAsync<TKey>(Expression<Func<TEntity, TKey>> orderExpression, CancellationToken token = default(CancellationToken));
        Task<IList<TEntity>> GetByExpressionAsync<TKey>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TKey>> orderExpression, CancellationToken token = default(CancellationToken));
    }
}
