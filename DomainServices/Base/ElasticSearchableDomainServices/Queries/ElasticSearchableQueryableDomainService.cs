using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Base;
using Domain.Base.Aggregates;
using Domain.Base.Entities;
using DomainServices.Base.QueryableDomainServices;
using Repository;
using Repository.Base;

namespace DomainServices.Base.ElasticSearchableDomainServices.Queries
{
    public class ElasticSearchableQueryableDomainService<TEntity, TId> : QueryableDomainService<TEntity, TId>, IElasticSearchableQueryableDomainService<TEntity, TId>
        where TEntity : BaseEntity<TId>, IQueryableAggregateRoot, IElasticSearchable
        where TId : struct
    {
        public ElasticSearchableQueryableDomainService(IQueryableElasticSearchRepository<TEntity> repository) : base(repository)
        {

        }

        public override IEnumerable<TEntity> IncludeList(Expression<Func<TEntity, object>> subSelector)
        {
            throw new NotSupportedException();
        }

        public override IQueryable<TEntity> Include(Expression<Func<TEntity, object>> subSelector)
        {
            throw new NotSupportedException();
        }

        public virtual IEnumerable<TEntity> QueryString(string term, Action operationToExecuteBeforeNextOperation = null)
        {
            return (_repository as IQueryableElasticSearchRepository<TEntity>).QueryString(term, operationToExecuteBeforeNextOperation);
        }

        public virtual PagingTableResult<TEntity> GetAllPagedResult(string id, int startIndex, int pageSize, string sorting, Action operationToExecuteBeforeNextOperation = null)
        {
            return (_repository as IQueryableElasticSearchRepository<TEntity>).GetAllPagedResult(id,startIndex,pageSize,sorting, operationToExecuteBeforeNextOperation);
        }
    }
}
