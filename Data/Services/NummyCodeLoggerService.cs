using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Nummy.ExceptionHandler.Data.Entitites;
using Nummy.ExceptionHandler.Utils;

namespace Nummy.ExceptionHandler.Data.Services;

internal class NummyCodeLoggerService(
    IHttpClientFactory clientFactory,
    IHttpContextAccessor contextAccessor,
    IOptions<NummyExceptionHandlerOptions> options,
    ILogger<NummyCodeLoggerService> logger)
    : INummyCodeLoggerService
{
    public async Task LogAsync(NummyCodeLogLevel logLevel, Exception ex)
    {
        var data = new NummyCodeLog
        {
            TraceIdentifier = contextAccessor.HttpContext?.TraceIdentifier,
            ApplicationId = options.Value.ApplicationId,
            LogLevel = logLevel,
            Title = ex.Message,
            StackTrace = ex.StackTrace,
            InnerException = ex.InnerException?.ToString(),
            ExceptionType = ex.GetType().FullName,
            Description = ex.ToString(),
        };

        await InsertLogAsync(data);
    }

    private async Task InsertLogAsync(NummyCodeLog data)
    {
        try
        {
            using var client = clientFactory.CreateClient(NummyConstants.ClientName);
            await client.PostAsJsonAsync(NummyConstants.CodeLogAddUrl, data);
        }
        catch (Exception ex)
        {
            logger.LogDebug(ex, "Failed to send exception log to Nummy service");
        }
    }
}
