using Nummy.ExceptionHandler.Data.Entitites;
using Nummy.ExceptionHandler.Utils;

namespace Nummy.ExceptionHandler.Data.Services;

internal class NummyCodeLoggerService(IHttpClientFactory clientFactory, IHttpContextAccessor contextAccessor)
    : INummyCodeLoggerService
{
    private readonly HttpClient _client = clientFactory.CreateClient(NummyConstants.ClientName);

    public async Task LogAsync(NummyCodeLogLevel logLevel, string applicationId, Exception ex)
    {
        var data = new NummyCodeLog
        {
            TraceIdentifier = contextAccessor.HttpContext?.TraceIdentifier,
            ApplicationId = applicationId,
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
        await _client.PostAsJsonAsync(NummyConstants.CodeLogAddUrl, data);
    }
}