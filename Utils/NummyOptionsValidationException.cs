using Nummy.ExceptionHandler.Models;

namespace Nummy.ExceptionHandler.Utils;

public class NummyOptionsValidationException : Exception
{
    public NummyOptionsValidationException()
        : base(
            $"When {nameof(NummyExceptionOptions.ReturnResponseDuringException)} is true, {nameof(NummyExceptionOptions.Response)} must have a value")
    {
    }
}