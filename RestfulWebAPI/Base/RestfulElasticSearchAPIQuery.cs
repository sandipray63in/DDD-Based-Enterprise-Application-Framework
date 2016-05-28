using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DomainServices.Base.ElasticSearchableDomainServices.Queries;
using Domain.Base;
using Repository;

namespace RestfulWebAPI.Base
{
    public class RestfulElasticSearchAPIQuery<TEntity> : RestfulAPIQuery<TEntity> where TEntity : BaseIdentityAndAuditableQueryableAggregateRoot, IElasticSearchable
    {
        public RestfulElasticSearchAPIQuery(IElasticSearchableQueryableDomainServiceAsync<TEntity> queryableDomainServiceAsync) : base(queryableDomainServiceAsync)
        {

        }

        public virtual async Task<IList<TEntity>> QueryStringAsync(string term, CancellationToken token = default(CancellationToken))
        {
            return await (_queryableDomainServiceAsync as IElasticSearchableQueryableDomainServiceAsync<TEntity>).QueryStringAsync(term,token);
        }

        public virtual async Task<PagingTableResult<TEntity>> GetAllPagedResultAsync(string id, int startIndex, int pageSize, string sorting, CancellationToken token = default(CancellationToken))
        {
          return await (_queryableDomainServiceAsync as IElasticSearchableQueryableDomainServiceAsync<TEntity>).GetAllPagedResultAsync(id,startIndex,pageSize,sorting, token);
        }

        //TODO - Need to include the Fuzzy Search API
    }
}