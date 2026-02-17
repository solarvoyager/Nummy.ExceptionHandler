namespace Nummy.ExceptionHandler.Utils.Exceptions;

internal class NummyServiceUrlValidationException()
    : NummyExceptionHandlerException($"{nameof(NummyExceptionHandlerOptions.NummyServiceUrl)} must have a valid Uri value. Make sure it is copied from Nummy.");