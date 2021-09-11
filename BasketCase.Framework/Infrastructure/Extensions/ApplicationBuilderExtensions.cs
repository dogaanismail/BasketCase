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
                        //if (await DataSettingsManager.IsDatabaseInstalledAsync())
                        //    await EngineContext.Current.Resolve<ILogService>().ErrorAsync(exception.Message, exception);
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
    }
}
