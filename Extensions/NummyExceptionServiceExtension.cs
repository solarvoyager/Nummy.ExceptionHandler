using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Nummy.ExceptionHandler.Middlewares;
using Nummy.ExceptionHandler.Models;
using Nummy.ExceptionHandler.Utils;

namespace Nummy.ExceptionHandler.Extensions;

public static class NummyExceptionServiceExtension
{
    public static IServiceCollection AddNummyExceptionHandler(this IServiceCollection services,
        Action<NummyExceptionOptions> options)
    {
        var exceptionOptions = new NummyExceptionOptions();
        options.Invoke(exceptionOptions);

        NummyModelValidator.ValidateNummyExceptionOptions(exceptionOptions);

        services.Configure(options);

        return services;
    }

    public static void UseNummyExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<NummyExceptionMiddleware>();
    }
}