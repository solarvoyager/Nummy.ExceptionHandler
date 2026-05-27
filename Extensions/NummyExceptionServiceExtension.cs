using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Nummy.ExceptionHandler.Data.Services;
using Nummy.ExceptionHandler.Middlewares;
using Nummy.ExceptionHandler.Utils;

namespace Nummy.ExceptionHandler.Extensions;

public static class NummyExceptionServiceExtension
{
    public static IServiceCollection AddNummyExceptionHandler(this IServiceCollection services,
        Action<NummyExceptionHandlerOptions> options)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(options);

        var exceptionHandlerOptions = new NummyExceptionHandlerOptions();
        options.Invoke(exceptionHandlerOptions);

        NummyValidators.ValidateNummyExceptionOptions(exceptionHandlerOptions);

        services.Configure(options);

        services.AddHttpContextAccessor();

        // Singleton is safe: IHttpContextAccessor resolves HttpContext via AsyncLocal<T>.
        // The context is read at call time inside LogAsync, not stored on this instance.
        services.AddSingleton<INummyCodeLoggerService, NummyCodeLoggerService>();

        services.AddHttpClient(NummyConstants.ClientName, config =>
        {
            config.BaseAddress = new Uri(exceptionHandlerOptions.NummyServiceUrl);
            config.Timeout = exceptionHandlerOptions.HttpClientTimeout;
            config.DefaultRequestHeaders.Clear();
        });

        return services;
    }

    public static void UseNummyExceptionHandler(this IApplicationBuilder app)
    {
        ArgumentNullException.ThrowIfNull(app);
        app.UseMiddleware<NummyExceptionMiddleware>();
    }
}
