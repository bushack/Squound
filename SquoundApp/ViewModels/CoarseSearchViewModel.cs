using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using System.Collections.ObjectModel;
using System.Diagnostics;

using SquoundApp.Extensions;
using SquoundApp.Interfaces;
using SquoundApp.Pages;

using Shared.DataTransfer;


namespace SquoundApp.ViewModels
{
    public partial class CoarseSearchViewModel : BaseViewModel
    {
        private readonly ILogger<CoarseSearchViewModel> _Logger;
        private readonly ICategoryRepository _Repository;
        private readonly INavigationService _Navigation;
        private readonly ISearchContext _Search;

        // Collection of categories to display on the coarse search page.
        // This collection is populated by the ApplyQueryAsync method.
        // As an ObservableProperty, we receive notifications when the collection is reassigned or modified,
        // such as when new categories are fetched from the API.
        // This collection is used only inside this class and therefore does not require ObservableCollection status.
        [ObservableProperty]
        private List<CategoryDto> categoryList = [];

        // Dynamic collection of either categories or subcategories to display on the coarse search page.
        // This collection is populated based on the user-selected category and can be bound to UI elements.
        // As an ObservableProperty, we receive notifications when the collection is reassigned or modified,
        // although we do not currently make use of this feature.
        // This collection is bound to the UI and therefore must be an ObservableCollection so that the UI
        // automatically updates when items are added or removed.
        [ObservableProperty]
        private ObservableCollection<object> dynamicList = [];

        // The category or subcategory selected by the user.
        // This property is used to track the user's selection and trigger either the loading of
        // subcategories or navigation to the refined search page.
        [ObservableProperty]
        private object? selectedItem = null;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="categories"></param>
        /// <param name="search"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public CoarseSearchViewModel(ILogger<CoarseSearchViewModel> logger, ICategoryRepository repository,
            INavigationService navigation, ISearchContext search)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _Navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
            _Search = search ?? throw new ArgumentNullException(nameof(search));
        }


        /// <summary>
        /// Populates the DynamicList with categories when the CategoryList changes.
        /// This typically happens only once when the CoarseSearchPage's OnAppearing method
        /// is called for the first time.
        /// </summary>
        /// <param name="value"></param>
        partial void OnCategoryListChanged(List<CategoryDto> value)
        {
            // When the CategoryList changes, we want to assign the categories to the observable collection (updates UI).
            // This presents the list of categories to the user, allowing them to select one of their choice.
            // Note that ObservableCollection constructor accepts IEnumerable<ItemSummaryDto> type.
            DynamicList = [.. value];
        }


        /// <summary>
		/// Logic to be executed when the content of the DynamicList changes.
		/// </summary>
		/// <param name="value"></param>
        partial void OnDynamicListChanged(ObservableCollection<object> value)
        {
            // Update page title.
            // Note that Keyword overrides Subcategory, which overrides Category.
            Title = _Search.Keyword ??
                    _Search.Subcategory?.Name ??
                    _Search.Category?.Name ??
                    "Search";
        }


        /// <summary>
        /// Updates the DynamicList contents and Title when the SelectedCategory changes.
        /// </summary>
        /// <param name="value">The category or subcategory selected by the user.</param>
        partial void OnSelectedItemChanged(object? value)
        {
            switch (value)
            {
                case CategoryDto category:
                {
                    if (category.Subcategories.IsNullOrEmpty())
                        return;

                        // Assign the selected category to the current search.
                        _Search.Category = category;

                        // Assign the category's subcategories to the observable collection (updates UI).
                        // Note that ObservableCollection constructor accepts IEnumerable<ItemSummaryDto> type.
                        DynamicList = [.. category.Subcategories];

                    break;
                }

                // If the selected item is a subcategory, we want to navigate to the RefinedSearchPage.
                case SubcategoryDto subcategory:
                {
                    // Write the selected subcategory to the search service.
                    _Search.Subcategory = subcategory;

                    // The selected item is a subcategory, therefore navigate to the RefinedSearchPage.
                    // This is where the user can perform a more detailed search based on the selected subcategory.
                    GoToRefinedSearchPageAsync().FireAndForget();
                    break;
                }

                default:
                    Debug.WriteLine("Selected item is not a category or subcategory.");
                    break;
            }
        }


        public async Task GetDataAsync()
        {
            // Check if the view model is already busy fetching data.
            // This prevents multiple simultaneous fetch operations which could
            // lead to performance issues or unexpected behaviour.
            // This is a common pattern to avoid re-entrance issues in async methods.
            if (IsBusy)
                return;

            try
            {
                // Set IsBusy to true to indicate that a fetch operation is in progress.
                // This will typically disable UI elements that should not be interacted with
                // while the data is being fetched, providing a better user experience.
                // This is important to ensure that the UI reflects that data is being loaded,
                // and to prevent the user from initiating another fetch operation while one is already in progress.
                IsBusy = true;

                CategoryList = [.. await _Repository.GetCategoriesAsync()];

                // Retrieve item categories from the category service.
                // This method is expected to return a list of item categories asynchronously.
                // The retrieved categories will be added to the categoryList collection.
                //var response = await _Categories.GetDataAsync();

                //if (response.Success is false)
                //{
                //    await Shell.Current.DisplayAlert("Error", response.ErrorMessage, "OK");
                //    return;
                //}

                //// Null or empty item list from API.
                //if (response.Data is null || response.Data.Count == 0)
                //{
                //    await Shell.Current.DisplayAlert("Sorry", "No items matched the search criteria", "OK");
                //    return;
                //}

                //// By assigning the fetched categories to the CategoryList, a notification is triggered
                //// that the collection has changed and the OnCategoryListChanged method is called.
                //CategoryList = [.. response.Data];
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
            await _Navigation.GoToAsync(nameof(RefinedSearchPage));
        }
    }
}
