using System.Net;

namespace Nummy.ExceptionHandler.Models
{
    public class NummyExceptionOptions
    {
        public required bool ReturnResponseDuringException { get; set; }
        public required object Response { get; set; }
        public NummyResponseContentType ResponseContentType { get; set; } = NummyResponseContentType.Json;
        public HttpStatusCode ResponseStatusCode { get; set; } = HttpStatusCode.BadRequest;
    }
}
