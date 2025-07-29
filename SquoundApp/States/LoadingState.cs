using System.Diagnostics;

using Shared.DataTransfer;
using Shared.Interfaces;

using SquoundApp.ViewModels;


namespace SquoundApp.States
{
    internal class LoadingState : IState<SearchViewModel>
    {
        public async Task Enter(SearchViewModel vm)
        {
            // Check if the view model is already busy fetching data
            // This prevents multiple simultaneous fetch operations which could
            // lead to performance issues or unexpected behavior.
            // This is a common pattern to avoid re-entrancy issues in async methods.
            if (vm.IsBusy)
                return;

            try
            {
                // Set IsBusy to true to indicate that a fetch operation is in progress.
                // This will typically disable UI elements that should not be interacted with
                // while the data is being fetched, providing a better user experience.
                // This is important to ensure that the UI reflects that data is being loaded,
                // and to prevent the user from initiating another fetch operation while one is already in progress.
                vm.IsBusy = true;

                // Save a deep copy of the current query to the PreviousQuery property.
                vm.PreviousQuery = new ProductQueryDto
                {
                    Category        = vm.CurrentQuery.Category,
                    Manufacturer    = vm.CurrentQuery.Manufacturer,
                    Keyword         = vm.CurrentQuery.Keyword,
                    MinPrice        = vm.CurrentQuery.MinPrice,
                    MaxPrice        = vm.CurrentQuery.MaxPrice,
                    SortBy          = vm.CurrentQuery.SortBy
                };

                // Clear the existing products in the ObservableCollection.
                // This ensures that the collection is updated with whatever is returned by the API.
                // ObservableCollection is used here so that the UI can automatically update
                // when items are added or removed, without needing to manually refresh the UI.
                // This is a key feature of ObservableCollection, which is designed for data binding in UI frameworks.
                vm.ProductList.Clear();

                // Retrieve products from the product service.
                // This method is expected to return a list of products asynchronously.
                // The retrieved products will be added to the productList collection.
                // To retrieve products from a remote JSON file, use:
                // var productList = await productService.GetProductsRemoteJson
                // ("https://raw.githubusercontent.com/bushack/files/refs/heads/main/products.json");
                // To retrieve products from an embedded JSON file instead, use:
                // var productList = await productService.GetProductsEmbeddedJson();
                var productList = await vm.ProductService.GetProductsRestApi(vm.CurrentQuery);

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
                    vm.ProductList.Add(product);
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
                vm.IsBusy = false;
            }
        }


        public Task Update(SearchViewModel viewModel)
        {
            return Task.CompletedTask;
        }


        public Task Exit(SearchViewModel viewModel)
        {
            return Task.CompletedTask;
        }
    }
}