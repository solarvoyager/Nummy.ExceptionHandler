namespace Nummy.ExceptionHandler.Utils.Exceptions;

internal class HandleExceptionValidationException()
    : NummyExceptionHandlerException($"When {nameof(NummyExceptionHandlerOptions.HandleException)} is {bool.TrueString}, you should define value of {nameof(NummyExceptionHandlerOptions.Response)} because it will be used as response body. If you want to use default response (full exception), set {nameof(NummyExceptionHandlerOptions.HandleException)} to {bool.FalseString}.");