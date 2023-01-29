using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SmartCarWashTest.Sale.WebApi.Infrastructure.Extensions
{
    /// <summary>
    /// Application insights Extension methods.
    /// </summary>
    public static class ApplicationInsightsExtensionMethods
    {
        /// <summary>
        /// Register application insights.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterAppInsights(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetry(configuration);
            services.AddApplicationInsightsKubernetesEnricher();
            
            return services;
        }
    }
}