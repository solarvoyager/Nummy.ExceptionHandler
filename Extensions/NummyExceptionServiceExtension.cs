using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Nummy.ExceptionHandler.Middlewares;
using Nummy.ExceptionHandler.Models;

namespace Nummy.ExceptionHandler.DependencyInjection
{
    public static class NummyExceptionServiceExtension
    {
        public static void AddNummyExceptionHander(this IServiceCollection services, Action<NummyExceptionOptions> options)
        {
            services.Configure(options);
        }

        public static void UsNummyExceptionHandler(IApplicationBuilder app)
        {
            app.UseMiddleware<NummyExceptionMiddleware>();
        }

    }
}
