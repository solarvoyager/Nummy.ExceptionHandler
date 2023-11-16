using Microsoft.Extensions.Options;
using Nummy.ExceptionHandler.Models;

namespace Nummy.ExceptionHandler.Service
{
    internal class Logger : ILogger
    {
        private readonly NummyExceptionOptions _options;

        public Logger(IOptions<NummyExceptionOptions> options)
        {
            _options = options.Value;
        }

        public void LogCustom(LogLevel logLevel, string? title = null, string? description = null, System.Exception? exception = null)
        {
            throw new NotImplementedException();
        }

        public void LogDebug(string? title = null, string? description = null, System.Exception? exception = null)
        {
            throw new NotImplementedException();
        }

        public void LogError(string? title = null, string? description = null, System.Exception? exception = null)
        {
            throw new NotImplementedException();
        }

        public void LogInfo(string? title = null, string? description = null, System.Exception? exception = null)
        {
            throw new NotImplementedException();
        }
    }
}
