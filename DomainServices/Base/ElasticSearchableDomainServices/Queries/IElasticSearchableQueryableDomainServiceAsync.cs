using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Base.Aggregates;
using Domain.Base.Entities;
using DomainServices.Base.QueryableDomainServices;
using Repository;

namespace DomainServices.Base.ElasticSearchableDomainServices.Queries
{
    public interface IElasticSearchableQueryableDomainServiceAsync<TId,TEntity> : IQueryableDomainServiceAsync<TId,TEntity>
        where TId : struct
        where TEntity : BaseEntity<TId>, IQueryableAggregateRoot, IElasticSearchable
    {
        Task<IEnumerable<TEntity>> QueryStringAsync(string term, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);

        Task<PagingTableResult<TEntity>> GetAllPagedResultAsync(string id, int startIndex, int pageSize, string sorting, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);

        //TODO - Need to include the Fuzzy Search API

    }
}
