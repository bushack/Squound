using Shared.DataTransfer;


namespace SquoundApp.Interfaces
{
    public interface IItemSummaryRepository
    {
        /// <summary>
        /// Provides asynchronous retrieval of item summaries based on the specified search context.
        /// This method interacts with an underlying item service to fetch data from a REST API.
        /// The data is paged and sorted according to the parameters defined in the search context.
        /// To query the pagination metadata (e.g., total items, total pages, current page, etc.),
        /// use the corresponding properties on this interface after calling this method.
        /// </summary>
        /// <param name="searchContext">Reference to a search context defining the query parameters.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task<IReadOnlyList<ItemSummaryDto>> GetItemsAsync(ISearchContext searchContext);

        /// <summary>
        /// Determines if the repository is empty.
        /// </summary>
        public bool IsEmpty { get; }


        /// <summary>
        /// Determines if the repository is not empty.
        /// </summary>
        public bool IsNotEmpty { get; }


        /// <summary>
        /// Determines if there is another page of items available.
        /// </summary>
        public bool HasNextPage { get; }


        /// <summary>
        /// Determines if there is a previous page of items available.
        /// </summary>
        public bool HasPrevPage { get; }


        /// <summary>
        /// The total number of items available based on the most recent search.
        /// </summary>
        public int TotalItems { get; }


        /// <summary>
        /// The total number of pages available based on the most recent search.
        /// </summary>
        public int TotalPages { get; }


        /// <summary>
        /// The current page number based on the most recent search.
        /// </summary>
        public int CurrentPage { get; }
    }
}
