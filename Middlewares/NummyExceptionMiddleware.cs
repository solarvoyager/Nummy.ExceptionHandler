using Microsoft.Extensions.Options;
using Nummy.ExceptionHandler.Data.Entitites;
using Nummy.ExceptionHandler.Data.Services;
using Nummy.ExceptionHandler.Utils;

namespace Nummy.ExceptionHandler.Middlewares;

internal sealed class NummyExceptionMiddleware
{
    private readonly NummyExceptionHandlerOptions _handlerOptions;
    private readonly INummyCodeLoggerService _loggerService;
    private readonly RequestDelegate _next;

    public NummyExceptionMiddleware(RequestDelegate next, IOptions<NummyExceptionHandlerOptions> options,
        INummyCodeLoggerService loggerService)
    {
        _handlerOptions = options.Value;
        _loggerService = loggerService;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            if (!_handlerOptions.HandleException) throw;

            await _loggerService.LogAsync(NummyCodeLogLevel.Error, exception);
            await HandleExceptionAsync(context);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context)
    {
        context.Response.StatusCode = (int)_handlerOptions.ResponseStatusCode;
        await context.Response.WriteAsJsonAsync(_handlerOptions.Response);
    }
}