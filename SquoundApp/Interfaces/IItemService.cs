
using Shared.DataTransfer;
using Shared.Logging;


namespace SquoundApp.Interfaces
{
    public interface IItemService
    {
        public Task<Result<SearchResponseDto<ItemSummaryDto>>> GetDataAsync(ISearchContext searchContext);


        /// <summary>
        /// Asynchronously retrieves a single ItemDetailDto that matches the itemId from a REST API.
        /// </summary>
        /// <param name="itemId">Identifier of the ItemDetailDto to retrieve.</param>
        /// <returns></returns>
        public Task<Result<ItemDetailDto>> GetDataAsync(long itemId);
    }
}
