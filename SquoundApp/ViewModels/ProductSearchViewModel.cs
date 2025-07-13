using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using SquoundApp.Models;
using SquoundApp.Pages;
using SquoundApp.Services;


namespace SquoundApp.ViewModels
{
    public partial class ProductSearchViewModel : BaseViewModel
    {
        public ObservableCollection<ProductModel> ProductList { get; } = new();

        readonly ProductService productService;

        public ProductSearchViewModel(ProductService productService)
        {
            this.productService = productService;

            Title = "Product Search";
        }

        [RelayCommand]
        async Task GoToProductListingAsync(ProductModel product)
        {
            if (product is null)
                return;

            await Shell.Current.GoToAsync($"{nameof(ProductListingPage)}", true,
                new Dictionary<string, object>
                {
                    {"Product", product}
                });
        }

        [RelayCommand]
        async Task GetProductsAsync()
        {
            // Check if the view model is already busy fetching data
            // If it is, we don't want to start another fetch operation.
            // This prevents multiple simultaneous fetch operations which could
            // lead to performance issues or unexpected behavior.
            // This is a common pattern to avoid re-entrancy issues in async methods.
            if (IsBusy)
                return;

            // Set IsBusy to true to indicate that a fetch operation is in progress.
            // This will typically disable UI elements that should not be interacted with
            // while the data is being fetched, providing a better user experience.
            // This is important to ensure that the UI reflects that data is being loaded,
            // and to prevent the user from initiating another fetch operation while one is already in progress.
            try
            {
                IsBusy = true;
                // Fetch products from the product service.
                // This method is expected to return a list of products asynchronously.
                // The fetched products will be added to the Products collection.
                // If the product service has a method to fetch products via HTTP, we call that.
                // If you want to fetch products from an embedded JSON file instead, you can call:
                // var products = await productService.GetProductsEmbedded();
                var productList = await productService.GetProductsHttp();

                // Clear the existing products in the ObservableCollection.
                // This ensures that the collection is updated with the new products fetched.
                // ObservableCollection is used here so that the UI can automatically update
                // when items are added or removed, without needing to manually refresh the UI.
                // This is a key feature of ObservableCollection, which is designed for data binding in UI frameworks.
                // The foreach loop iterates over the fetched products and adds each one to the Products collection.
                // This is done to ensure that the UI reflects the latest data.
                // The Products collection is an ObservableCollection, which means that any changes to it
                // will automatically notify the UI to update, making it easy to display dynamic data.
                // This is particularly useful in MVVM (Model-View-ViewModel) patterns where the ViewModel
                // holds the data and the View binds to it.
                ProductList.Clear();
                foreach (var product in productList)
                {
                    // Add each product to the ObservableCollection.
                    // This will trigger the UI to update and display the new products.
                    // The ObservableCollection is designed to notify the UI of changes,
                    // so when we add items to it, the UI will automatically reflect those changes.
                    // ObservableRangeCollection would more efficient if you want to add multiple items at once,
                    // and delay the UI update until all items are added.
                    ProductList.Add(product);
                }
            }
            catch (Exception ex)
            { 
                // Handle exceptions, e.g., log them or show an alert to the user.
                Console.WriteLine($"Error fetching data: {ex.Message}");

                await Shell.Current.DisplayAlert(
                    "Error",
                    "An error occurred while attempting to fetch data",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        Task ClearProducts()
        {
            // This command clears the Products collection.
            // It is typically used to reset the product list, for example, when the user wants to start a new search
            // or when the application needs to refresh the data.
            // The ObservableCollection will notify the UI that it has been cleared,
            // so any bound UI elements will update accordingly.
            ProductList.Clear();

            return Task.CompletedTask;
        }
    }
}
