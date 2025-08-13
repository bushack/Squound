using Microsoft.Extensions.Logging;

using SquoundApp.Exceptions;
using SquoundApp.Interfaces;

using Shared.DataTransfer;
using Shared.Logging;


namespace SquoundApp.Services
{
    public class CategoryService(HttpService httpService, ILogger<CategoryService> logger) : IService<CategoryDto>
    {
        private readonly ILogger<CategoryService> _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly HttpService _HttpService = httpService ?? throw new ArgumentNullException(nameof(httpService));

        private List<CategoryDto> _CategoryList = [];

        /// <summary>
        /// Exposes the list of categories.
        /// </summary>
        public IReadOnlyList<CategoryDto> CategoryList => _CategoryList;

        /// <summary>
        /// Queries whether the list of categories has been loaded.
        /// </summary>
        public bool IsLoaded => _CategoryList.Count > 0;


        /// <summary>
        /// Asynchronously retrieves a list of item categories from a REST API.
        /// </summary>
        /// <returns></returns>
        public async Task<Result<List<CategoryDto>>> GetDataAsync()
        {
            // For the release version of the project we will set the base address for the HttpService
            // This is useful if you are making multiple requests to the same base URL.
            // For example "https://squound.azure.net/api/items/";

            // URL of debug REST service (Android does not support https://localhost:5001)
            // This URL is used for debugging purposes on Android devices or emulators.
            string LocalHostUrl = DeviceInfo.Platform == DevicePlatform.Android ? "192.168.1.114" : "localhost";
            string Scheme = DeviceInfo.Platform == DevicePlatform.Android ? "http" : "https";
            string Port = DeviceInfo.Platform == DevicePlatform.Android ? "5050" : "7184";
            string RestUrl = $"{Scheme}://{LocalHostUrl}:{Port}/api/items/categories";

            if (_CategoryList.Count > 0)
            {
                _Logger.LogDebug("Category data in memory. Returning cached data.");

                return Result<List<CategoryDto>>.Ok(_CategoryList);
            }

            try
            {
                _Logger.LogInformation("Retrieving categories from {RestUrl}", RestUrl);

                // Fetch the categories from the REST API.
                var response = await _HttpService.GetJsonAsync<List<CategoryDto>>(RestUrl)
                    ?? throw new ApiResponseException($"Attempt to retrieve JSON content from {RestUrl} failed.");

                _CategoryList = response.Data
                    ?? throw new ApiResponseException("JSON content retrieved from {RestUrl} is null.");

                _Logger.LogInformation("Successfully retrieved categories from {RestUrl}.", RestUrl);

                // TODO : If we fail to retrieve the categories nothing will be displayed.
                // Is it better to hardwire categories? Or maybe have a fallback list of categories in the
                // event the query fails.
                return Result<List<CategoryDto>>.Ok(_CategoryList);
            }

            catch (ApiResponseException ex)
            {
                _Logger.LogWarning(ex, "Invalid response from server at endpoint {RestUrl}", RestUrl);

                return Result<List<CategoryDto>>.Fail("Invalid response from server.");
            }

            catch (Exception ex)
            {
                _Logger.LogWarning(ex, "Undefined error from server at endpoint {RestUrl}", RestUrl);

                return Result<List<CategoryDto>>.Fail("Undefined error from server.");
            }
        }

        public async Task RefreshDataAsync()
        {
            _CategoryList = [];

            await GetDataAsync();
        }
    }
}