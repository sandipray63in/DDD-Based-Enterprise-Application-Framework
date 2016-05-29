using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base.Aggregates;
using Domain.Base.Entities;
using Infrastructure;
using Infrastructure.Utilities;
using Repository.Base;

namespace DomainServices.Base.QueryableDomainServices
{
    public class QueryableDomainServiceAsync<TId,TEntity> : DisposableClass, IQueryableDomainServiceAsync<TId, TEntity>
        where TId:struct
        where TEntity : BaseEntity<TId>,IQueryableAggregateRoot
    {
        protected readonly IQueryableRepository<TEntity> _repository;

        public QueryableDomainServiceAsync(IQueryableRepository<TEntity> repository)
        {
            ContractUtility.Requires<ArgumentNullException>(repository != null, "repository instance cannot be null");
            _repository = repository;
        }

        public virtual async Task<IQueryable<TEntity>> GetAsync()
        {
            return await GetAsync(CancellationToken.None);
        }

        public virtual async Task<IQueryable<TEntity>> GetAsync(CancellationToken token = default(CancellationToken))
        {
            return await Task.Run(() => _repository);
        }

        public virtual async Task<IList<TEntity>> GetAllAsync(CancellationToken token = default(CancellationToken))
        {
            return await _repository.Select(x => x).ToListAsync(token);
        }
        
        public virtual async Task<TEntity> GetByIDAsync(TId id, CancellationToken token = default(CancellationToken))
        {
            return await _repository.FirstOrDefaultAsync(x => x.Id.Equals(id), token);
        }

        public virtual async Task<IList<TEntity>> GetByFilterExpressionAsync(Expression<Func<TEntity, bool>> whereExpression, CancellationToken token = default(CancellationToken))
        {
            return await _repository.Where(whereExpression).ToListAsync(token);
        }

        public virtual async Task<IList<TEntity>> GetByOrderExpressionAsync<TKey>(Expression<Func<TEntity, TKey>> orderExpression, CancellationToken token = default(CancellationToken))
        {
            return await _repository.OrderBy(orderExpression).ToListAsync(token);
        }
        public virtual async Task<IList<TEntity>> GetByExpressionAsync<TKey>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TKey>> orderExpression, CancellationToken token = default(CancellationToken))
        {
            return await _repository.Where(whereExpression).OrderBy(orderExpression).ToListAsync(token);
        }

        #region Free Disposable Members

        protected override void FreeManagedResources()
        {
            base.FreeManagedResources();
            _repository.Dispose();
        }

        #endregion
    }
}
