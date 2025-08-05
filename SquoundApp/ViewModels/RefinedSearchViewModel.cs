using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;
using System.Diagnostics;

using SquoundApp.Pages;
using SquoundApp.Services;

using Shared.DataTransfer;


namespace SquoundApp.ViewModels
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RefinedSearchViewModel"/> class
    /// with the specified <see cref="ProductService"/> and <see cref="SearchService"/>.
    /// </summary>
    /// <param name="ps">The <see cref="ProductService"/> instance used
    /// to retrieve product data. Cannot be null.</param>
    /// <param name="ss">The <see cref="SearchService"/> instance used
    /// to manage the user's current search selection. Cannot be null.</param>
    public partial class RefinedSearchViewModel(ProductService ps, SearchService ss) : BaseViewModel
    {
        // Responsible for retrieving products from the REST API.
        // This data is presented to the user on the RefinedSearchPage, where the user can select a specific
        // product before progressing to the ProductListingPage to study the product in detail.
        private readonly ProductService productService = ps ?? throw new ArgumentNullException(nameof(ps));

        // Responsible for managing the current search criteria.
        private readonly SearchService searchService = ss ?? throw new ArgumentNullException(nameof(ss));

        // Collection of products retrieved from the REST API based on the current search criteria.
        public ObservableCollection<ProductDto> ProductList { get; } = [];


        /// <summary>
        /// Query command that is executed whenever the RefinedSearchPage appears.
        /// This command is responsible for fetching products based on the current search criteria.
        /// If the search criteria changes the application should re-navigate to the RefinedSearchPage
        /// and this command will automatically execute a fetch of the latest products.
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

                // Save a deep copy of the current query to the PreviousQuery property.
                // This is useful for scenarios where you might want to revert to the
                // previous query or to compare the current query with the previous one.
                searchService.SaveCurrentSearch();

                // Clear the existing products in the ObservableCollection.
                // This ensures that the collection is updated with whatever is returned by the API.
                // ObservableCollection is used here so that the UI can automatically update
                // when items are added or removed, without needing to manually refresh the UI.
                // This is a key feature of ObservableCollection, which is designed for data binding in UI frameworks.
                ProductList.Clear();

                // Retrieve products from the product service.
                // This method is expected to return a list of products asynchronously.
                // The retrieved products will be added to the productList collection.
                // To retrieve products from a remote JSON file, use:
                // var productList = await productService.GetProductsRemoteJson
                // ("https://raw.githubusercontent.com/bushack/files/refs/heads/main/products.json");
                // To retrieve products from an embedded JSON file instead, use:
                // var productList = await productService.GetProductsEmbeddedJson();
                var productList = await productService.GetProductsRestApi(searchService.CurrentQuery);

                if (productList == null)
                    return;

                // The foreach loop iterates over the fetched products and adds each one to the Products collection.
                // This ensures that the UI reflects the latest data.
                // The Products collection is an ObservableCollection, which means that any changes to it
                // will automatically notify the UI to update, making it easy to display dynamic data.
                // This is particularly useful in MVVM (Model-View-ViewModel) patterns where the ViewModel
                // holds the data and the View binds to it.
                //ProductList.Clear();
                foreach (var product in productList)
                {
                    // Add each product to the ObservableCollection.
                    // This will trigger the UI to update and display the new products.
                    // The ObservableCollection is designed to notify the UI of changes,
                    // so when we add items to it, the UI will automatically reflect those changes.
                    // ObservableRangeCollection would be more efficient if you want to add multiple items at once,
                    // and delay the UI update until all items are added.
                    ProductList.Add(product);
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
        /// Asynchronously initiates a navigation to the ProductListingPage of parameter 'product'.
        /// </summary>
        /// <param name="product">Data transfer object of the product to be displayed.</param>
        /// <returns></returns>
        [RelayCommand]
        async Task GoToProductListingAsync(ProductDto product)
        {
            if (product is null)
                return;

            // Navigate to the ProductListingPage and pass the selected product as a parameter.
            await Shell.Current.GoToAsync($"{nameof(ProductListingPage)}", true,
                new Dictionary<string, object>
                {
                    {"Product", product}
                });
        }
    }
}
