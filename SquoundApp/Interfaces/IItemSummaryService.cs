using Shared.DataTransfer;
using Shared.Logging;


namespace SquoundApp.Interfaces
{
    public interface IItemSummaryService
    {
        /// <summary>
        /// Asynchronously attempts to retrieve a list of ItemSummaryDto objects from a REST API.
        /// </summary>
        /// <param name="searchContext">Database query parameters, for filtering search results.</param>
        /// <returns>Object containing a list of ItemSummaryDto objects matching the query parameters.</returns>
        /// <remarks>The return object can be queried for metadata and pagination info.</remarks>
        public Task<Result<SearchResponseDto<ItemSummaryDto>>> GetDataAsync(ISearchContext searchContext);
    }
}
