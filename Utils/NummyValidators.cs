using Nummy.ExceptionHandler.Utils.Exceptions;

namespace Nummy.ExceptionHandler.Utils;

internal static class NummyValidators
{
    public static void ValidateNummyExceptionOptions(NummyExceptionHandlerOptions handlerOptions)
    {
        if (handlerOptions is { HandleException: true, Response: null })
            throw new NummyExceptionHandlerResponseValidationException();

        if (string.IsNullOrWhiteSpace(handlerOptions.DsnUrl))
            throw new NummyExceptionHandlerDsnValidationException();
    }
}