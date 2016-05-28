using System;
using System.Collections.Generic;
using DomainServices.Base.QueryableDomainServices;
using Domain.Base;
using Repository;

namespace DomainServices.Base.ElasticSearchableDomainServices.Queries
{
    public interface IElasticSearchableQueryableDomainService<TEntity> : IQueryableDomainService<TEntity> where TEntity : BaseIdentityAndAuditableQueryableAggregateRoot, IElasticSearchable
    {
        IList<TEntity> QueryString(string term, Action operationToExecuteBeforeNextOperation = null);

        PagingTableResult<TEntity> GetAllPagedResult(string id, int startIndex, int pageSize, string sorting, Action operationToExecuteBeforeNextOperation = null);

        //TODO - Need to include the Fuzzy Search API
    }
}
