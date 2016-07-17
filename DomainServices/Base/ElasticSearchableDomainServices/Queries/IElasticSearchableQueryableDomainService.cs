using System;
using System.Collections.Generic;
using Domain.Base;
using Domain.Base.Aggregates;
using Domain.Base.Entities;
using DomainServices.Base.QueryableDomainServices;
using Repository;

namespace DomainServices.Base.ElasticSearchableDomainServices.Queries
{
    public interface IElasticSearchableQueryableDomainService<TId, TEntity> : IQueryableDomainService<TId, TEntity>
        where TId : struct
        where TEntity : BaseEntity<TId>, IQueryableAggregateRoot, IElasticSearchable
    {
        IList<TEntity> QueryString(string term, Action operationToExecuteBeforeNextOperation = null);

        PagingTableResult<TEntity> GetAllPagedResult(string id, int startIndex, int pageSize, string sorting, Action operationToExecuteBeforeNextOperation = null);

        //TODO - Need to include the Fuzzy Search API
    }
}
