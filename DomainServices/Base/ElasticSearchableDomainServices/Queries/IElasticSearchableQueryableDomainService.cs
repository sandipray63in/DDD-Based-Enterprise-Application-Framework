using System;
using System.Collections.Generic;
using DomainServices.Base.QueryableDomainServices;
using Domain.Base;
using Repository;
using Domain.Base.Aggregates;
using Domain.Base.Entities;

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
