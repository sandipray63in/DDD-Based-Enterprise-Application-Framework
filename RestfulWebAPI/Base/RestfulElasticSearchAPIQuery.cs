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
    public class RestfulElasticSearchAPIQuery<TId,TEntity> : RestfulAPIQuery<TId,TEntity>
        where TId : struct
        where TEntity : BaseEntity<TId>, IQueryableAggregateRoot, IElasticSearchable
    {
        public RestfulElasticSearchAPIQuery(IElasticSearchableQueryableDomainServiceAsync<TId, TEntity> queryableDomainServiceAsync) : base(queryableDomainServiceAsync)
        {

        }

        public virtual async Task<IEnumerable<TEntity>> QueryStringAsync(string term, CancellationToken token = default(CancellationToken))
        {
            return await (_queryableDomainServiceAsync as IElasticSearchableQueryableDomainServiceAsync<TId, TEntity>).QueryStringAsync(term,token);
        }

        public virtual async Task<PagingTableResult<TEntity>> GetAllPagedResultAsync(string id, int startIndex, int pageSize, string sorting, CancellationToken token = default(CancellationToken))
        {
          return await (_queryableDomainServiceAsync as IElasticSearchableQueryableDomainServiceAsync<TId, TEntity>).GetAllPagedResultAsync(id,startIndex,pageSize,sorting, token);
        }

        //TODO - Need to include the Fuzzy Search API
    }
}