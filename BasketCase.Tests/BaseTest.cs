using AutoMapper;
using BasketCase.Business.Events;
using BasketCase.Business.Interfaces.Basket;
using BasketCase.Business.Interfaces.Configuration;
using BasketCase.Business.Interfaces.Logging;
using BasketCase.Business.Interfaces.Product;
using BasketCase.Business.Services.Configuration;
using BasketCase.Business.Services.Logging;
using BasketCase.Business.Services.Product;
using BasketCase.Business.Services.ShoppingCart;
using BasketCase.Core;
using BasketCase.Core.Caching;
using BasketCase.Core.Configuration;
using BasketCase.Core.Configuration.Settings;
using BasketCase.Core.Events;
using BasketCase.Core.Infrastructure;
using BasketCase.Core.Infrastructure.Mapper;
using BasketCase.Repository.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
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
            var httpContext = new DefaultHttpContext
            {
                Request = { Headers = { { HeaderNames.Host, TestsDefaults.HostIpAddress } } }
            };

            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            httpContextAccessor.Setup(p => p.HttpContext).Returns(httpContext);

            services.AddSingleton(httpContextAccessor.Object);

            var actionContextAccessor = new Mock<IActionContextAccessor>();
            actionContextAccessor.Setup(x => x.ActionContext)
                .Returns(new ActionContext(httpContext, httpContext.GetRouteData(), new ActionDescriptor()));

            services.AddSingleton(actionContextAccessor.Object);

            var urlHelperFactory = new Mock<IUrlHelperFactory>();
            var urlHelper = new TestUrlHelper(actionContextAccessor.Object.ActionContext);

            urlHelperFactory.Setup(x => x.GetUrlHelper(It.IsAny<ActionContext>()))
                .Returns(urlHelper);

            services.AddTransient(provider => actionContextAccessor.Object);

            services.AddSingleton(urlHelperFactory.Object);


            services.AddSingleton<ITypeFinder>(typeFinder);

            services.AddTransient<ISystemFileProvider, SystemFileProvider>();

            services.AddSingleton<IMemoryCache>(memoryCache);
            services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();
            services.AddSingleton<ILocker, MemoryCacheManager>();

            services.AddTransient(typeof(IRepository<>), typeof(RepositoryBase<>));
           
            services.AddTransient<IWebHelper, WebHelper>();

            #region Services Dependency Registrations
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<ILogService, LogService>();
            services.AddTransient<IProductVariantService, ProductVariantService>();
            services.AddTransient<IShoppingCartService, ShoppingCartService>();
            services.AddTransient<ISettingService, SettingService>();
            #endregion

            services.AddSingleton<IEventPublisher, EventPublisher>();

            #region Consumer Dependency Registrations
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
                foreach (var findInterface in consumer.FindInterfaces((type, criteria) =>
                {
                    var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                    return isMatch;
                }, typeof(IConsumer<>)))
                    services.AddTransient(findInterface, consumer);

            #endregion

            #region Setting Dependency Registrations

            var settings = typeFinder.FindClassesOfType(typeof(ISettings), false).ToList();
            foreach (var setting in settings)
                services.AddTransient(setting,
                    context => context.GetRequiredService<ISettingService>().LoadSettingAsync(setting).Result);

            #endregion

            #region Mapper Configurations

            var mapperConfigurations = typeFinder.FindClassesOfType<IOrderedMapperProfile>();         
            var instances = mapperConfigurations
                .Select(mapperConfiguration => (IOrderedMapperProfile)Activator.CreateInstance(mapperConfiguration))
                .OrderBy(mapperConfiguration => mapperConfiguration.Order);

            var config = new MapperConfiguration(cfg =>
            {
                foreach (var instance in instances)
                {
                    cfg.AddProfile(instance.GetType());
                }
            });

            AutoMapperConfiguration.Init(config);

            #endregion

            _serviceProvider = services.BuildServiceProvider();

            EngineContext.Replace(new DevTestEngine(_serviceProvider));
        }

        public T GetService<T>()
        {
            try
            {
                return _serviceProvider.GetRequiredService<T>();
            }
            catch (InvalidOperationException ex)
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

        protected class TestUrlHelper : UrlHelperBase
        {
            public TestUrlHelper(ActionContext actionContext) : base(actionContext)
            {
            }

            public override string Action(UrlActionContext actionContext)
            {
                return string.Empty;
            }

            public override string RouteUrl(UrlRouteContext routeContext)
            {
                return string.Empty;
            }
        }
    }
}
