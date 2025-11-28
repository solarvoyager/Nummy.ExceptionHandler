using Microsoft.Extensions.Options;
using Nummy.ExceptionHandler.Data.Entitites;
using Nummy.ExceptionHandler.Data.Services;
using Nummy.ExceptionHandler.Utils;

namespace Nummy.ExceptionHandler.Middlewares;

internal sealed class NummyExceptionMiddleware(
    RequestDelegate next,
    IOptions<NummyExceptionHandlerOptions> options,
    INummyCodeLoggerService loggerService
)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            if (!options.Value.HandleException) throw;

            await loggerService.LogAsync(NummyCodeLogLevel.Error, options.Value.ApplicationId, exception);
            await HandleExceptionAsync(context);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context)
    {
        context.Response.StatusCode = (int)options.Value.ResponseStatusCode;
        await context.Response.WriteAsJsonAsync(options.Value.Response);
    }
}