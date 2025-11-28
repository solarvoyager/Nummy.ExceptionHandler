using Nummy.ExceptionHandler.Data.Entitites;

namespace Nummy.ExceptionHandler.Data.Services;

public interface INummyCodeLoggerService
{
    Task LogAsync(NummyCodeLogLevel logLevel, string applicationId, Exception ex);
}