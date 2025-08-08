using System.Diagnostics;
using System.Text.Json;

using SquoundApp.Utilities;

using Shared.DataTransfer;


namespace SquoundApp.Services
{
    public class ItemService(HttpService httpService)
    {
        private readonly HttpService httpService = httpService;

        List<ItemDto> itemList = [];

        /// <summary>
        /// Asynchronously retrieves a list of items from a REST API.
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<SearchResponseDto<ItemDto>> GetItemsRestApi(SearchQueryDto query)
        {
            // For the release version of the project we will set the base address for the HttpService
            // This is useful if you are making multiple requests to the same base URL.
            // For example "https://squound.azure.net/api/items/";

            // URL of debug REST service (Android does not support https://localhost:5001)
            // This URL is used for debugging purposes on Android devices or emulators.
            string LocalHostUrl = DeviceInfo.Platform == DevicePlatform.Android ? "192.168.1.114" : "localhost";
            string Scheme = DeviceInfo.Platform == DevicePlatform.Android ? "http" : "https";
            string Port = DeviceInfo.Platform == DevicePlatform.Android ? "5050" : "7184";
            string RestUrl = $"{Scheme}://{LocalHostUrl}:{Port}/api/items/search?{query.ToQueryString()}";
            //string RestUrl = $"{Scheme}://{LocalHostUrl}:{Port}/api/items/search?category=None&manufacturer=Austinsuite&sortby=PriceDesc&pagenumber=1&pagesize=10";
            //string RestUrl = $"{Scheme}://{LocalHostUrl}:{Port}/api/items/search?category=Lighting&sortby=PriceAsc&pagenumber=1&pagesize=10";
            //string RestUrl = $"{Scheme}://{LocalHostUrl}:{Port}/api/items/12";
            //string RestUrl = $"{Scheme}://{LocalHostUrl}:{Port}/api/items/all";

            try
            {
                // Always clear the internal list prior to initiating a new fetch.
                itemList.Clear();

                // Fetch the items from the REST API using the provided SearchResponseDto.
                var response = await httpService.GetJsonAsync<SearchResponseDto<ItemDto>>(RestUrl);

                // If the response is not null, assign it's payload to the internal list.
                if (response is not null)
                {
                    itemList = response.Items;
                }

                // Return the response with it's payload of items
                // if not null, else a default empty result.
                return response ?? new SearchResponseDto<ItemDto>();
            }

            catch (Exception ex)
            {
                Debug.WriteLine($"Error fetching items: {ex.Message}");

                await Shell.Current.DisplayAlert("Error", "Unable to fetch items from the server", "OK");

                // Return a default empty result on exception.
                return new SearchResponseDto<ItemDto>();
            }
        }

        /// <summary>
        /// Asynchronously retrieves a list of items from a remote JSON file.
        /// </summary>
        /// <param name="url">URL of the file to read.
        /// Example: "https://raw.githubusercontent.com/bushack/files/refs/heads/main/items.json"</param>
        /// <returns></returns>
        public async Task<List<ItemDto>?> GetItemsRemoteJson(string url)
        {
            if (itemList?.Count > 0)
                return itemList;

            var response = await httpService.GetJsonAsync<List<ItemDto>>(url);

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
        public async Task<List<ItemDto>?> GetItemsEmbeddedJson(string filename)
        {
            if (itemList?.Count > 0)
                return itemList;

            // Else read from embedded JSON file
            // See https://www.youtube.com/watch?v=DuNLR_NJv8U
            using var stream = await FileSystem.OpenAppPackageFileAsync(filename);
            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();
            var items = JsonSerializer.Deserialize<List<ItemDto>>(contents);

            if (items != null)
            {
                itemList = items;
            }

            return itemList;
        }
    }
}
