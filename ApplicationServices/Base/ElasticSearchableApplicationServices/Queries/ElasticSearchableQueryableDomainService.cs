using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ApplicationServices.Base.QueryableApplicationServices;
using Domain.Base;
using Repository;
using Repository.Base;

namespace ApplicationServices.Base.ElasticSearchableDomainServices.Queries
{
    public class ElasticSearchableQueryableDomainService<TEntity> : QueryableApplicationService<TEntity>,IElasticSearchableQueryableDomainService<TEntity> where TEntity : BaseIdentityAndAuditableQueryableAggregateRoot, IElasticSearchable
    {
        public ElasticSearchableQueryableDomainService(IQueryableElasticSearchRepository<TEntity> repository) : base(repository)
        {

        }

        public override IList<TEntity> IncludeList(Expression<Func<TEntity, object>> subSelector)
        {
            throw new NotSupportedException();
        }

        public override IQueryable<TEntity> Include(Expression<Func<TEntity, object>> subSelector)
        {
            throw new NotSupportedException();
        }

        public virtual IList<TEntity> QueryString(string term, Action operationToExecuteBeforeNextOperation = null)
        {
            return (_repository as IQueryableElasticSearchRepository<TEntity>).QueryString(term, operationToExecuteBeforeNextOperation);
        }

        public virtual PagingTableResult<TEntity> GetAllPagedResult(string id, int startIndex, int pageSize, string sorting, Action operationToExecuteBeforeNextOperation = null)
        {
            return (_repository as IQueryableElasticSearchRepository<TEntity>).GetAllPagedResult(id,startIndex,pageSize,sorting, operationToExecuteBeforeNextOperation);
        }
    }
}
