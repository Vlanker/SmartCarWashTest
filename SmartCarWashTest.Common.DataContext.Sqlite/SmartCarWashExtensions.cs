using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SmartCarWashTest.Common.DataContext.Sqlite
{
    public static class SmartCarWashExtensions
    {
        /// <summary>
        /// Adds SmartCarWashContext to the specified IServiceCollection. Uses the Sqlite database provider.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="relativePath">Set to override the default of "..".</param>
        /// <returns>An IServiceCollection that can be used to add more services.</returns>
        public static IServiceCollection AddSmartCarWashContext(
            this IServiceCollection services, string relativePath = "..")
        {
            var databasePath = Path.Combine(relativePath, ProjectConstants.DbName);

            services.AddDbContext<SmartCarWashContext>(options =>
            {
                options.UseSqlite($"Filename={databasePath}",
                    builder => builder.MigrationsAssembly(ProjectConstants.MigrationProject));
                options.UseLazyLoadingProxies();
            });

            return services;
        }
    }
}