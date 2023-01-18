using System;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SmartCarWashTest.Common.DataContext.Sqlite;
using SmartCarWashTest.Common.Converters.JsonConverters;
using SmartCarWashTest.WebApi.Middlewares;
using SmartCarWashTest.WebApi.Repositories;
using SmartCarWashTest.WebApi.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace SmartCarWashTest.WebApi
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
            services.AddCors();

            var databasePath = Configuration.GetConnectionString(ProjectConstants.ConnectionString);

            services.AddSmartCarWashContext(databasePath);

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddControllers(options =>
                {
                    Console.WriteLine("Default output formatters:");

                    foreach (var formatter in options.OutputFormatters)
                    {
                        if (formatter is not OutputFormatter mediaFormatter)
                        {
                            Console.WriteLine($"  {formatter.GetType().Name}");
                        }
                        else // OutputFormatter class has SupportedMediaTypes
                        {
                            Console.WriteLine("  {0}, Media types: {1}",
                                arg0: mediaFormatter.GetType().Name,
                                arg1: string.Join(", ", mediaFormatter.SupportedMediaTypes));
                        }
                    }
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.IncludeFields = true; // includes all fields
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
                })
                .AddXmlDataContractSerializerFormatters()
                .AddXmlSerializerFormatters()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            services.AddSwaggerGen(options =>
            {
                options.AddMapTypes();
                options.AddSwaggerDoc();
                options.AddBearerSecurity();
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .RequireAuthenticatedUser()
                    .Build();
            });

            services.AddHealthChecks()
                .AddDbContextCheck<SmartCarWashContext>();

            services.AddRepositories();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(configurePolicy: options =>
            {
                options.WithMethods("GET", "POST", "PUT", "DELETE");
                options.WithOrigins("https://localhost:5001"); // allow requests from the MVC client
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SmartCarWashTest.WebApi v1");
                    c.SupportedSubmitMethods(new[]
                    {
                        SubmitMethod.Get, SubmitMethod.Post,
                        SubmitMethod.Put, SubmitMethod.Delete
                    });
                });
            }

            if (env.IsDevelopment())
            {
                using var scope = app.ApplicationServices.CreateScope();
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<SmartCarWashContext>();
                var deleted = context.Database.EnsureDeleted();
                Console.WriteLine($"Database deleted: {deleted}");
                var created = context.Database.EnsureCreated();
                Console.WriteLine($"Database created: {created}");
                Console.WriteLine("SQL script used to create database:");
                Console.WriteLine(context.Database.GenerateCreateScript());
                DbInitialize.Initialize(context);
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            // must be after UseRouting and before UseEndpoints
            app.UseCors(configurePolicy: options =>
            {
                options.WithMethods("GET", "POST", "PUT", "DELETE");
                options.WithOrigins(
                    "https://localhost:5002" // for MVC client
                );
            });

            app.UseHealthChecks(path: "/howdoyoufeel");

            app.UseMiddleware<SecurityHeaders>();

            app.Use(next => (context) =>
            {
                var endpoint = context.GetEndpoint();
                if (endpoint != null)
                {
                    Console.WriteLine("*** Name: {0}; Route: {1}; Metadata: {2}",
                        arg0: endpoint.DisplayName,
                        arg1: (endpoint as RouteEndpoint)?.RoutePattern,
                        arg2: string.Join(", ", endpoint.Metadata));
                }

                // pass context to next middleware in pipeline
                return next(context);
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}