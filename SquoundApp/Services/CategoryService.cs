using Microsoft.Extensions.Logging;

using SquoundApp.Exceptions;
using SquoundApp.Interfaces;

using Shared.DataTransfer;
using Shared.Interfaces;
using Shared.Logging;


namespace SquoundApp.Services
{
    public partial class CategoryService : ICategoryService
    {
        private readonly ILogger<CategoryService> _Logger;
        private readonly IEventService _Events;
        private readonly IHttpService _Http;

        // Internal cache.
        //private List<CategoryDto> _CategoryList = [];

        // For user interface binding.
        //[ObservableProperty]
        //private ObservableCollection<CategoryDto> categories = [];

        //// For user interface binding.
        //[ObservableProperty]
        //private ObservableCollection<SubcategoryDto> subcategories = [];

        /// <summary>
        /// Exposes the internal cache as a read-only list.
        /// </summary>
        //public IReadOnlyList<CategoryDto> CategoryList => _CategoryList;

        /// <summary>
        /// Queries whether the internal cache has been populated.
        /// </summary>
        //public bool IsLoaded => _CategoryList.Count > 0;


        public CategoryService(ILogger<CategoryService> logger, IEventService events, IHttpService http)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _Events = events ?? throw new ArgumentNullException(nameof(events));
            _Http = http ?? throw new ArgumentNullException(nameof(http));
        }


        /// <summary>
        /// Asynchronously retrieves a list of categories from a REST API.
        /// </summary>
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

            //if (_CategoryList.Count > 0)
            //{
            //    _Logger.LogDebug("Category data in memory. Returning cached data.");

            //    return Result<List<CategoryDto>>.Ok(_CategoryList);
            //}

            try
            {
                _Logger.LogInformation("Requesting data from server at endpoint: {RestUrl}", RestUrl);
                var result = await _Http.GetJsonAsync<List<CategoryDto>>(RestUrl)
                    ?? throw new ApiResponseException($"Request failed.");

                _Logger.LogInformation("Response received from server at endpoint: {RestUrl}", RestUrl);
                var data = result.Data
                    ?? throw new ApiResponseException("Data is null.");

                _Logger.LogInformation("Retrieved data from server at endpoint: {RestUrl}.", RestUrl);
                return Result<List<CategoryDto>>.Ok(data);
            }

            catch (ApiResponseException ex)
            {
                _Logger.LogWarning(ex, "Invalid response from server at endpoint: {RestUrl}", RestUrl);
                return Result<List<CategoryDto>>.Fail("Invalid response from server.");
            }

            catch (Exception ex)
            {
                _Logger.LogWarning(ex, "Undefined error from server at endpoint: {RestUrl}", RestUrl);
                return Result<List<CategoryDto>>.Fail("Undefined error from server.");
            }
        }
    }
}