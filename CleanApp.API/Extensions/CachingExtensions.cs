using App.Application.Contracts.Caching;
using App.Caching;

namespace CleanApp.API.Extensions
{
    public static class CachingExtensions
    {
        public static IServiceCollection AddMemoryCacheExt(this IServiceCollection services )
        {
            services.AddMemoryCache();
            
            services.AddSingleton<ICacheService, CacheService>();
            return services;
        }
    }
}
