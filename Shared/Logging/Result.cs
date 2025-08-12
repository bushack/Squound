

namespace Shared.Logging
{
    public record Result<T>(bool Success, T? Data, string? ErrorMessage = null)
    {
        /// <summary>
        /// Creates a new Result<T> object signifying the operation was successful.
        /// </summary>
        /// <param name="data">Payload of the operation.</param>
        /// <returns></returns>
        public static Result<T> Ok(T data) => new(true, data);

        /// <summary>
        /// Creates a new Result<T> object signifying the operation failed.
        /// </summary>
        /// <param name="errorMessage">Description of the reason for failure.</param>
        /// <returns></returns>
        public static Result<T> Fail(string errorMessage) => new(false, default, errorMessage);
    }
}
