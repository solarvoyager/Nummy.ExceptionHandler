using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nummy.ExceptionHandler.Data.Entitites;
using Nummy.ExceptionHandler.Data.Services;
using Nummy.ExceptionHandler.Utils;

namespace Nummy.ExceptionHandler.Middlewares;

internal sealed class NummyExceptionMiddleware(
    RequestDelegate next,
    IOptions<NummyExceptionHandlerOptions> options,
    INummyCodeLoggerService loggerService,
    ILogger<NummyExceptionMiddleware> logger
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
            try
            {
                await loggerService.LogAsync(NummyCodeLogLevel.Error, exception);
            }
            catch (Exception logEx)
            {
                logger.LogDebug(logEx, "Failed to send exception log to Nummy service");
            }

            if (!options.Value.HandleException) throw;

            await HandleExceptionAsync(context);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context)
    {
        if (context.Response.HasStarted) return;

        context.Response.StatusCode = (int)options.Value.ResponseStatusCode;
        await context.Response.WriteAsJsonAsync(options.Value.Response);
    }
}
