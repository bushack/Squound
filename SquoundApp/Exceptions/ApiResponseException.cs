

namespace SquoundApp.Exceptions
{
    public class ApiResponseException : Exception
    {
        public ApiResponseException(string message) : base(message) { }

        public ApiResponseException(string message, Exception inner) : base(message, inner) { }
    }
}
