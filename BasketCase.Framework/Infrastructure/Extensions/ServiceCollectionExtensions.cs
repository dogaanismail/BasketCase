using BasketCase.Core.Configuration;
using BasketCase.Core.Infrastructure;
using BasketCase.Domain.Enumerations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace BasketCase.Framework.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register httpContextAccessor
        /// </summary>
        /// <param name="services"></param>
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        /// <summary>
        /// Register swagger implementation
        /// </summary>
        /// <param name="services"></param>
        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BasketCase Api", Version = "1.0.0" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer", 
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });
            });
        }

        /// <summary>
        /// Adds services required for distributed cache
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        public static void AddDistributedCache(this IServiceCollection services)
        {
            var appSettings = Singleton<AppConfig>.Instance;
            var distributedCacheConfig = appSettings.DistributedCacheConfig;

            if (!distributedCacheConfig.Enabled)
                return;

            switch (distributedCacheConfig.DistributedCacheType)
            {
                case DistributedCacheType.Memory:
                    services.AddDistributedCache();
                    break;

                case DistributedCacheType.SqlServer:
                    services.AddDistributedSqlServerCache(options =>
                    {
                        options.ConnectionString = distributedCacheConfig.ConnectionString;
                        options.SchemaName = distributedCacheConfig.SchemaName;
                        options.TableName = distributedCacheConfig.TableName;
                    });
                    break;

                case DistributedCacheType.Redis:
                    services.AddStackExchangeRedisCache(options =>
                    {
                        options.Configuration = distributedCacheConfig.ConnectionString;
                    });
                    break;
            }
        }

        /// <summary>
        /// Adds behavior options
        /// </summary>
        /// <param name="services"></param>
        public static void AddBehaviorOptions(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
            });
        }
    }
}
