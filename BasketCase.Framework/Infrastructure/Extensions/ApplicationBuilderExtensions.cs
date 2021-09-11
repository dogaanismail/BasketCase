using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace BasketCase.Framework.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IApplicationBuilder
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configure swagger
        /// </summary>
        /// <param name="application"></param>
        public static void UseSwagger(this IApplicationBuilder application)
        {
            application.UseSwagger();
            application.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BasketCase.Api V1");
                c.DocumentTitle = "Title";
                c.DisplayOperationId();
                c.DocExpansion(DocExpansion.None);
            });
        }
    }
}
