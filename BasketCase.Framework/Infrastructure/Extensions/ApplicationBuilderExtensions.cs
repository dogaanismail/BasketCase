using BasketCase.Business.Interfaces.Logging;
using BasketCase.Core.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Net;

namespace BasketCase.Framework.Infrastructure.Extensions
{
    /// <summary>
    /// Represents extensions of IApplicationBuilder
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configure the application HTTP request pipeline
        /// </summary>
        /// <param name="application"></param>
        public static void ConfigureRequestPipeline(this IApplicationBuilder application)
        {
            EngineContext.Current.ConfigureRequestPipeline(application);
        }

        /// <summary>
        /// Configure swagger
        /// </summary>
        /// <param name="application"></param>
        public static void UseSystemSwagger(this IApplicationBuilder application)
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

        /// <summary>
        /// Configure environment
        /// </summary>
        /// <param name="application"></param>
        public static void UseSystemEnvironment(this IApplicationBuilder application)
        {
            var env = EngineContext.Current.Resolve<IWebHostEnvironment>();

            if (env.IsDevelopment())
                application.UseDeveloperExceptionPage();

            else
                application.UseHsts();
        }

        /// <summary>
        /// Configure routing
        /// </summary>
        /// <param name="application"></param>
        public static void UseSystemRouting(this IApplicationBuilder application)
        {
            application.UseRouting();
        }

        /// <summary>
        /// Configure exception handling
        /// </summary>
        /// <param name="application"></param>
        public static void UseSystemExceptionHandler(this IApplicationBuilder application)
        {
            var webHostEnvironment = EngineContext.Current.Resolve<IWebHostEnvironment>();

            application.UseExceptionHandler(handler =>
            {
                handler.Run(async context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    if (exception == null)
                        return;

                    try
                    {
                        await EngineContext.Current.Resolve<ILogService>().ErrorAsync(exception.Message, exception);
                    }
                    finally
                    {
                        var code = HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";
                        context.Response.StatusCode = (int)code;
                        await context.Response.WriteAsync(exception.Message);
                    }
                });
            });
        }

        /// <summary>
        /// Configure static file serving
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseSystemStaticFiles(this IApplicationBuilder application)
        {
            application.UseHttpsRedirection().UseResponseCompression();
        }

        /// <summary>
        /// Configure systerm end point
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public static void UseSystemEndPoint(this IApplicationBuilder application)
        {
            application.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
