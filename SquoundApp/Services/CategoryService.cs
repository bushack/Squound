using Microsoft.Extensions.Logging;

using SquoundApp.Interfaces;
using SquoundApp.Utilities;

using Shared.DataTransfer;


namespace SquoundApp.Services
{
    public class CategoryService(HttpService httpService) : IService<CategoryDto>
    {
        private readonly HttpService httpService = httpService ?? throw new ArgumentNullException(nameof(httpService));

        private List<CategoryDto>? categoryList = null;

        private Task<List<CategoryDto>>? loadTask = null;

        /// <summary>
        /// Exposes the list of categories.
        /// </summary>
        public IReadOnlyList<CategoryDto> CategoryList => categoryList ?? [];

        /// <summary>
        /// Queries whether the list of categories has been loaded.
        /// </summary>
        public bool IsLoaded => categoryList is not null;


        /// <summary>
        /// Asynchronously retrieves a list of product categories from a REST API.
        /// </summary>
        /// <returns></returns>
        public async Task<List<CategoryDto>> GetDataAsync()
        {
            if (categoryList is not null)
                return categoryList;

            loadTask ??= LoadDataAsync();

            var result = await loadTask;

            categoryList ??= result;

            return categoryList;
        }


        /// <summary>
        /// Internal method that retrieves product categories from REST API.
        /// </summary>
        /// <returns></returns>
        private async Task<List<CategoryDto>> LoadDataAsync()
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

            try
            {
                // Fetch the categories from the REST API.
                var categories = await httpService.GetJsonAsync<List<CategoryDto>>(RestUrl);

                return categories ?? new List<CategoryDto>();
            }

            catch (Exception ex)
            {
                // Display an alert to the user indicating that an error occurred while fetching data.
                await Shell.Current.DisplayAlert(
                    "Error",
                    "An undefined error occurred while attempting to fetch data",
                    "OK");

                return new List<CategoryDto>();
            }
        }

        public async Task RefreshDataAsync()
        {
            loadTask = null;
            categoryList = null;

            await GetDataAsync();
        }
    }
}