using FSL.Framework.Core.Caching;
using FSL.Framework.Core.Caching.Provider;
using FSL.Framework.Core.Caching.Service;
using FSL.Framework.Web.Caching.Provider;
using Microsoft.Extensions.DependencyInjection;

namespace FSL.Framework.Web.Caching
{
    public static class CachingExtension
    {
        public static IServiceCollection UseCaching(
            this IServiceCollection services)
        {
            services.AddSingleton<ICacheProvider, MemoryCacheProvider>();
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<IAsyncCacheService, CacheService>();

            return services;
        }
    }
}
