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
    public interface IQueryableDomainServiceAsync<TEntity, TId> : IDisposable
        where TEntity : BaseEntity<TId>, IQueryableAggregateRoot
        where TId : struct
    {
        Task<IQueryable<TEntity>> GetAsync(CancellationToken token = default(CancellationToken));
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken token = default(CancellationToken));
        Task<TEntity> GetByIDAsync(TId id, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<TEntity>> GetByFilterExpressionAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<TEntity>> GetByOrderExpressionAsync<TKey>(Expression<Func<TEntity, TKey>> orderExpression, CancellationToken token = default(CancellationToken));
        Task<IEnumerable<TEntity>> GetByExpressionAsync<TKey>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TKey>> orderExpression, CancellationToken token = default(CancellationToken));
    }
}
