﻿using Nummy.ExceptionHandler.Models;

namespace Nummy.ExceptionHandler.Utils;

internal class NummyOptionsValidationException : Exception
{
    public NummyOptionsValidationException()
        : base(
            $"When {nameof(NummyExceptionOptions.ReturnResponseDuringException)} is true, {nameof(NummyExceptionOptions.Response)} must have a value")
    {
    }
}