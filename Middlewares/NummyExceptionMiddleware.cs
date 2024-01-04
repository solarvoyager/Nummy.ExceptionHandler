using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Options;
using Nummy.ExceptionHandler.Data.Entitites;
using Nummy.ExceptionHandler.Data.Services;
using Nummy.ExceptionHandler.Utils;

namespace Nummy.ExceptionHandler.Middlewares;

internal sealed class NummyExceptionHandler : IExceptionHandler
{
    private readonly NummyExceptionHandlerOptions _handlerOptions;
    private readonly INummyCodeLoggerService _loggerService;

    public NummyExceptionHandler(IOptions<NummyExceptionHandlerOptions> options, INummyCodeLoggerService loggerService)
    {
        _handlerOptions = options.Value;
        _loggerService = loggerService;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (!_handlerOptions.HandleException)
            return false;

        await _loggerService.LogAsync(NummyCodeLogLevel.Error, exception);

        httpContext.Response.StatusCode = (int)_handlerOptions.ResponseStatusCode;

        await httpContext.Response.WriteAsJsonAsync(_handlerOptions.Response, cancellationToken);

        return true;
    }
}