using Domain.Base;
using System.Runtime.Caching;

namespace Infrastructure.Caching
{
    public class DomainMemoryCache<TEntity> : MemoryCache<TEntity>
        where TEntity : ICacheable
    {
        public DomainMemoryCache(CacheItemPolicy policy, string regionName = null) : base(policy,regionName)
        {

        }
    }
}
