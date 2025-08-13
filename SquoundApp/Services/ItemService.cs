using Microsoft.Extensions.Logging;

using SquoundApp.Exceptions;
using SquoundApp.States;

using Shared.DataTransfer;
using Shared.Logging;


namespace SquoundApp.Services
{
    public class ItemService(HttpService httpService, ILogger<ItemService> logger)
    {
        private readonly ILogger<ItemService> _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly HttpService _HttpService = httpService ?? throw new ArgumentNullException(nameof(httpService));

        SearchResponseDto<ItemSummaryDto> _Response = new();

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
        /// <param name="searchState"></param>
        /// <returns></returns>
        public async Task<Result<SearchResponseDto<ItemSummaryDto>>> GetDataAsync(SearchState searchState)
        {
            // Search state has not changed since last API call.
            if (searchState.HasNotChanged)
            {
                _Logger.LogDebug("Search state unchanged. Returning cached data.");

                // Return the cached response from the previous API call.
                return Result<SearchResponseDto<ItemSummaryDto>>.Ok(_Response);
            }

            // Search state has changed - fetch new data.
            try
            {
                // TODO : Want to set the base URL in the HttpService prior to release version.
                var url = $"{Scheme}://{LocalHostUrl}:{Port}/api/items/search?{searchState.ToQueryString()}";

                _Logger.LogInformation("Retrieving items from {url}", url);

                // Fetch the items from the REST API.
                var response = await _HttpService.GetJsonAsync<SearchResponseDto<ItemSummaryDto>>(url)
                    ?? throw new ApiResponseException("API retrieve JSON is failed.");

                // Cache data in the event that a duplicate search is performed - lowers API workload.
                _Response = response.Data
                    ?? throw new ApiResponseException("API response data is null.");

                // Member m_Response now contains data that matches the most up-to-date search state.
                searchState.ResetChangedFlag();

                _Logger.LogInformation("Successfully retrieved {Count} item(s).", _Response.Items.Count);

                return Result<SearchResponseDto<ItemSummaryDto>>.Ok(_Response);
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
                var response = await httpService.GetJsonAsync<ItemDetailDto>(url)
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

        /// <summary>
        /// Asynchronously retrieves a list of items from a remote JSON file.
        /// </summary>
        /// <param name="url">URL of the file to read.
        ///// Example: "https://raw.githubusercontent.com/bushack/files/refs/heads/main/items.json"</param>
        ///// <returns></returns>
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

        /// <summary>
        /// Asynchronously retrieves a list of items from an embedded JSON file.
        /// </summary>
        /// <param name="filename">Relative filepath of the file to read.
        /// Example: "Resources/Raw/items.json"</param>
        /// <returns></returns>
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
