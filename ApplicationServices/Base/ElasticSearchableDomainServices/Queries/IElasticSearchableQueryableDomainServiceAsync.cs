using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DomainServices.Base.QueryableDomainServices;
using Domain.Base;
using Repository;

namespace DomainServices.Base.ElasticSearchableDomainServices.Queries
{
    public interface IElasticSearchableQueryableDomainServiceAsync<TEntity> : IQueryableDomainServiceAsync<TEntity> where TEntity : BaseIdentityAndAuditableQueryableAggregateRoot, IElasticSearchable
    {
        Task<IList<TEntity>> QueryStringAsync(string term, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);

        Task<PagingTableResult<TEntity>> GetAllPagedResultAsync(string id, int startIndex, int pageSize, string sorting, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);

        //TODO - Need to include the Fuzzy Search API

    }
}
