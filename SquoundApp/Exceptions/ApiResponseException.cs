using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquoundApp.Exceptions
{
    public class ApiResponseException : Exception
    {
        public ApiResponseException(string message) : base(message) { }

        public ApiResponseException(string message, Exception inner) : base(message, inner) { }
    }
}
