using Nummy.ExceptionHandler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nummy.ExceptionHandler.Utils
{
    public static class NummyModelValidator
    {
        public static void ValidateNummyExceptionOptions(NummyExceptionOptions options) {

            if (options.ReturnResponseDuringException && options.Response is null)
                throw new NummyOptionsValidationException();
        }
    }
}
