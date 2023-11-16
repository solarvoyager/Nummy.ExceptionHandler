using System.Net;

namespace Nummy.ExceptionHandler.Models
{
    public class NummyExceptionOptions
    {
        public bool UseCustomResponse { get; set; }
        public object Response { get; set; }
        public string ResponseContentType { get; set; } = "application/json";
        public HttpStatusCode ResponseStatusCode { get; set; } = HttpStatusCode.BadRequest;
    }
}
