using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Base;
using Domain.Base.Entities;
using Domain.Base.Aggregates;
using DomainServices.Base.ElasticSearchableDomainServices.Queries;
using Repository;

namespace RestfulWebAPI.Base
{
    public class RestfulElasticSearchAPIQuery<TEntity, TId> : RestfulAPIQuery<TEntity, TId>
        where TEntity : BaseEntity<TId>, IQueryableAggregateRoot, IElasticSearchable
        where TId : struct
    {
        public RestfulElasticSearchAPIQuery(IElasticSearchableQueryableDomainServiceAsync<TEntity, TId> queryableDomainServiceAsync) : base(queryableDomainServiceAsync)
        {

        }

        public virtual async Task<IEnumerable<TEntity>> QueryStringAsync(string term, CancellationToken token = default(CancellationToken))
        {
            return await (_queryableDomainServiceAsync as IElasticSearchableQueryableDomainServiceAsync<TEntity, TId>).QueryStringAsync(term,token);
        }

        public virtual async Task<PagingTableResult<TEntity>> GetAllPagedResultAsync(string id, int startIndex, int pageSize, string sorting, CancellationToken token = default(CancellationToken))
        {
          return await (_queryableDomainServiceAsync as IElasticSearchableQueryableDomainServiceAsync<TEntity, TId>).GetAllPagedResultAsync(id,startIndex,pageSize,sorting, token);
        }

        //TODO - Need to include the Fuzzy Search API
    }
}