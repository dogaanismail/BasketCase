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
using BasketCase.Core.Infrastructure.DependencyManagement;
using BasketCase.Repository.Generic;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace BasketCase.Framework.Infrastructure
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="typeFinder"></param>
        /// <param name="config"></param>
        public void Register(IServiceCollection services, ITypeFinder typeFinder, AppConfig appConfig)
        {
            services.AddScoped<ISystemFileProvider, SystemFileProvider>();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            services.AddScoped(typeof(IRepository<>), typeof(RepositoryBase<>));

            services.AddScoped<IWebHelper, WebHelper>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<IProductVariantService, ProductVariantService>();
            services.AddScoped<IShoppingCartService, ShoppingCartService>();
            services.AddScoped<ISettingService, SettingService>();

            services.AddSingleton<IEventPublisher, EventPublisher>();

            #region Caching implementations
            if (appConfig.DistributedCacheConfig.Enabled)
            {
                services.AddScoped<ILocker, DistributedCacheManager>();
                services.AddScoped<IStaticCacheManager, DistributedCacheManager>();
            }
            else
            {
                services.AddSingleton<ILocker, MemoryCacheManager>();
                services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();
            }

            #endregion
      
            #region Consumer Dependency Registrations
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
                foreach (var findInterface in consumer.FindInterfaces((type, criteria) =>
                {
                    var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                    return isMatch;
                }, typeof(IConsumer<>)))
                    services.AddScoped(findInterface, consumer);

            #endregion

            #region Setting Dependency Registrations
            if (appConfig.CommonConfig.RegisterSettings)
            {
                var settings = typeFinder.FindClassesOfType(typeof(ISettings), false).ToList();
                foreach (var setting in settings)
                {
                    services.AddScoped(setting, serviceProvider =>
                    {
                        return serviceProvider.GetRequiredService<ISettingService>().LoadSettingAsync(setting).Result;
                    });
                }
            }

            #endregion
        }

        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        public int Order => 0;
    }
}
