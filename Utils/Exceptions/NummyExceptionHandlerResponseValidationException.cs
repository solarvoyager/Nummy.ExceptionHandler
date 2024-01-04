namespace Nummy.ExceptionHandler.Utils.Exceptions;

internal class NummyExceptionHandlerResponseValidationException() : Exception(
    $"When {nameof(NummyExceptionHandlerOptions.HandleException)} is {bool.TrueString}, {nameof(NummyExceptionHandlerOptions.Response)} must have a value");