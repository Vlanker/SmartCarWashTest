using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SmartCarWashTest.Sale.WebApi.Infrastructure.Extensions
{
    /// <summary>
    /// SwaggerGen extension Methods.
    /// </summary>
    public static class SwaggerGenExtensionMethods
    {
        /// <summary>
        /// Add Map Types.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static SwaggerGenOptions AddMapTypes(this SwaggerGenOptions options)
        {
            options.MapType<TimeSpan>(() =>
                new OpenApiSchema { Type = "string", Format = "time-span", Description = "00:00" });

            return options;
        }

        /// <summary>
        /// Add Swagger Doc.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static SwaggerGenOptions AddSwaggerDoc(this SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "SmartCarWashTest - CRUD WebApi HTTP API",
                Version = "v1",
                Description = "The CRUD WebApi Service HTTP API"
            });

            var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

            return options;
        }

        /// <summary>
        /// Add Bearer Security.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static SwaggerGenOptions AddBearerSecurity(this SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the bearer scheme." +
                              "\r\n\r\n" +
                              "Enter 'Bearer' [space] and then your token in the text input below." +
                              "\r\n\r\n" +
                              "Example: \"Bearer 12345abcdef\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme,
                        }
                    },
                    new List<string>()
                }
            });

            return options;
        }
    }
}