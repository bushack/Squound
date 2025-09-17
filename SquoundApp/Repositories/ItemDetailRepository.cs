using Microsoft.Extensions.Logging;

using SquoundApp.Exceptions;
using SquoundApp.Interfaces;

using Shared.DataTransfer;


namespace SquoundApp.Repositories
{
    public class ItemDetailRepository : IItemDetailRepository
    {
        private readonly ILogger<ItemDetailRepository> _Logger;
        private readonly IItemDetailService _Service;

        // Internal cache of the most recently retrieved items.
        private readonly List<ItemDetailDto> _Cache = [];
        private readonly int _CacheCapacity = 5;


        /// <summary>
        /// Initializes a new instance of the <see cref="ItemDetailRepository"/> class.
        /// </summary>
        /// <param name="logger">Reference to a logging service.</param> 
        /// <param name="service">Reference to an item service for data retrieval.</param>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is null.</exception>
        public ItemDetailRepository(ILogger<ItemDetailRepository> logger, IItemDetailService service) 
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _Service = service ?? throw new ArgumentNullException(nameof(service));
        }


        /// <summary>
        /// Asynchronously retrieves detailed information for a specific item by its unique identifier.
        /// </summary>
        /// <param name="query">Query parameter object defining the objectives of the GET request.</param>
        /// <returns></returns>
        /// <exception cref="ItemRepositoryException">Thrown when unable to successfully retrieve item detail.</exception>
        public async Task<ItemDetailDto> GetItemAsync(ISearchContext searchContext)
        {
            if (searchContext.ItemId is null)
                throw new ItemRepositoryException("Item Id is null.");

            var itemId = searchContext.ItemId ?? Defaults.MinimumItemId;

            // Check if item is cached.
            var cachedItem = GetItemFromCache(itemId);

            if (cachedItem is not null)
            {
                _Logger.LogInformation("Returning cached data for Item Id: {itemId}.", itemId);
                return cachedItem;
            }

            // Item not cached - fetch from API.
            else
            {
                _Logger.LogInformation("Requesting data for Item Id: {itemId}.", itemId);

                // Fetch item from API. Throw exception on failure.
                var result = await _Service.GetDataAsync(searchContext)
                    ?? throw new ItemRepositoryException("Item data not received.");

                // Extract item from result. Throw exception on null.
                var data = result.Data
                    ?? throw new ItemRepositoryException("Item data is null.");

                // Cache item for future use.
                AddItemToCache(data);

                // Return item.
                _Logger.LogInformation("Returning data for Item Id: {itemId}.", itemId);
                return data;
            }
        }


        private void AddItemToCache(ItemDetailDto itemDetail)
        {
            // Check if item is already in cache.
            if (_Cache.Any(item => item.ItemId == itemDetail.ItemId))
            {
                _Logger.LogInformation("Item Id: {itemId} already in cache. Skipping add.", itemDetail.ItemId);
                return;
            }

            // Enforce limit on cache capacity.
            if (_Cache.Count >= _CacheCapacity)
            {
                _Cache.RemoveAt(0);
                _Logger.LogInformation("Discarded Item Id: {itemId} from cache.", _Cache.ElementAt(0));
            }

            // Add the new item to the cache.
            _Cache.Add(itemDetail);
            _Logger.LogInformation("Added Item Id: {itemId} to cache.", itemDetail.ItemId);
        }


        private ItemDetailDto? GetItemFromCache(long itemId)
        {
            _Logger.LogInformation("Checking cache for Item Id: {itemId}", itemId);
            return _Cache.FirstOrDefault(item => item.ItemId == itemId);
        }
    }
}
