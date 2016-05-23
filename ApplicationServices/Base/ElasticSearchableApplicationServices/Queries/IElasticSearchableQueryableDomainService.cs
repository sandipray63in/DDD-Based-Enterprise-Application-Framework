using System;
using System.Collections.Generic;
using ApplicationServices.Base.QueryableApplicationServices;
using Domain.Base;
using Repository;

namespace ApplicationServices.Base.ElasticSearchableDomainServices.Queries
{
    public interface IElasticSearchableQueryableDomainService<TEntity> : IQueryableApplicationService<TEntity> where TEntity : BaseIdentityAndAuditableQueryableAggregateRoot, IElasticSearchable
    {
        IList<TEntity> QueryString(string term, Action operationToExecuteBeforeNextOperation = null);

        PagingTableResult<TEntity> GetAllPagedResult(string id, int startIndex, int pageSize, string sorting, Action operationToExecuteBeforeNextOperation = null);

        //TODO - Need to include the Fuzzy Search API
    }
}
