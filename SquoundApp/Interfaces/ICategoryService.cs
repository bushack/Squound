using System.Collections.ObjectModel;

using Shared.DataTransfer;
using Shared.Logging;


namespace SquoundApp.Interfaces
{
    public interface ICategoryService
    {
        /// <summary>
        /// Exposes whether the internal cache has been populated.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// The observable list of categories for UI binding.
        /// </summary>
        //ObservableCollection<CategoryDto> Categories { get; }

        /// <summary>
        /// The observable list of subcategories for UI binding.
        /// </summary>
        //ObservableCollection<SubcategoryDto> Subcategories { get; }

        /// <summary>
        /// Asynchronously retrieves a list of item categories from a REST API.
        /// </summary>
        /// <returns></returns>
        public Task<Result<List<CategoryDto>>> GetDataAsync();

        /// <summary>
        /// Clears the cache and re-fetches the category data.
        /// </summary>
        public Task RefreshDataAsync();
    }
}
