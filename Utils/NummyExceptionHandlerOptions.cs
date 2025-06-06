using System.Net;

namespace Nummy.ExceptionHandler.Utils;

public class NummyExceptionHandlerOptions
{
    public bool HandleException { get; set; }
    public object? Response { get; set; }
    public HttpStatusCode ResponseStatusCode { get; set; } = HttpStatusCode.InternalServerError;
    public string ApplicationId { get; set; }
    public string NummyServiceUrl { get; set; }
}