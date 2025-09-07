using Microsoft.Extensions.Logging;

using SquoundApp.Exceptions;
using SquoundApp.Interfaces;

using Shared.DataTransfer;
using Shared.Interfaces;
using Shared.Logging;


namespace SquoundApp.Services
{
    public class ItemService(ILogger<ItemService> logger, IEventService events, IHttpService http) : IItemService
    {
        private readonly ILogger<ItemService> _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
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
        /// Asynchronously retrieves a list of item summaries that match the query criteria from a REST API.
        /// </summary>
        /// <param name="searchContext"></param>
        /// <returns></returns>
        public async Task<Result<SearchResponseDto<ItemSummaryDto>>> GetDataAsync(ISearchContext searchContext)
        {
            try
            {
                // TODO : Want to set the base URL in the HttpService prior to release version.
                var url = $"{Scheme}://{LocalHostUrl}:{Port}/api/items/search?{searchContext.BuildUrlQueryString()}";

                _Logger.LogInformation("Retrieving items from {url}", url);

                // Fetch the items from the REST API.
                var response = await _Http.GetJsonAsync<SearchResponseDto<ItemSummaryDto>>(url)
                    ?? throw new ApiResponseException("API retrieve JSON failed.");

                var data = response.Data
                    ?? throw new ApiResponseException("API response data is null.");

                _Logger.LogInformation("Successfully retrieved {Count} item(s).", data.Items.Count);

                // Save the search context in case the user wishes to cancel any future changes.
                searchContext.SaveChanges();

                // Return success code and pagination data alongside the item summaries.
                return Result<SearchResponseDto<ItemSummaryDto>>.Ok(data);
            }

            catch (ApiResponseException ex)
            {
                _Logger.LogWarning(ex, "Invalid response from server.");
                
                return Result<SearchResponseDto<ItemSummaryDto>>.Fail("Invalid response from server.");
            }

            catch (Exception ex)
            {
                _Logger.LogWarning(ex, "Undefined error from server.");
                
                return Result<SearchResponseDto<ItemSummaryDto>>.Fail("Undefined error from server.");
            }
        }


        /// <summary>
        /// Asynchronously retrieves a single ItemDetailDto that matches the itemId from a REST API.
        /// </summary>
        /// <param name="itemId">Identifier of the ItemDetailDto to retrieve.</param>
        /// <returns></returns>
        public async Task<Result<ItemDetailDto>> GetDataAsync(long itemId)
        {
            try
            {
                // TODO : Want to set the base URL in the HttpService prior to release version.
                var url = $"{Scheme}://{LocalHostUrl}:{Port}/api/items/{itemId}";

                _Logger.LogInformation("Retrieving item {itemId} from {url}", itemId, url);

                // Fetch the items from the REST API.
                var response = await _Http.GetJsonAsync<ItemDetailDto>(url)
                    ?? throw new ApiResponseException("Failed to retrieve JSON content from API.");

                var data = response.Data
                    ?? throw new ApiResponseException("JSON content retrieved from API is null.");

                _Logger.LogInformation("Successfully retrieved item {ItemId} from {url}.", data.ItemId, url);

                return Result<ItemDetailDto>.Ok(data);
            }

            catch (ApiResponseException ex)
            {
                _Logger.LogWarning(ex, "Invalid response while retrieving item {itemId} from server.", itemId);

                return Result<ItemDetailDto>.Fail("Invalid response from server.");
            }

            catch (Exception ex)
            {
                _Logger.LogWarning(ex, "Unexpected error while retrieving item {itemId} from server.", itemId);

                return Result<ItemDetailDto>.Fail("Unexpected error occurred while retrieving data from server.");
            }
        }

        // <summary>
        // Asynchronously retrieves a list of items from a remote JSON file.
        // </summary>
        // <param name="url">URL of the file to read.
        // Example: "https://raw.githubusercontent.com/bushack/files/refs/heads/main/items.json"</param>
        // <returns></returns>
        //public async Task<List<ItemSummaryDto>?> GetItemsRemoteJson(string url)
        //{
        //    if (itemList?.Count > 0)
        //        return itemList;

        //    var response = await httpService.GetJsonAsync<List<ItemSummaryDto>>(url);

        //    if (response != null)
        //    {
        //        itemList = response;
        //    }

        //    return itemList;
        //}

        // <summary>
        // Asynchronously retrieves a list of items from an embedded JSON file.
        // </summary>
        // <param name="filename">Relative filepath of the file to read.
        // Example: "Resources/Raw/items.json"</param>
        // <returns></returns>
        //public async Task<List<ItemSummaryDto>?> GetItemsEmbeddedJson(string filename)
        //{
        //    if (itemList?.Count > 0)
        //        return itemList;

        //    // Else read from embedded JSON file
        //    // See https://www.youtube.com/watch?v=DuNLR_NJv8U
        //    using var stream = await FileSystem.OpenAppPackageFileAsync(filename);
        //    using var reader = new StreamReader(stream);
        //    var contents = await reader.ReadToEndAsync();
        //    var items = JsonSerializer.Deserialize<List<ItemSummaryDto>>(contents);

        //    if (items != null)
        //    {
        //        itemList = items;
        //    }

        //    return itemList;
        //}
    }
}
