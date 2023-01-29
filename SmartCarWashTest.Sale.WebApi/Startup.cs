using System;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartCarWashTest.Common.Converters.JsonConverters;
using SmartCarWashTest.Sale.WebApi.Infrastructure.Extensions;
using SmartCarWashTest.Sale.WebApi.Infrastructure.Middlewares;

namespace SmartCarWashTest.Sale.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            services.RegisterAppInsights(Configuration);

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IncludeFields = true; // includes all fields
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
                    options.JsonSerializerOptions.Converters.Add(new DateTimeToDateConverter());
                })
                // .AddXmlDataContractSerializerFormatters()
                // .AddXmlSerializerFormatters()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSwaggerGen(options =>
            {
                options.AddMapTypes();
                options.AddSwaggerDoc();
                options.AddBearerSecurity();
            });

            services.ConfigureAuthService();

            services.AddCustomHealthChecks(Configuration);

            services.AddEventBus(Configuration);
            services.RegisterRabbitMqEventBus(Configuration);
            
            services.AddOptions();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
           

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"/swagger/v1/swagger.json", "SmartCarWashTest.Sale.WebApi v1");
                    c.OAuthClientId("saleswaggerui");
                    c.OAuthAppName("Sale WebApi Swagger UI");
                });
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI(setup =>
                {
                    setup.SwaggerEndpoint($"/swagger/v1/swagger.json", "SmartCarWashTest.Sale.WebApi v1");
                    setup.OAuthClientId("saleswaggerui");
                    setup.OAuthAppName("Sale WebApi Swagger UI");
                });
            }

            // app.UseHttpsRedirection();

            app.UseRouting();
            
            app.UseCors("CorsPolicy");
            
            app.ConfigureAuth();
            
            app.UseStaticFiles();

            app.UseEndpoints(endpoints => { endpoints.ConfigureEndpoints(); });

            app.UseMiddleware<SecurityHeaders>();
            app.ConfigureEventBus();
        }
    }
}