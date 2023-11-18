using Nummy.ExceptionHandler.Models;

namespace Nummy.ExceptionHandler.Utils;

public static class NummyModelValidator
{
    public static void ValidateNummyExceptionOptions(NummyExceptionOptions options)
    {
        if (options.ReturnResponseDuringException && options.Response is null)
            throw new NummyOptionsValidationException();
    }
}