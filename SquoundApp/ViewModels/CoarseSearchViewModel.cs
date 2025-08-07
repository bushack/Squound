using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;
using System.Diagnostics;

using SquoundApp.Extensions;
using SquoundApp.Pages;
using SquoundApp.Services;

using Shared.DataTransfer;


namespace SquoundApp.ViewModels
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CoarseSearchViewModel"/> class
    /// with the specified <see cref="CategoryService"/> and <see cref="SearchService"/>.
    /// </summary>
    /// <param name="cs">The <see cref="CategoryService"/> instance used
    /// to retrieve product category data. Cannot be null.</param>
    /// <param name="ss">The <see cref="SearchService"/> instance used
    /// to manage the user's current search selection. Cannot be null.</param>
    public partial class CoarseSearchViewModel(CategoryService cs, SearchService ss) : BaseViewModel
    {
        // Responsible for managing the current search criteria.
        private readonly SearchService searchService = ss ?? throw new ArgumentNullException(nameof(ss));

        // Responsible for retrieving product categories and subcategories from the REST API.
        // This data is presented to the user on the CoarshSearchPage, where the user can select a category
        // or subcategory before progressing to the RefinedSearchPage to further hone in on specific products.
        private readonly CategoryService categoryService = cs ?? throw new ArgumentNullException(nameof(cs));

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
        private ObservableCollection<object> itemList = [];

        // The category or subcategory selected by the user.
        // This property is used to track the user's selection and trigger either the loading of
        // subcategories or navigation to the refined search page.
        [ObservableProperty]
        private object? selectedItem = null;


        /// <summary>
        /// Populates the ItemList with categories when the CategoryList changes.
        /// This typically happens only once when the CoarseSearchPage's OnAppearing method
        /// is called for the first time.
        /// </summary>
        /// <param name="value"></param>
        partial void OnCategoryListChanged(List<CategoryDto> value)
        {
            // When the CategoryList changes, we also want to update the ItemList
            // to reflect the current categories available for selection.
            // This ensures that the UI is always in sync with the data.
            ItemList.Clear();

            foreach (var category in value)
                ItemList.Add(category);
        }


        /// <summary>
        /// Updates the ItemList contents and Title when the SelectedCategory changes.
        /// </summary>
        /// <param name="value">The category or subcategory selected by the user.</param>
        partial void OnSelectedItemChanged(object? value)
        {
            switch (value)
            {
                // If the selected item is a category, we want to load its subcategories into the ItemList.
                case CategoryDto category:
                {
                    // Check if the selected category is null or if it has no subcategories.
                    if (category.Subcategories is null || category.Subcategories.Count is 0)
                        return;

                    // If the selected category has subcategories, load them into the ItemList.
                    ItemList.Clear();

                    foreach (var subcategory in category.Subcategories)
                    ItemList.Add(subcategory);

                    // Write the selected category to the current search.
                    searchService.CurrentQuery.Category = category.Name;

                    // Update the UI to reflect the selected category and its subcategories.
                    Title = category.Name;

                    break;
                }

                // If the selected item is a subcategory, we want to navigate to the RefinedSearchPage.
                case SubcategoryDto subcategory:
                {
                    // Clear the selected item and title in readiness for the next time the page is displayed.
                    //SelectedItem = null;
                    //Title = string.Empty;

                    // Write the selected subcategory to the current search.
                    searchService.CurrentQuery.Subcategory = subcategory.Name;

                    // If the selected item is a subcategory, navigate to the RefinedSearchPage.
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
            // lead to performance issues or unexpected behavior.
            // This is a common pattern to avoid re-entrancy issues in async methods.
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

                // Retrieve product categories from the category service.
                // This method is expected to return a list of product categories asynchronously.
                // The retrieved categories will be added to the categoryList collection.
                var categories = await categoryService.GetDataAsync();

                if (categories == null)
                    return;

                // By assigning the fetched categories to the CategoryList, a notification is triggered
                // that the collection has changed and the OnCategoryListChanged method is called.
                CategoryList = new List<CategoryDto>(categories);
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
            await Shell.Current.GoToAsync(nameof(RefinedSearchPage));
        }
    }
}
