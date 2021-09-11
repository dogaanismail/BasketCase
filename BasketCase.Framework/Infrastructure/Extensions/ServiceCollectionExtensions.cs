﻿using BasketCase.Business.Configuration.Common;
using BasketCase.Core.Attributes;
using BasketCase.Core.Configuration;
using BasketCase.Core.Infrastructure;
using BasketCase.Domain.Enumerations;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Net;

namespace BasketCase.Framework.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add services to the application and configure service provider
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        /// <param name="webHostEnvironment">Hosting environment</param>
        /// <returns>Configured service provider</returns>
        public static (IEngine, AppConfig) ConfigureApplicationServices(this IServiceCollection services,
            IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.SystemDefault;

            CommonHelper.DefaultFileProvider = new SystemFileProvider(webHostEnvironment);

            services.AddHttpContextAccessor();

            var appConfigs = new AppConfig();
            configuration.Bind(appConfigs);
            services.AddSingleton(appConfigs);
            AppConfigsHelper.SaveAppSettings(appConfigs);

            var engine = EngineContext.Create();

            engine.ConfigureServices(services, configuration);
            engine.RegisterDependencies(services, appConfigs);

            return (engine, appConfigs);
        }

        /// <summary>
        /// Register httpContextAccessor
        /// </summary>
        /// <param name="services"></param>
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        /// <summary>
        /// Register mvc configurations
        /// </summary>
        /// <param name="services"></param>
        public static void AddSystemMvc(this IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("basketcase-policy", b =>
            {
                b.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            services.AddMvc(opt =>
            {
                opt.EnableEndpointRouting = false;
                opt.Filters.Add(typeof(ValidateModelAttribute));
            }).SetCompatibilityVersion(CompatibilityVersion.Latest)
            .AddFluentValidation(fvc => { });
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
