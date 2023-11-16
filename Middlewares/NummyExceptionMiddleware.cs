using Microsoft.AspNetCore.Http;
using Nummy.ExceptionHandler.Models;
using System.Text.Json;

namespace Nummy.ExceptionHandler.Middlewares
{
    internal class NummyExceptionMiddleware
    {
        //private readonly ILogger _logger;
        private readonly NummyExceptionOptions _options;
        private readonly RequestDelegate _next;

        public NummyExceptionMiddleware(RequestDelegate next, NummyExceptionOptions options)
        {
            //_logger = logger;
            _next = next;
            _options = options;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                //_logger.LogError(exception: ex);

                if (_options.ReturnResponseDuringException)
                    await HandleExceptionAsync(httpContext, _options);
                else throw ex;
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, NummyExceptionOptions options)
        {
            context.Response.ContentType = options.ResponseContentType == NummyResponseContentType.Json
                ? "application/json"
                : "application/xml";

            context.Response.StatusCode = (int)options.ResponseStatusCode;

            await context.Response.WriteAsync(JsonSerializer.Serialize(options.Response));
        }
    }
}
