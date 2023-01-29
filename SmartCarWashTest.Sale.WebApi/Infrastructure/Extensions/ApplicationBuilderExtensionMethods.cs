using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SmartCarWashTest.EventBusRabbitMq.EventBuses;

namespace SmartCarWashTest.Sale.WebApi.Infrastructure.Extensions
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

            return app;
        }
    }
}