using Nummy.ExceptionHandler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nummy.ExceptionHandler.Utils
{
    public class NummyOptionsValidationException : Exception
    {
        public NummyOptionsValidationException()
            : base($"When {nameof(NummyExceptionOptions.ReturnResponseDuringException)} is true, {nameof(NummyExceptionOptions.Response)} must have a value") { }
    }
}
