using System.Diagnostics;
using System.Text.Json;

using SquoundApp.Utilities;

using Shared.DataTransfer;


namespace SquoundApp.Services
{
    public class ItemService(HttpService httpService)
    {
        private readonly HttpService httpService = httpService;

        List<ItemSummaryDto> itemList = [];

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
        /// <param name="query">Search criteria to submit to the REST API.</param>
        /// <returns></returns>
        public async Task<SearchResponseDto<ItemSummaryDto>> GetItemSummariesAsync(SearchQueryDto query)
        {
            try
            {
                // Always clear the internal list prior to initiating a new fetch.
                itemList.Clear();

                // TODO : Want to set the base URL in the HttpService prior to release version.
                var url = $"{Scheme}://{LocalHostUrl}:{Port}/api/items/search?{query.ToQueryString()}";

                // Fetch the items from the REST API using the provided SearchResponseDto.
                var response = await httpService.GetJsonAsync<SearchResponseDto<ItemSummaryDto>>(url);

                // If the response is not null, assign it's payload to the internal list.
                if (response is not null)
                {
                    itemList = response.Items;
                }

                // Return the response with it's payload of items
                // if not null, else a default empty result.
                return response ?? new SearchResponseDto<ItemSummaryDto>();
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching items: {ex.Message}");

                await Shell.Current.DisplayAlert("Error", "Unable to fetch items from the server", "OK");

                // Return a default empty result on exception.
                return new SearchResponseDto<ItemSummaryDto>();
            }
        }


        /// <summary>
        /// Asynchronously retrieves a single ItemDetailDto that matches the itemId from a REST API.
        /// </summary>
        /// <param name="itemId">Identifier of the ItemDetailDto to retrieve.</param>
        /// <returns></returns>
        public async Task<ItemDetailDto> GetItemDetailAsync(long itemId)
        {
            try
            {
                // TODO : Want to set the base URL in the HttpService prior to release version.
                var url = $"{Scheme}://{LocalHostUrl}:{Port}/api/items/{itemId}";

                var response = await httpService.GetJsonAsync<ItemDetailDto>(url);

                if (response is not null)
                {
                    return response;
                }

                // Return a default empty result if no item found.
                else return new ItemDetailDto();
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching item: {ex.Message}");

                await Shell.Current.DisplayAlert("Error", "Unable to fetch item from the server", "OK");

                // Return a default empty result on exception.
                return new ItemDetailDto();
            }
        }

        /// <summary>
        /// Asynchronously retrieves a list of items from a remote JSON file.
        /// </summary>
        /// <param name="url">URL of the file to read.
        /// Example: "https://raw.githubusercontent.com/bushack/files/refs/heads/main/items.json"</param>
        /// <returns></returns>
        public async Task<List<ItemSummaryDto>?> GetItemsRemoteJson(string url)
        {
            if (itemList?.Count > 0)
                return itemList;

            var response = await httpService.GetJsonAsync<List<ItemSummaryDto>>(url);

            if (response != null)
            {
                itemList = response;
            }

            return itemList;
        }

        /// <summary>
        /// Asynchronously retrieves a list of items from an embedded JSON file.
        /// </summary>
        /// <param name="filename">Relative filepath of the file to read.
        /// Example: "Resources/Raw/items.json"</param>
        /// <returns></returns>
        public async Task<List<ItemSummaryDto>?> GetItemsEmbeddedJson(string filename)
        {
            if (itemList?.Count > 0)
                return itemList;

            // Else read from embedded JSON file
            // See https://www.youtube.com/watch?v=DuNLR_NJv8U
            using var stream = await FileSystem.OpenAppPackageFileAsync(filename);
            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();
            var items = JsonSerializer.Deserialize<List<ItemSummaryDto>>(contents);

            if (items != null)
            {
                itemList = items;
            }

            return itemList;
        }
    }
}
