using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Nummy.ExceptionHandler.Middlewares;
using Nummy.ExceptionHandler.Models;
using Nummy.ExceptionHandler.Service;

namespace Nummy.ExceptionHandler.DependencyInjection
{
    public static class NummyExceptionServiceExtension
    {
        public static void AddNummyExceptionHander(this IServiceCollection services, Action<NummyExceptionOptions>? options = null)
        {
            services.AddScoped<ILogger, Logger>();

            if (options is not null)
                services.Configure(options);
        }

        public static void UsNummyExceptionHandler(IApplicationBuilder app)
        {
            app.UseMiddleware<NummyExceptionMiddleware>();
        }

    }
}
