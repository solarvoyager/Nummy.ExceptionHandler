namespace Nummy.ExceptionHandler.Utils.Exceptions;

internal class NummyServiceUrlValidationException()
    : NummyExceptionHandlerException($"{nameof(NummyExceptionHandlerOptions.NummyServiceUrl)} must have a valid Uri value. Make sure to it copied from the Nummy.");