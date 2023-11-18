using System.Net;

namespace Nummy.ExceptionHandler.Models;

public class NummyExceptionOptions
{
    public bool ReturnResponseDuringException { get; set; }
    public object? Response { get; set; }
    public NummyResponseContentType ResponseContentType { get; set; } = NummyResponseContentType.Json;
    public HttpStatusCode ResponseStatusCode { get; set; } = HttpStatusCode.BadRequest;
}