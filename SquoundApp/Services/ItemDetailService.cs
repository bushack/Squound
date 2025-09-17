using Microsoft.Extensions.Logging;

using SquoundApp.Exceptions;
using SquoundApp.Interfaces;

using Shared.DataTransfer;
using Shared.Interfaces;
using Shared.Logging;


namespace SquoundApp.Services
{
    public class ItemDetailService(ILogger<ItemDetailService> logger, IEventService events, IHttpService http) : IItemDetailService
    {
        private readonly ILogger<ItemDetailService> _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IEventService _Events = events ?? throw new ArgumentNullException(nameof(events));
        private readonly IHttpService _Http = http ?? throw new ArgumentNullException(nameof(http));

        //private SearchResponseDto<ItemSummaryDto> _Response = new();

        // For the release version of the project we will set the base address for the HttpService
        // This is useful if you are making multiple requests to the same base URL.
        // For example "https://squound.azure.net/api/items/";

        // URL of debug REST service (Android does not support https://localhost:5001)
        // This URL is used for debugging purposes on Android devices or emulators.
        private readonly string LocalHostUrl = DeviceInfo.Platform == DevicePlatform.Android ? "192.168.1.114" : "localhost";
        private readonly string Scheme = DeviceInfo.Platform == DevicePlatform.Android ? "http" : "https";
        private readonly string Port = DeviceInfo.Platform == DevicePlatform.Android ? "5050" : "7184";
        //string RestUrl = $"{Scheme}://{LocalHostUrl}:{Port}/api/items/search?category=None&manufacturer=Austinsuite&sortby=PriceDesc&pagenumber=1&pagesize=10";
        //string RestUrl = $"{Scheme}://{LocalHostUrl}:{Port}/api/items/search?category=Lighting&sortby=PriceAsc&pagenumber=1&pagesize=10";
        //string RestUrl = $"{Scheme}://{LocalHostUrl}:{Port}/api/items/12";
        //string RestUrl = $"{Scheme}://{LocalHostUrl}:{Port}/api/items/all";


        /// <summary>
        /// Asynchronously attempts to retrieve a single ItemDetailDto object from a REST API.
        /// </summary>
        /// <param name="searchContext">Database query parameters, for filtering search results.</param>
        public async Task<Result<ItemDetailDto>> GetDataAsync(ISearchContext searchContext)
        {
            try
            {
                // TODO : Want to set the base URL in the HttpService prior to release version.
                var url = $"{Scheme}://{LocalHostUrl}:{Port}/api/items/detail?{searchContext.BuildItemDetailUrlQueryString()}";

                _Logger.LogInformation("Retrieving item {itemId} from {url}", searchContext.ItemId, url);

                // Fetch the items from the REST API.
                var response = await _Http.GetJsonAsync<ItemDetailDto>(url)
                    ?? throw new ApiResponseException("Failed to retrieve JSON content from API.");

                var data = response.Data
                    ?? throw new ApiResponseException("JSON content retrieved from API is null.");

                _Logger.LogInformation("Successfully retrieved item {itemId} from {url}.", searchContext.ItemId, url);

                return Result<ItemDetailDto>.Ok(data);
            }

            catch (ApiResponseException ex)
            {
                _Logger.LogWarning(ex, "Invalid response while retrieving item {itemId} from server.", searchContext.ItemId);

                return Result<ItemDetailDto>.Fail("Invalid response from server.");
            }

            catch (Exception ex)
            {
                _Logger.LogWarning(ex, "Unexpected error while retrieving item {itemId} from server.", searchContext.ItemId);

                return Result<ItemDetailDto>.Fail("Unexpected error occurred while retrieving data from server.");
            }
        }
    }
}