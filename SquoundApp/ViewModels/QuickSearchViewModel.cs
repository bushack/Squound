using CommunityToolkit.Mvvm.ComponentModel;

using System.Collections.ObjectModel;

using Microsoft.Extensions.Logging;

using SquoundApp.Extensions;
using SquoundApp.Interfaces;
using SquoundApp.Pages;
using SquoundApp.Services;

using Shared.DataTransfer;


namespace SquoundApp.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public partial class QuickSearchViewModel : BaseViewModel
    {
        private readonly ILogger<QuickSearchViewModel> _Logger;
        private readonly ICategoryService _Categories;
        private readonly ISearchContext _Search;

        // Ensures one-time initialization.
        private bool _IsInitialized = false;

        // For user interface binding.
        [ObservableProperty]
        private ObservableCollection<CategoryDto> categoryList = [];

        // For user interface binding.
        [ObservableProperty]
        private CategoryDto? selectedItem = null;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="categories"></param>
        /// <param name="search"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public QuickSearchViewModel(ILogger<QuickSearchViewModel> logger, ICategoryService categories, ISearchContext search)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _Categories = categories ?? throw new ArgumentNullException(nameof(categories));
            _Search = search ?? throw new ArgumentNullException(nameof(search));
        }


        /// <summary>
        /// Retrieves item categories from the category service and populates the CategoryList.
        /// This method is called by the CoarseSearchPage's OnAppearing method.
        /// </summary>
        public async Task InitAsync()
        {
            // Check if the view model is already busy fetching data.
            // This prevents multiple simultaneous fetch operations which could
            // lead to performance issues or unexpected behaviour.
            // This is a common pattern to avoid re-entry issues in async methods.
            if (IsBusy)
                return;

            try
            {
                // Ensure one-time run of initialization logic.
                if (_IsInitialized)
                    return;

                _IsInitialized = true;

                // Set IsBusy to true to indicate that a fetch operation is in progress.
                // This will typically disable UI elements that should not be interacted with
                // while the data is being fetched, providing a better user experience.
                // This is important to ensure that the UI reflects that data is being loaded,
                // and to prevent the user from initiating another fetch operation while one is already in progress.
                IsBusy = true;

                // Retrieve item categories from the category service.
                // This method is expected to return a list of item categories asynchronously.
                // The retrieved categories will be added to the categoryList collection.
                var response = await _Categories.GetDataAsync();

                if (response.Success is false)
                {
                    await Shell.Current.DisplayAlert("Error", response.ErrorMessage, "OK");
                    return;
                }

                // Null or empty item list from API.
                if (response.Data is null || response.Data.Count == 0)
                {
                    await Shell.Current.DisplayAlert("Sorry", "No items matched the search criteria", "OK");
                    return;
                }

                // By assigning the fetched categories to the CategoryList, a notification is triggered
                // that the collection has changed and the OnCategoryListChanged method is called.
                CategoryList = [.. response.Data];
            }

            catch (Exception ex)
            {
                _Logger.LogWarning(ex, "Error while retrieving category data.");

                // Display an alert to the user indicating that an error occurred while fetching data.
                await Shell.Current.DisplayAlert(
                    "Search Error",
                    "An undefined error occurred while attempting to retrieve data",
                    "OK");
            }

            finally
            {
                IsBusy = false;
            }
        }


        /// <summary>
        /// Called when the SelectedItem property changes.
        /// </summary>
        partial void OnSelectedItemChanged(CategoryDto? value)
        {
            if (value is not null)
            {
                // Write the selected category to the search service.
                _Search.Category = value;

                // Navigate to the RefinedSearchPage.
                GoToRefinedSearchPageAsync().FireAndForget();
            }
        }


        /// <summary>
        /// Asynchronously initiates a navigation to the RefinedSearchPage.
        /// </summary>
        private async Task GoToRefinedSearchPageAsync()
        {
            await ServiceLocator.GetService<NavigationService>().GoToAsync(nameof(RefinedSearchPage));
        }
    }
}
