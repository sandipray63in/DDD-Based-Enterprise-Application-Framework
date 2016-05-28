using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using Domain.Base;

namespace DomainServices.Base.QueryableDomainServices
{
    public interface IQueryableDomainServiceAsync<TEntity> : IDisposable where TEntity : BaseIdentityAndAuditableQueryableAggregateRoot
    {
        Task<IQueryable<TEntity>> GetAsync(CancellationToken token = default(CancellationToken));
        Task<IList<TEntity>> GetAllAsync(CancellationToken token = default(CancellationToken));
        Task<TEntity> GetByIDAsync(int id, CancellationToken token = default(CancellationToken));
        Task<IList<TEntity>> GetByFilterExpressionAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken token = default(CancellationToken));
        Task<IList<TEntity>> GetByOrderExpressionAsync<TKey>(Expression<Func<TEntity, TKey>> orderExpression, CancellationToken token = default(CancellationToken));
        Task<IList<TEntity>> GetByExpressionAsync<TKey>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TKey>> orderExpression, CancellationToken token = default(CancellationToken));
    }
}
