using Nummy.ExceptionHandler.Models;

namespace Nummy.ExceptionHandler.Service
{
    internal interface ILogger
    {
        void LogError(string? title = null, string? description = null, System.Exception? exception = null);
        void LogDebug(string? title = null, string? description = null, System.Exception? exception = null);
        void LogInfo(string? title = null, string? description = null, System.Exception? exception = null);
        void LogCustom(LogLevel logLevel, string? title = null, string? description = null, System.Exception? exception = null);
    }
}
