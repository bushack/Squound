using Microsoft.Extensions.Logging;

using SquoundApp.Interfaces;

using Shared.DataTransfer;


namespace SquoundApp.Repositories
{
    public class CategoryRepository(ILogger<CategoryRepository> logger, ICategoryService service) : ICategoryRepository
    {
        private readonly ILogger<CategoryRepository> _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly ICategoryService _Service = service ?? throw new ArgumentNullException(nameof(service));

        // Internal cache.
        private List<CategoryDto> _CategoryList = [];

        // Queries whether the internal cache has been populated.
        public bool IsLoaded => _CategoryList.Count > 0;


        public async Task<IReadOnlyList<CategoryDto>> GetCategoriesAsync()
        {
            if (IsLoaded)
            {
                _Logger.LogDebug("Returning cached data.");
                return _CategoryList;
            }

            else
            {
                _Logger.LogInformation("Requesting data.");
                var result = await _Service.GetDataAsync();

                // Unable to fetch categories from API.
                if (result.Success is false)
                {
                    _Logger.LogDebug("Request failed: {message}", result.ErrorMessage);
                    _Logger.LogDebug("Returning cached data.");
                    return _CategoryList;
                }

                // Null or empty item list from API.
                if (result.Data is null || result.Data.Count == 0)
                {
                    _Logger.LogDebug("Request successful but returned no data.");
                    _Logger.LogDebug("Returning cached data.");
                    return _CategoryList;
                }

                _Logger.LogInformation("Request successful. Caching data.");
                _CategoryList = [.. result.Data];

                return _CategoryList;
            }
        }
    }
}
