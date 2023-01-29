using System;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SmartCarWashTest.Common.Converters.JsonConverters;
using SmartCarWashTest.Common.DataContext.Sqlite.Extensions;
using SmartCarWashTest.Common.DataContext.Sqlite.Settings;
using SmartCarWashTest.CRUD.WebApi.Infrastructure.Extensions;
using SmartCarWashTest.CRUD.WebApi.Infrastructure.Filters;
using SmartCarWashTest.CRUD.WebApi.Infrastructure.Middlewares.Headers;
using SmartCarWashTest.CRUD.WebApi.Repositories.Extensions;

namespace SmartCarWashTest.CRUD.WebApi
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
            // AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            
            services.RegisterAppInsights(Configuration);

            services
                .AddControllers(options =>
                {
                    options.Filters.Add(typeof(HttpGlobalExceptionFilter));
                    options.Filters.Add(typeof(ValidateModelStateFilter));
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IncludeFields = true; // includes all fields
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
                    options.JsonSerializerOptions.Converters.Add(new DateTimeToDateConverter());
                })
                .AddXmlSerializerFormatters()
                .AddXmlDataContractSerializerFormatters();

            services.AddSwaggerGen(options =>
            {
                options.AddMapTypes();
                options.AddSwaggerDoc();
                options.AddJwtBearerApiSecurity();
            });

            services.ConfigureAuthService();

            services.AddCustomHealthChecks(Configuration);

            services.AddEventBus(Configuration);
            services.RegisterEventBus(Configuration);

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddCustomMiddlewares();

            // Context
            var contextSettings = Configuration.Get<ContextSettings>();
            services.AddSmartCarWashContext(contextSettings.DbRelativePath);
            services.AddRepositories();
            
            services.AddOptions();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            app.UseCors("CorsPolicy");


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"/swagger/v1/swagger.json", "SmartCarWashTest.CRUD.WebApi v1");
                    c.OAuthClientId("crudswaggerui");
                    c.OAuthAppName("CRUD WebApi Swagger UI");
                });
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI(setup =>
                {
                    setup.SwaggerEndpoint($"/swagger/v1/swagger.json", "SmartCarWashTest.CRUD.WebApi v1");
                    setup.OAuthClientId("crudswaggerui");
                    setup.OAuthAppName("CRUD WebApi Swagger UI");
                });
            }

            app.UseRouting();

            if (env.IsDevelopment())
            {
                app.InitializeDbContext(logger);
            }

            app.ConfigureAuth();

            app.UseEndpoints(endpoints => { endpoints.ConfigureEndpoints(); });


            app.UseMiddleware<SecurityHeaders>();
            app.ConfigureEventBus();
        }
    }
}