namespace Nummy.ExceptionHandler.Utils.Exceptions;

internal class ApplicationIdValidationException()
    : NummyExceptionHandlerException($"{nameof(NummyExceptionHandlerOptions.ApplicationId)} must have a valid Guid value. Make sure it is copied from Nummy.");