using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SquoundApp.Exceptions
{
    public class HttpServiceException : Exception
    {
        public HttpServiceException(string message) : base(message) { }

        public HttpServiceException(string message, Exception inner) : base(message, inner) { }
    }
}
