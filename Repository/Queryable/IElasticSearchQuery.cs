using System.Collections.Generic;
using Domain.Base;

namespace Repository.Queryable
{
    public interface IElasticSearchQuery<TEntity> : IQuery<TEntity> where TEntity : BaseIdentityAndAuditableQueryableAggregateRoot, IElasticSearchable
    {
        IList<TEntity> QueryString(string term);

        PagingTableResult<TEntity> GetAllPagedResult(string id, int startIndex, int pageSize, string sorting);

        //TODO - Need to include the Fuzzy Search API
    }
}
