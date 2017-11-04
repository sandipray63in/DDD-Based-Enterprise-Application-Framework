using System.Collections.Generic;
using Domain.Base;
using Domain.Base.Aggregates;

namespace Repository.Queryable
{
    public interface IElasticSearchQuery<TEntity> : IQuery<TEntity> 
        where TEntity : IQueryableAggregateRoot,IElasticSearchable
    {
        IEnumerable<TEntity> QueryString(string term);

        PagingTableResult<TEntity> GetAllPagedResult(string id, int startIndex, int pageSize, string sorting);

        //TODO - Need to include the Fuzzy Search API
    }
}
