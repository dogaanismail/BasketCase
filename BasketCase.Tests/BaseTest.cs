﻿using BasketCase.Business.Interfaces.Logging;
using BasketCase.Business.Interfaces.Product;
using BasketCase.Business.Services.Logging;
using BasketCase.Business.Services.Product;
using BasketCase.Core;
using BasketCase.Core.Caching;
using BasketCase.Core.Configuration;
using BasketCase.Core.Infrastructure;
using BasketCase.Repository.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Moq;
using System;
using System.IO;
using System.Linq;

namespace BasketCase.Tests
{
    /// <summary>
    /// Base test abstract class implementations
    /// </summary>
    public abstract class BaseTest
    {
        private static readonly ServiceProvider _serviceProvider;

        static BaseTest()
        {
            var services = new ServiceCollection();

            services.AddHttpClient();

            var typeFinder = new AppDomainTypeFinder();
            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            services.AddTransient<IActionContextAccessor, ActionContextAccessor>();

            var appSettings = new AppConfig();
            services.AddSingleton(appSettings);
            Singleton<AppConfig>.Instance = appSettings;

            var hostApplicationLifetime = new Mock<IHostApplicationLifetime>();
            services.AddSingleton(hostApplicationLifetime.Object);

            var rootPath =
              new DirectoryInfo(
                      $@"{Directory.GetCurrentDirectory().Split("bin")[0]}{Path.Combine(@"\..\BasketCase.Api".Split('\\', '/').ToArray())}")
                  .FullName;

            var webHostEnvironment = new Mock<IWebHostEnvironment>();
            webHostEnvironment.Setup(p => p.WebRootPath).Returns(Path.Combine(rootPath, "wwwroot"));
            webHostEnvironment.Setup(p => p.ContentRootPath).Returns(rootPath);
            webHostEnvironment.Setup(p => p.EnvironmentName).Returns("test");
            webHostEnvironment.Setup(p => p.ApplicationName).Returns("basketCase");
            services.AddSingleton(webHostEnvironment.Object);

            CommonHelper.DefaultFileProvider = new SystemFileProvider(webHostEnvironment.Object);
            var httpContextAccessor = new Mock<IHttpContextAccessor>();

            services.AddSingleton(httpContextAccessor.Object);

            var actionContextAccessor = new Mock<IActionContextAccessor>();

            services.AddSingleton(actionContextAccessor.Object);
            services.AddTransient(provider => actionContextAccessor.Object);
            services.AddSingleton<ITypeFinder>(typeFinder);

            services.AddTransient<ISystemFileProvider, SystemFileProvider>();

            services.AddSingleton<IMemoryCache>(memoryCache);
            services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();
            services.AddSingleton<ILocker, MemoryCacheManager>();

            services.AddTransient(typeof(IRepository<>), typeof(RepositoryBase<>));
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ILogService, LogService>();
            services.AddTransient<IProductVariantService, ProductVariantService>();

            services.AddTransient<IWebHelper, WebHelper>();

            _serviceProvider = services.BuildServiceProvider();

            EngineContext.Replace(new DevTestEngine(_serviceProvider));
        }

        public T GetService<T>()
        {
            try
            {
                return _serviceProvider.GetRequiredService<T>();
            }
            catch
            {
                return (T)EngineContext.Current.ResolveUnregistered(typeof(T));
            }
        }

        public partial class DevTestEngine : SystemEngine
        {
            protected readonly IServiceProvider _internalServiceProvider;

            public DevTestEngine(IServiceProvider serviceProvider)
            {
                _internalServiceProvider = serviceProvider;
            }

            public override IServiceProvider ServiceProvider => _internalServiceProvider;
        }
    }
}
