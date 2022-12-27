using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SmartCarWashTest.Common.DataContext.Sqlite
{
    public static class SmartCarWashExtensions
    {
        /// <summary>
        /// Adds NorthwindContext to the specified IServiceCollection. Uses the Sqlite database provider.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="relativePath">Set to override the default of ".."</param>
        /// <returns>An IServiceCollection that can be used to add more services.</returns>
        public static IServiceCollection AddNorthwindContext(
            this IServiceCollection services, string relativePath = "..")
        {
            var databasePath = Path.Combine(relativePath, "SmartCarWash.db");

            services.AddDbContext<SmartCarWashContext>(options =>
                options.UseSqlite($"Data Source={databasePath}")
            );

            return services;
        }
    }
}