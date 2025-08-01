using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;
using System.Diagnostics;

using SquoundApp.Pages;
using SquoundApp.Services;

using Shared.DataTransfer;


namespace SquoundApp.ViewModels
{
    public partial class CoarseSearchViewModel : BaseViewModel
    {
        // Respomsible for retrieving data from the REST API.
        internal readonly CategoryService CategoryService;

        // Collection of categories to display on the coarse search page.
        public ObservableCollection<CategoryDto> CategoryList { get; } = [];


        /// <summary>
        /// Initializes a new instance of the <see cref="CoarseSearchViewModel"/> class
        /// with the specified category service.
        /// </summary>
        /// <param name="service">The <see cref="CategoryService"/> instance used
        /// to retrieve product category data. Cannot be null.</param>
        public CoarseSearchViewModel(CategoryService service)
        {
            CategoryService = service;
        }


        /// <summary>
        /// Retrieves product categories from the category service and populates the CategoryList.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task ApplyQueryAsync()
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

                // Clear the existing products in the ObservableCollection.
                // This ensures that the collection is updated with whatever is returned by the API.
                // ObservableCollection is used here so that the UI can automatically update
                // when items are added or removed, without needing to manually refresh the UI.
                // This is a key feature of ObservableCollection, which is designed for data binding in UI frameworks.
                CategoryList.Clear();

                // Retrieve product categories from the category service.
                // This method is expected to return a list of product categories asynchronously.
                // The retrieved categories will be added to the categoryList collection.
                var categoryList = await CategoryService.GetCategoriesRestApi();

                if (categoryList == null)
                    return;

                // The CategoryList is an ObservableCollection, which means that any changes to it
                // will automatically notify the UI to update, making it easy to display dynamic data.
                // This is particularly useful in MVVM (Model-View-ViewModel) patterns where the ViewModel
                // holds the data and the View binds to it.
                //ProductList.Clear();
                foreach (var category in categoryList)
                {
                    // The ObservableCollection is designed to notify the UI of changes,
                    // so when we add items to it, the UI will automatically reflect those changes.
                    // ObservableRangeCollection would be more efficient if you want to add multiple items at once,
                    // and delay the UI update until all items are added.
                    CategoryList.Add(category);
                }
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
        /// <param name="category"></param>
        /// <returns></returns>
        [RelayCommand]
        async Task GoToRefinedSearchPageAsync(CategoryDto category)
        {
            if (category is null)
                return;

            // Navigate to the RefinedSearchPage and pass the selected category as a parameter.
            await Shell.Current.GoToAsync($"{nameof(SearchPage)}", true,
                new Dictionary<string, object>
                {
                    {"Category", category}
                });
        }
    }
}
