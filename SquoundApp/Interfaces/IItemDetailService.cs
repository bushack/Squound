using Shared.DataTransfer;
using Shared.Logging;


namespace SquoundApp.Interfaces
{
    public interface IItemDetailService
    {
        /// <summary>
        /// Asynchronously attempts to retrieve a single ItemDetailDto object from a REST API.
        /// </summary>
        /// <param name="searchContext">Database query parameters, for filtering search results.</param>
        public Task<Result<ItemDetailDto>> GetDataAsync(ISearchContext searchContext);
    }
}
