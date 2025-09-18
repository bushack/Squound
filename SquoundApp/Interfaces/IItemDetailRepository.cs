using SquoundApp.Exceptions;

using Shared.DataTransfer;


namespace SquoundApp.Interfaces
{
    public interface IItemDetailRepository
    {
        /// <summary>
        /// Asynchronously checks the availability of data for a specific item.
        /// </summary>
        /// <param name="searchContext">Reference to a search context defining the query parameters.</param>
        /// <returns>True if the data is available.</returns>
        /// <exception cref="ItemRepositoryException">Thrown when item data is not available.</exception>
        public Task<bool> IsItemAvailable(ISearchContext searchContext);

        /// <summary>
        /// Asynchronously retrieves detailed information for a specific item by its unique identifier.
        /// </summary>
        /// <param name="searchContext">Reference to a search context defining the query parameters.</param>
        /// <returns></returns>
        /// <exception cref="ItemRepositoryException">Thrown when unable to successfully retrieve item detail.</exception>
        public Task<ItemDetailDto> GetItemAsync(ISearchContext searchContext);
    }
}
