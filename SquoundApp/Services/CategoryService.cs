using SquoundApp.Utilities;

using Shared.DataTransfer;


namespace SquoundApp.Services
{
    public class CategoryService
    {
        private readonly HttpService httpService;

        List<CategoryDto> categoryList = [];

        public CategoryService(HttpService httpService)
        {
            this.httpService = httpService;
        }


        /// <summary>
        /// Asynchronously retrieves a list of product categories from a REST API.
        /// </summary>
        /// <returns></returns>
        public async Task<List<CategoryDto>?> GetCategoriesRestApi()
        {
            // For the release version of the project we will set the base address for the HttpService
            // This is useful if you are making multiple requests to the same base URL.
            // For example "https://squound.azure.net/api/products/";

            // URL of debug REST service (Android does not support https://localhost:5001)
            // This URL is used for debugging purposes on Android devices or emulators.
            string LocalHostUrl = DeviceInfo.Platform == DevicePlatform.Android ? "192.168.1.114" : "localhost";
            string Scheme = DeviceInfo.Platform == DevicePlatform.Android ? "http" : "https";
            string Port = DeviceInfo.Platform == DevicePlatform.Android ? "5050" : "7184";
            string RestUrl = $"{Scheme}://{LocalHostUrl}:{Port}/api/products/categories";

            // Fetch the categories from the REST API.
            var response = await httpService.GetJsonAsync<List<CategoryDto>>(RestUrl);

            if (response is not null)
            {
                // Clear the internal list prior to repopulating it with new data.
                categoryList.Clear();

                // Assign the fetched categories to the internal list.
                categoryList = response;
            }

            return categoryList;
        }
    }
}
