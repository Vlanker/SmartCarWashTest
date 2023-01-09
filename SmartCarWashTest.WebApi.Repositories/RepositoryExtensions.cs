using Microsoft.Extensions.DependencyInjection;
using SmartCarWashTest.WebApi.Repositories.Interfaces;
using SmartCarWashTest.WebApi.Repositories.Repositories;

namespace SmartCarWashTest.WebApi.Repositories
{
    public static class DataServiceExtensions
    {
        /// <summary>
        /// Adds repositories scope.
        /// </summary>
        /// <param name="services"></param>
        /// <returns>An IServiceCollection that can be used to add more services.</returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IBuyerCacheRepository, BuyerCacheRepository>();
            services.AddScoped<IProductCacheRepository, ProductCacheRepository>();
            services.AddScoped<IProvidedProductCacheRepository, ProvidedProductCacheRepository>();
            services.AddScoped<ISaleCacheRepository, SaleCacheRepository>();
            services.AddScoped<ISalesDataCacheRepository, SalesDataCacheRepository>();
            services.AddScoped<ISalesPointCacheRepository, SalesPointCacheRepository>();

            return services;
        }
    }
}