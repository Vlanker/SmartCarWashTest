using Microsoft.Extensions.DependencyInjection;
using SmartCarWashTest.CRUD.WebApi.Repositories.Abstractions;
using SmartCarWashTest.CRUD.WebApi.Repositories.Repositories;

namespace SmartCarWashTest.CRUD.WebApi.Repositories.Extensions
{
    /// <summary>
    /// Repository DataService extension methods.
    /// </summary>
    public static class RepositoryDataServiceExtensionMethods
    {
        /// <summary>
        /// Adds a scoped service of the repositories.
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