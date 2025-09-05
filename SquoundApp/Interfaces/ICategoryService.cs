using Shared.DataTransfer;
using Shared.Logging;


namespace SquoundApp.Interfaces
{
    public interface ICategoryService
    {
        /// <summary>
        /// Asynchronously retrieves a list of categories from a REST API.
        /// </summary>
        public Task<Result<List<CategoryDto>>> GetDataAsync();
    }
}
