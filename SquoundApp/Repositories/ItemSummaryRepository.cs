using Microsoft.Extensions.Logging;

using SquoundApp.Exceptions;
using SquoundApp.Interfaces;

using Shared.DataTransfer;


namespace SquoundApp.Repositories
{
    public class ItemSummaryRepository : IItemSummaryRepository
    {
        private readonly ILogger<ItemSummaryRepository> _Logger;
        private readonly IItemService _Service;

        private SearchResponseDto<ItemSummaryDto> _Cache = new();


        public ItemSummaryRepository(ILogger<ItemSummaryRepository> logger, IItemService service)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _Service = service ?? throw new ArgumentNullException(nameof(service));
        }


        /// <summary>
        /// Provides asynchronous retrieval of item summaries based on the specified search context.
        /// This method interacts with an underlying item service to fetch data from a REST API.
        /// The data is paged and sorted according to the parameters defined in the search context.
        /// To query the pagination metadata (e.g., total items, total pages, current page, etc.),
        /// use the corresponding properties on this interface after calling this method.
        /// </summary>
        /// <param name="searchContext">Reference to a search context defining the query parameters.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task<IReadOnlyList<ItemSummaryDto>> GetItemsAsync(ISearchContext searchContext)
        {
            // Search context has not changed since last API call.
            if (searchContext.HasNotChanged)
            {
                _Logger.LogInformation("Search context unchanged. Returning cached data.");
                return _Cache.Items;
            }

            // Search context has changed - fetch new data.
            else
            {
                _Logger.LogInformation("Search context changed. Requesting new data.");

                // Fetch item summaries from API. Throw exception on failure.
                var result = await _Service.GetDataAsync(searchContext)
                    ?? throw new ItemRepositoryException("Item data not received.");

                // Extract item summaries from result. Throw exception on null.
                var data = result.Data
                    ?? throw new ItemRepositoryException("Item data is null.");

                // Cache item summaries for future use.
                _Cache = data;

                // Return item summaries.
                _Logger.LogInformation("Returning data for {Count} item(s).", _Cache.Items.Count);
                return _Cache.Items;
            }
        }


        /// <summary>
        /// Determines if the repository is empty.
        /// </summary>
        public bool IsEmpty => _Cache.TotalItems == 0;


        /// <summary>
        /// Determines if the repository is not empty.
        /// </summary>
        public bool IsNotEmpty => _Cache.TotalItems != 0;


        /// <summary>
        /// Determines if there is another page of items available.
        /// </summary>
        public bool HasNextPage => CurrentPage < TotalPages;


        /// <summary>
        /// Determines if there is a previous page of items available.
        /// </summary>
        public bool HasPrevPage => CurrentPage > 1;


        /// <summary>
        /// The total number of items available based on the most recent search.
        /// </summary>
        public int TotalItems => _Cache.TotalItems;


        /// <summary>
        /// The total number of pages available based on the most recent search.
        /// </summary>
        public int TotalPages => _Cache.TotalPages;


        /// <summary>
        /// The current page number based on the most recent search.
        /// </summary>
        public int CurrentPage => _Cache.CurrentPage;
    }
}
