using System;
using System.Runtime.Caching;
using Infrastructure.Utilities;

namespace Infrastructure.Caching
{
    /// <summary>
    /// A wrapper class over .NET ObjectCache which is compliant with ICache interface.
    /// Although this class is currently nowhere used within the project but can be used in future if there is a need.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TObjectToCache"></typeparam>
    public class MemoryCache<TObjectToCache> : ICache<string, TObjectToCache>
    {
        // Gets a reference to the default MemoryCache instance. 
        private static readonly ObjectCache _cache = MemoryCache.Default;
        private CacheItemPolicy _policy = null;
        private string _regionName;

        public MemoryCache(CacheItemPolicy policy,string regionName = null)
        {
            ContractUtility.Requires<ArgumentNullException>(policy.IsNotNull(), "policy instance cannot be null");
            _policy = policy;
            _regionName = regionName;
        }

        public TObjectToCache this[string key]
        {
            get
            {
                return (TObjectToCache)_cache[key];
            }
            set
            {
                _cache[key] = value;
            }
        }

        public bool Contains(string key)
        {
            return _cache.Contains(key, _regionName);
        }

        public TObjectToCache Get(string key)
        {
            return (TObjectToCache)_cache.Get(key, _regionName);
        }

        public bool Add(string key, TObjectToCache value)
        {
            // Add inside cache 
            _cache.Set(key, value,_policy,_regionName);
            return true;
        }

        public bool Remove(string key)
        {
            _cache.Remove(key, _regionName);
            return true;
        }
    }
}
