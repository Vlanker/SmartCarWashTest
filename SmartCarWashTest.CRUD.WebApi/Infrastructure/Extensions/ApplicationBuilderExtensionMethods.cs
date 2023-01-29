using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SmartCarWashTest.Common.DataContext.Sqlite.Contexts;
using SmartCarWashTest.Common.DataContext.Sqlite.Initializers;
using SmartCarWashTest.CRUD.WebApi.DTOs.Events;
using SmartCarWashTest.CRUD.WebApi.Repositories.IntegrationEvents.EventHandlers;
using SmartCarWashTest.EventBusRabbitMq.EventBuses;

namespace SmartCarWashTest.CRUD.WebApi.Infrastructure.Extensions
{
    public static class ApplicationBuilderExtensionMethods
    {
        public static IApplicationBuilder ConfigureAuth(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }

        public static IApplicationBuilder ConfigureEventBus(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();

            // add a Subscribe here.
            // Example:
            // eventBus.Subscribe<SalePublishedIntegrationEvent, SaleCreateIntegrationEventHandler>();

            eventBus.Subscribe<SalePublishedIntegrationEvent, SaleCreateIntegrationEventHandler>();

            return app;
        }

        public static IApplicationBuilder InitializeDbContext(this IApplicationBuilder app, ILogger logger)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<SmartCarWashContext>();
            var deleted = context.Database.EnsureDeleted();
            logger.LogInformation("Database deleted: {Deleted}", deleted);
            var created = context.Database.EnsureCreated();
            logger.LogInformation("Database created: {Created}", created);
            logger.LogInformation("SQL script used to create database:");
            logger.LogInformation("{Script}", context.Database.GenerateCreateScript());
            DbInitialize.Initialize(context);

            return app;
        }
    }
}