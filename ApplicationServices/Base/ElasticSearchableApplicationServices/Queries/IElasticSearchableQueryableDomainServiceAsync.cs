using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ApplicationServices.Base.QueryableApplicationServices;
using Domain.Base;
using Repository;

namespace ApplicationServices.Base.ElasticSearchableDomainServices.Queries
{
    public interface IElasticSearchableQueryableDomainServiceAsync<TEntity> : IQueryableApplicationServiceAsync<TEntity> where TEntity : BaseIdentityAndAuditableQueryableAggregateRoot, IElasticSearchable
    {
        Task<IList<TEntity>> QueryStringAsync(string term, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);

        Task<PagingTableResult<TEntity>> GetAllPagedResultAsync(string id, int startIndex, int pageSize, string sorting, CancellationToken token = default(CancellationToken), Action operationToExecuteBeforeNextOperation = null);

        //TODO - Need to include the Fuzzy Search API

    }
}
