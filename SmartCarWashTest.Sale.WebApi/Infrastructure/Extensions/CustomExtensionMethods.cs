using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using SmartCarWashTest.EventBus.EventBusSubscriptionManagers;
using SmartCarWashTest.EventBusRabbitMq.EventBuses;
using SmartCarWashTest.EventBusRabbitMq.PersistentConnections;
using SmartCarWashTest.EventBusRabbitMq.Settings;

namespace SmartCarWashTest.Sale.WebApi.Infrastructure.Extensions
{
    /// <summary>
    /// Custom Service extension methods.
    /// </summary>
    public static class CustomExtensionMethods
    {
        /// <summary>
        /// Add Jwt Bearer Auth.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection ConfigureAuthService(this IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer();
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            return services;
        }

        /// <summary>
        /// Add HealthChecks.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddCustomHealthChecks(this IServiceCollection services,
            IConfiguration configuration)
        {
            var eventBusSettings = configuration.GetSection(nameof(EventBusSettings)).Get<EventBusSettings>();
            var hcBuilder = services.AddHealthChecks();
            hcBuilder.AddCheck("self", () => HealthCheckResult.Healthy());
            hcBuilder
                .AddRabbitMQ(
                    $"amqp://{eventBusSettings.HostName}",
                    name: "sale-rabbitmqbus-check",
                    tags: new[] { "rabbitmqbus" });

            return services;
        }

        /// <summary>
        /// Add RabbitMQ EventBus.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddEventBus(this IServiceCollection services,
            IConfiguration configuration)
        {
            var eventBusSettings = configuration.GetSection(nameof(EventBusSettings)).Get<EventBusSettings>();

            services.AddSingleton<IRabbitMqPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<DefaultRabbitMqPersistentConnection>>();
                var factory = new ConnectionFactory()
                {
                    HostName = eventBusSettings.HostName,
                    DispatchConsumersAsync = true
                };

                if (eventBusSettings.Port != 0)
                {
                    factory.Port = eventBusSettings.Port;
                }

                if (!string.IsNullOrEmpty(eventBusSettings.UserName))
                {
                    factory.UserName = eventBusSettings.UserName;
                }

                if (!string.IsNullOrEmpty(eventBusSettings.Password))
                {
                    factory.Password = eventBusSettings.Password;
                }

                var retryCount = 5;
                if (eventBusSettings.RetryCount < 1)
                {
                    retryCount = eventBusSettings.RetryCount;
                }

                return new DefaultRabbitMqPersistentConnection(factory, logger, retryCount);
            });

            return services;
        }

        /// <summary>
        /// Register RabbitMQ EventBus.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection RegisterRabbitMqEventBus(this IServiceCollection services,
            IConfiguration configuration)
        {
            var eventBusSettings = configuration.GetSection(nameof(EventBusSettings)).Get<EventBusSettings>();

            services.AddSingleton<IEventBus, EventBusRabbitMq.EventBuses.EventBusRabbitMq>(sp =>
            {
                var subscriptionClientName = eventBusSettings.SubscriptionClientName;
                var rabbitMqPersistentConnection = sp.GetRequiredService<IRabbitMqPersistentConnection>();
                var logger = sp.GetRequiredService<ILogger<EventBusRabbitMq.EventBuses.EventBusRabbitMq>>();
                var eventBusSubscriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

                var retryCount = 5;
                if (eventBusSettings.RetryCount < 1)
                {
                    retryCount = eventBusSettings.RetryCount;
                }

                return new EventBusRabbitMq.EventBuses.EventBusRabbitMq(rabbitMqPersistentConnection, logger, sp,
                    eventBusSubscriptionsManager, subscriptionClientName, retryCount);
            });

            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

            // services.AddTransient<SaleCreateIntegrationEventHandler>();

            return services;
        }
    }
}