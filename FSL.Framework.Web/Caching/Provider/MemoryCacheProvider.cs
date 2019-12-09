using FSL.Framework.Core.Caching.Provider;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace FSL.Framework.Web.Caching.Provider
{
    public sealed class MemoryCacheProvider : ICacheProvider
    {
        private readonly IMemoryCache _memoryCache;

        public MemoryCacheProvider(
            IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public bool IsCacheNull => _memoryCache == null;

        public object Get(
            string key)
        {
            if (IsCacheNull)
            {
                return null;
            }

            return _memoryCache.Get(key);
        }

        public void Insert(
            string key, 
            object value, 
            DateTime absoluteExpiration, 
            TimeSpan slidingExpiration)
        {
            if (IsCacheNull)
            {
                return;
            }

            _memoryCache.Set(
                key,
                value,
                absoluteExpiration);
        }

        public object Remove(string key)
        {
            if (IsCacheNull)
            {
                return null;
            }

            _memoryCache.Remove(key);

            return null;
        }
    }
}
