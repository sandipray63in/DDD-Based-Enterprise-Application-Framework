using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base;
using Infrastructure.Utilities;
using Repository.Base;

namespace ApplicationServices.Base.QueryableApplicationServices
{
    public class QueryableApplicationServiceAsync<TEntity> : IQueryableApplicationServiceAsync<TEntity> where TEntity : BaseIdentityAndAuditableQueryableAggregateRoot
    {
        protected readonly IQueryableRepository<TEntity> _repository;

        public QueryableApplicationServiceAsync(IQueryableRepository<TEntity> repository)
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
        
        public virtual async Task<TEntity> GetByIDAsync(int id, CancellationToken token = default(CancellationToken))
        {
            return await _repository.FirstOrDefaultAsync(x => x.Id == id, token);
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
    }
}
