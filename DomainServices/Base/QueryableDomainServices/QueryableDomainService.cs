using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Base.Aggregates;
using Domain.Base.Entities;
using Infrastructure;
using Infrastructure.Utilities;
using Repository.Base;

namespace DomainServices.Base.QueryableDomainServices
{
    public class QueryableDomainService<TId,TEntity> : DisposableClass, IQueryableDomainService<TId,TEntity>
        where TId : struct
        where TEntity : BaseEntity<TId>, IQueryableAggregateRoot
    {
        protected readonly IQueryableRepository<TEntity> _repository;

        public QueryableDomainService(IQueryableRepository<TEntity> repository)
        {
            ContractUtility.Requires<ArgumentNullException>(repository != null, "repository instance cannot be null");
            _repository = repository;
        }

        public virtual IQueryable<TEntity> Get()
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(QueryableDomainService<TId, TEntity>).FullName);
            return _repository;
        }

        public virtual IList<TEntity> GetAll()
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(QueryableDomainService<TId, TEntity>).FullName);
            return _repository.Select(x => x).ToList();
        }

        public virtual TEntity GetByID(TId id)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(QueryableDomainService<TId, TEntity>).FullName);
            return _repository.FirstOrDefault(x => x.Id.Equals(id));
        }

        public virtual IList<TEntity> GetByFilterExpression(Expression<Func<TEntity, bool>> whereExpression)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(QueryableDomainService<TId, TEntity>).FullName);
            return _repository.Where(whereExpression).ToList();
        }

        public virtual IList<TEntity> GetByOrderExpression<TKey>(Expression<Func<TEntity, TKey>> orderExpression)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(QueryableDomainService<TId, TEntity>).FullName);
            return _repository.OrderBy(orderExpression).ToList();
        }

        public virtual IList<TEntity> GetByExpression<TKey>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TKey>> orderExpression)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(QueryableDomainService<TId, TEntity>).FullName);
            return _repository.Where(whereExpression).OrderBy(orderExpression).ToList();
        }

        public virtual IList<TEntity> IncludeList(Expression<Func<TEntity, object>> subSelector)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(QueryableDomainService<TId, TEntity>).FullName);
            return _repository.Include(subSelector).ToList();
        }

        public virtual IQueryable<TEntity> Include(Expression<Func<TEntity, object>> subSelector)
        {
            CheckForObjectAlreadyDisposedOrNot(typeof(QueryableDomainService<TId, TEntity>).FullName);
            return _repository.Include(subSelector);
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
