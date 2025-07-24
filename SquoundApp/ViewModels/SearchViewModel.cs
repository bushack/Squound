using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;
using System.Diagnostics;

using SquoundApp.Services;
using SquoundApp.Pages;

using Shared.DataTransfer;


namespace SquoundApp.ViewModels
{
    public partial class SearchViewModel : BaseViewModel
    {
        // Variables involved in showing and hiding the sort and filter menus.
        [ObservableProperty]
        public bool isTitleLabelVisible = true;

        [ObservableProperty]
        public bool isSortButtonActive = true;

        [ObservableProperty]
        public bool isFilterButtonActive = true;

        [ObservableProperty]
        public bool isSortMenuActive = false;

        [ObservableProperty]
        public bool isFilterMenuActive = false;


        // Variables for sort options.
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsSortOptionNotNameAscending))]
        public bool isSortOptionNameAscending = false;
        public bool IsSortOptionNotNameAscending => !IsSortOptionNameAscending;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsSortOptionNotNameDescending))]
        public bool isSortOptionNameDescending = false;
        public bool IsSortOptionNotNameDescending => !IsSortOptionNameDescending;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsSortOptionNotPriceAscending))]
        public bool isSortOptionPriceAscending = true;
        public bool IsSortOptionNotPriceAscending => !IsSortOptionPriceAscending;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsSortOptionNotPriceDescending))]
        public bool isSortOptionPriceDescending = false;
        public bool IsSortOptionNotPriceDescending => !IsSortOptionPriceDescending;


        // Variables for filter options.
        [ObservableProperty]
        public string category = "";

        [ObservableProperty]
        public string filterKeyword = "";

        [ObservableProperty]
        public string filterMinimumPrice = "";

        [ObservableProperty]
        public string filterMaximumPrice = "";


        /// <summary>
        /// Collection of products matched by the search criteria.
        /// </summary>
        public ObservableCollection<ProductDto> ProductList { get; } = new();

        /// <summary>
        /// Responsible for retrieving products from the REST API.
        /// </summary>
        readonly ProductService productService;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="productService"></param>
        public SearchViewModel(ProductService productService)
        {
            this.productService = productService;

            Title = "Search";
        }

        /// <summary>
        /// OnChanged methods are called AFTER the value has been set.
        /// </summary>
        /// <param name="value">The value applied to isSortOptionNameAscending.</param>
        partial void OnIsSortOptionNameAscendingChanged(bool value)
        {
            if (value)
            {
            }
        }

        /// <summary>
        /// OnChanged methods are called AFTER the value has been set.
        /// </summary>
        /// <param name="value">The value applied to isSortOptionNameDescending.</param>
        partial void OnIsSortOptionNameDescendingChanged(bool value)
        {
            if (value)
            {
            }
        }

        /// <summary>
        /// OnChanged methods are called AFTER the value has been set.
        /// </summary>
        /// <param name="value">The value applied to isSortOptionPriceAscending.</param>
        partial void OnIsSortOptionPriceAscendingChanged(bool value)
        {
            if (value)
            {
            }
        }

        /// <summary>
        /// OnChanged methods are called AFTER the value has been set.
        /// </summary>
        /// <param name="value">The value applied to isSortOptionPriceDescending.</param>
        partial void OnIsSortOptionPriceDescendingChanged(bool value)
        {
            if (value)
            {
            }
        }

        [RelayCommand]
        private void SetSortOptionAsNameAscending()
        {
            IsSortOptionNameAscending = true;
            IsSortOptionNameDescending = false;
            IsSortOptionPriceAscending = false;
            IsSortOptionPriceDescending = false;
        }

        [RelayCommand]
        private void SetSortOptionAsNameDescending()
        {
            IsSortOptionNameAscending = false;
            IsSortOptionNameDescending = true;
            IsSortOptionPriceAscending = false;
            IsSortOptionPriceDescending = false;
        }

        [RelayCommand]
        private void SetSortOptionAsPriceAscending()
        {
            IsSortOptionNameAscending = false;
            IsSortOptionNameDescending = false;
            IsSortOptionPriceAscending = true;
            IsSortOptionPriceDescending = false;
        }

        [RelayCommand]
        private void SetSortOptionAsPriceDescending()
        {
            IsSortOptionNameAscending = false;
            IsSortOptionNameDescending = false;
            IsSortOptionPriceAscending = false;
            IsSortOptionPriceDescending = true;
        }

        [RelayCommand]
        private void OnSortButton()
        {
            Title = "Sort Options";

            IsTitleLabelVisible = true;

            IsSortButtonActive = false;
            IsFilterButtonActive = false;

            IsSortMenuActive = true;
            IsFilterMenuActive = false;
        }

        [RelayCommand]
        private void OnFilterButton()
        {
            Title = "Filter Options";

            IsTitleLabelVisible = true;

            IsSortButtonActive = false;
            IsFilterButtonActive = false;

            IsSortMenuActive = false;
            IsFilterMenuActive = true;
        }

        [RelayCommand]
        private void OnApplySort()
        {
            IsTitleLabelVisible = false;

            IsSortButtonActive = true;
            IsFilterButtonActive = true;

            IsSortMenuActive = false;
            IsFilterMenuActive = false;
        }

        [RelayCommand]
        private void OnCancelSort()
        {
            IsTitleLabelVisible = false;

            IsSortButtonActive = true;
            IsFilterButtonActive = true;

            IsSortMenuActive = false;
            IsFilterMenuActive = false;
        }

        [RelayCommand]
        private void OnApplyFilter()
        {
            IsTitleLabelVisible = false;

            IsSortButtonActive = true;
            IsFilterButtonActive = true;

            IsSortMenuActive = false;
            IsFilterMenuActive = false;
        }

        [RelayCommand]
        private void OnResetFilter()
        {
            FilterKeyword = "";
            FilterMinimumPrice = "";
            FilterMaximumPrice = "";
        }

        [RelayCommand]
        private void OnCancelFilter()
        {
            IsTitleLabelVisible = false;

            IsSortButtonActive = true;
            IsFilterButtonActive = true;

            IsSortMenuActive = false;
            IsFilterMenuActive = false;
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

            await Shell.Current.GoToAsync($"{nameof(ProductListingPage)}", true,
                new Dictionary<string, object>
                {
                    {"Product", product}
                });
        }


        /// <summary>
        /// Initiates a fetch operation to retrieve products based on the specified category.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [RelayCommand]
        async Task GetProductsAsync(string category)
        {
            // Check if the view model is already busy fetching data
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

                // Retrieve products from the product service.
                // This method is expected to return a list of products asynchronously.
                // The retrieved products will be added to the productList collection.
                // To retrieve products from a remote JSON file, use:
                // var productList = await productService.GetProductsRemoteJson();
                // To retrieve products from an embedded JSON file instead, use:
                // var productList = await productService.GetProductsEmbeddedJson();
                var productList = await productService.GetProductsRestApi(category);

                if (productList == null)
                    return;

                // Clear the existing products in the ObservableCollection.
                // This ensures that the collection is updated with the new products fetched.
                // ObservableCollection is used here so that the UI can automatically update
                // when items are added or removed, without needing to manually refresh the UI.
                // This is a key feature of ObservableCollection, which is designed for data binding in UI frameworks.
                // The foreach loop iterates over the fetched products and adds each one to the Products collection.
                // This ensures that the UI reflects the latest data.
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
    }
}
