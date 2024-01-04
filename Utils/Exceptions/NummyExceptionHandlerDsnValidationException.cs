namespace Nummy.ExceptionHandler.Utils.Exceptions;

internal class NummyExceptionHandlerDsnValidationException()
    : Exception($"{nameof(NummyExceptionHandlerOptions.DsnUrl)} must have a valid DSN url");