namespace Nummy.ExceptionHandler.Data.Entitites;

internal class NummyCodeLog
{
    public string? TraceIdentifier { get; set; }
    public required NummyCodeLogLevel LogLevel { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public string? StackTrace { get; set; }
    public string? InnerException { get; set; }
    public string? ExceptionType { get; set; }
}