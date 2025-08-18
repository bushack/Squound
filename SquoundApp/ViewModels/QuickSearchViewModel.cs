using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;
using System.Diagnostics;

using SquoundApp.Extensions;
using SquoundApp.Pages;
using SquoundApp.Services;

using Shared.DataTransfer;
using SquoundApp.States;


namespace SquoundApp.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="categoryService">The <see cref="CategoryService"/> instance used
    /// to retrieve item category data. Cannot be null.</param>
    /// <param name="searchService">The <see cref="SearchService"/> instance used
    /// to manage the user's current search selection. Cannot be null.</param>
    public partial class QuickSearchViewModel(CategoryService categoryService, SearchService searchService) : BaseViewModel
    {
        // Responsible for retrieving item categories from the REST API.
        private readonly CategoryService _CategoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));

        // Responsible for managing the current search criteria.
        private readonly SearchService _SearchService = searchService ?? throw new ArgumentNullException(nameof(searchService));

        //
        private bool _IsInitialized = false;

        //
        [ObservableProperty]
        private ObservableCollection<CategoryDto> categoryList = [];

        //
        [ObservableProperty]
        private object? selectedCategory = null;


        //
        partial void OnSelectedCategoryChanged(object? value)
        {
            if (value is not null && value is CategoryDto category)
            {
                // Write the selected category to the search service.
                _SearchService.Category = category;

                // Navigate to the RefinedSearchPage.
                GoToRefinedSearchPageAsync().FireAndForget();
            }
        }


        /// <summary>
        /// Retrieves item categories from the category service and populates the CategoryList.
        /// This method is called by the CoarseSearchPage's OnAppearing method.
        /// </summary>
        /// <returns></returns>
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
                if (_IsInitialized) return;
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
                var response = await _CategoryService.GetDataAsync();

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
                // Handle exceptions, e.g., log them.
                Debug.WriteLine($"Search error while attempting to fetch data: {ex.Message}");

                // Display an alert to the user indicating that an error occurred while fetching data.
                await Shell.Current.DisplayAlert(
                    "Search Error",
                    "An undefined error occurred while attempting to fetch data",
                    "OK");
            }

            finally
            {
                IsBusy = false;
            }
        }


        /// <summary>
        /// Asynchronously initiates a navigation to the RefinedSearchPage.
        /// </summary>
        /// <returns></returns>
        private async Task GoToRefinedSearchPageAsync()
        {
            // Navigate to the RefinedSearchPage and pass the selected category as a parameter.
            //await Shell.Current.GoToAsync(nameof(RefinedSearchPage));

            var navService = ServiceLocator.GetService<NavigationService>();
            await navService.GoToAsync(nameof(RefinedSearchPage));
        }
    }
}
