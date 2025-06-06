using Nummy.ExceptionHandler.Data.Services;
using Nummy.ExceptionHandler.Middlewares;
using Nummy.ExceptionHandler.Utils;

namespace Nummy.ExceptionHandler.Extensions;

public static class NummyExceptionServiceExtension
{
    public static IServiceCollection AddNummyExceptionHandler(this IServiceCollection services,
        Action<NummyExceptionHandlerOptions> options)
    {
        var exceptionHandlerOptions = new NummyExceptionHandlerOptions();
        options.Invoke(exceptionHandlerOptions);

        NummyValidators.ValidateNummyExceptionOptions(exceptionHandlerOptions);

        services.Configure(options);

        //services.AddExceptionHandler<NummyExceptionHandler>();
        services.AddProblemDetails();

        services.AddSingleton<INummyCodeLoggerService, NummyCodeLoggerService>();

        services.AddHttpClient(NummyConstants.ClientName, config =>
        {
            config.BaseAddress = new Uri(exceptionHandlerOptions.NummyServiceUrl);
            config.Timeout = new TimeSpan(0, 0, 30);
            config.DefaultRequestHeaders.Clear();
        });

        return services;
    }

    public static void UseNummyExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<NummyExceptionMiddleware>();
    }
}