using BasketCase.Core.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BasketCase.Core.Infrastructure.DependencyManagement
{
    /// <summary>
    /// Dependency registrar interface
    /// </summary>
    public interface IDependencyRegistrar
    {
        /// <summary>
        /// Register services and interfaces
        /// </summary>
        /// <param name="services"></param>
        /// <param name="typeFinder"></param>
        /// <param name="config"></param>
        void Register(IServiceCollection services, ITypeFinder typeFinder, AppConfig appConfig);

        /// <summary>
        /// Gets order of this dependency registrar implementation
        /// </summary>
        int Order { get; }
    }
}
