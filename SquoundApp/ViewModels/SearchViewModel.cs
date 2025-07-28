using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;
using System.Diagnostics;

using SquoundApp.Pages;
using SquoundApp.Services;
using SquoundApp.States;

using Shared.DataTransfer;
using Shared.StateMachine;


namespace SquoundApp.ViewModels
{
    public partial class SearchViewModel : BaseViewModel
    {
        // Query DTO that holds the most recently applied search criteria.
        private ProductQueryDto PreviousQuery { get; set; } = new();

        // Query DTO that holds the current but not yet applied search criteria.
        public ProductQueryDto CurrentQuery { get; private set; } = new();


        // Variables for filter options.
        [ObservableProperty]
        private string category = string.Empty;

        [ObservableProperty]
        private string manufacturer = string.Empty;

        [ObservableProperty]
        private string keyword = string.Empty;

        [ObservableProperty]
        private string minimumPrice = string.Empty;

        [ObservableProperty]
        private string maximumPrice = string.Empty;

        [ObservableProperty]
        private bool sortByNameAscending = false;

        [ObservableProperty]
        private bool sortByNameDescending = false;

        [ObservableProperty]
        private bool sortByPriceAscending = true;

        [ObservableProperty]
        private bool sortByPriceDescending = false;


        // Partial methods to handle changes in filter properties.
        partial void OnCategoryChanged(string value)
        {
            CurrentQuery.Category = value;
        }
        partial void OnManufacturerChanged(string value)
        {
            CurrentQuery.Manufacturer = value;
        }
        partial void OnKeywordChanged(string value)
        {
            CurrentQuery.Keyword = value;
        }
        partial void OnMinimumPriceChanged(string value)
        {
            // Extract only the digits from the string.
            var digits = new string(value.Where(char.IsDigit).ToArray());

            // Try to parse the digits to a decimal.
            if (decimal.TryParse(digits, out decimal price))
            {
                // If the parsed price is out of the valid range.
                if (price < 0 || price > (decimal)ProductQueryDto.PracticalMaximumPrice)
                {
                    CurrentQuery.MinPrice = null;
                }

                // If the parsed price is greater than the maximum price.
                // Set both minimum and maximum prices to the parsed price.
                else if (price > CurrentQuery.MaxPrice)
                {
                    CurrentQuery.MinPrice = price;
                    CurrentQuery.MaxPrice = price;

                    RestoreQueryToUserInterface(CurrentQuery);
                }

                // If the parsed price is valid and within the range.
                else
                {
                    CurrentQuery.MinPrice = price;
                }
            }

            else
            {
                // If parsing fails.
                CurrentQuery.MinPrice = null;
            }
        }
        partial void OnMaximumPriceChanged(string value)
        {
            // Extract only the digits from the string.
            var digits = new string(value.Where(char.IsDigit).ToArray());

            // Try to parse the digits to a decimal.
            if (decimal.TryParse(digits, out decimal price))
            {
                // If the parsed price is out of the valid range.
                if (price < 0 || price > (decimal)ProductQueryDto.PracticalMaximumPrice)
                {
                    CurrentQuery.MaxPrice = null;
                }

                // If the parsed price is less than the minimum price.
                // Set both minimum and maximum prices to the parsed price.
                else if (price < CurrentQuery.MinPrice)
                {
                    CurrentQuery.MinPrice = price;
                    CurrentQuery.MaxPrice = price;

                    RestoreQueryToUserInterface(CurrentQuery);
                }

                // If the parsed price is valid and within the range.
                else
                {
                    CurrentQuery.MaxPrice = price;
                }
            }

            else
            {
                // If parsing fails.
                CurrentQuery.MaxPrice = null;
            }
        }
        partial void OnSortByNameAscendingChanged(bool value)
        {
            CurrentQuery.SortBy = value ? ProductSortOption.NameAsc : CurrentQuery.SortBy;
        }
        partial void OnSortByNameDescendingChanged(bool value)
        {
            CurrentQuery.SortBy = value ? ProductSortOption.NameDesc : CurrentQuery.SortBy;
        }
        partial void OnSortByPriceAscendingChanged(bool value)
        {
            CurrentQuery.SortBy = value ? ProductSortOption.PriceAsc : CurrentQuery.SortBy;
        }
        partial void OnSortByPriceDescendingChanged(bool value)
        {
            CurrentQuery.SortBy = value ? ProductSortOption.PriceDesc : CurrentQuery.SortBy;
        }


        // Variables responsible for showing and hiding the sort and filter menus.
        [ObservableProperty]
        private bool isTitleLabelVisible = true;

        [ObservableProperty]
        private bool isSortButtonActive = true;

        [ObservableProperty]
        private bool isFilterButtonActive = true;

        [ObservableProperty]
        private bool isSortMenuActive = false;

        [ObservableProperty]
        private bool isFilterMenuActive = false;


        /// <summary>
        /// Collection of products matched by the search criteria.
        /// </summary>
        public ObservableCollection<ProductDto> ProductList { get; } = new();


        /// <summary>
        /// Represents the state machine used to manage the states and transitions of the <see cref="SearchViewModel"/>.
        /// </summary>
        /// <remarks>This state machine is responsible for controlling the behavior and lifecycle of the
        /// <see cref="SearchViewModel"/>. It ensures that the view model transitions between states in a predictable
        /// and controlled manner.</remarks>
        private readonly StateMachine<SearchViewModel> stateMachine;


        /// <summary>
        /// Responsible for retrieving products from the REST API.
        /// </summary>
        private readonly ProductService productService;


        /// <summary>
        /// Initializes a new instance of the <see cref="SearchViewModel"/> class
        /// with the specified product service.
        /// </summary>
        /// <param name="service">The <see cref="ProductService"/> instance used
        /// to retrieve product data. Cannot be null.</param>
        public SearchViewModel(ProductService service)
        {
            stateMachine = new StateMachine<SearchViewModel>(this);

            stateMachine.ChangeState(new SearchViewModelIdleState());

            productService = service;
        }


        // Methods responsible for setting the sort options.
        [RelayCommand]
        private void SetSortOption(ProductSortOption sortBy)
        {
            // Reset all sort options to false.
            SortByNameAscending     = false;
            SortByNameDescending    = false;
            SortByPriceAscending    = false;
            SortByPriceDescending   = false;

            // Set the appropriate sort option based on the provided sortBy parameter.
            switch (sortBy)
            {
                case ProductSortOption.NameAsc:
                    SortByNameAscending = true;
                    break;

                case ProductSortOption.NameDesc:
                    SortByNameDescending = true;
                    break;

                case ProductSortOption.PriceAsc:
                    SortByPriceAscending = true;
                    break;

                case ProductSortOption.PriceDesc:
                    SortByPriceDescending = true;
                    break;

                // If no valid sort option is provided, default to sorting by price ascending.
                default:
                    SortByPriceAscending = true;
                    break;
            }
        }

        [RelayCommand]
        private void SetSortOptionAsNameAscending()
        {
            SortByNameAscending = true;
            SortByNameDescending = false;
            SortByPriceAscending = false;
            SortByPriceDescending = false;
        }

        [RelayCommand]
        private void SetSortOptionAsNameDescending()
        {
            SortByNameAscending = false;
            SortByNameDescending = true;
            SortByPriceAscending = false;
            SortByPriceDescending = false;
        }

        [RelayCommand]
        private void SetSortOptionAsPriceAscending()
        {
            SortByNameAscending = false;
            SortByNameDescending = false;
            SortByPriceAscending = true;
            SortByPriceDescending = false;
        }

        [RelayCommand]
        private void SetSortOptionAsPriceDescending()
        {
            SortByNameAscending = false;
            SortByNameDescending = false;
            SortByPriceAscending = false;
            SortByPriceDescending = true;
        }


        // Methods responsible for showing and hiding the sort and filter menus.
        [RelayCommand]
        private void OnSortButton()
        {
            //Title = "Sort Options";

            //IsTitleLabelVisible     = true;

            //IsSortMenuActive        = true;
            //IsFilterMenuActive      = false;

            //IsSortButtonActive      = false;
            //IsFilterButtonActive    = false;

            stateMachine.ChangeState(new SearchViewModelSortMenuState());
        }


        //
        [RelayCommand]
        private void OnFilterButton()
        {
            //Title = "Filter Options";

            //IsTitleLabelVisible     = true;

            //IsSortMenuActive        = false;
            //IsFilterMenuActive      = true;

            //IsSortButtonActive      = false;
            //IsFilterButtonActive    = false;

            stateMachine.ChangeState(new SearchViewModelFilterMenuState());
        }


        // Methods responsible for applying and canceling the sort and filter options.
        [RelayCommand]
        private async Task ApplyQueryAsync()
        {
            // Save a deep copy of the current query before applying the new sort preferences.
            PreviousQuery = new ProductQueryDto
            {
                Category            = CurrentQuery.Category,
                Manufacturer        = CurrentQuery.Manufacturer,
                Keyword             = CurrentQuery.Keyword,
                MinPrice            = CurrentQuery.MinPrice,
                MaxPrice            = CurrentQuery.MaxPrice,
                SortBy              = CurrentQuery.SortBy
            };

            // Trigger the product retrieval based on the current sort and filter options.
            await GetProductsAsync();

            stateMachine.ChangeState(new SearchViewModelIdleState());
        }


        //
        [RelayCommand]
        private void OnCancelSort()
        {
            // Restore the sort option from the previous query.
            RestoreQueryToUserInterface(PreviousQuery);

            stateMachine.ChangeState(new SearchViewModelIdleState());
        }


        //
        [RelayCommand]
        private void OnCancelFilter()
        {
            // Restore the previous query to the current query.
            RestoreQueryToUserInterface(PreviousQuery);

            stateMachine.ChangeState(new SearchViewModelIdleState());
        }


        //
        [RelayCommand]
        private void OnResetFilter()
        {
            CurrentQuery.Keyword        = null;
            CurrentQuery.Category       = null;
            CurrentQuery.Manufacturer   = null;
            CurrentQuery.MinPrice       = null;
            CurrentQuery.MaxPrice       = null;

            RestoreQueryToUserInterface(CurrentQuery);
        }


        //
        //private void ResetMenus()
        //{
        //    // Hide the menu label.
        //    IsTitleLabelVisible     = false;

        //    // Deactivate and hide the sort and filter menus.
        //    IsSortMenuActive        = false;
        //    IsFilterMenuActive      = false;

        //    // Reactivate and show the sort and filter buttons.
        //    IsSortButtonActive      = true;
        //    IsFilterButtonActive    = true;
        //}


        //
        private void RestoreQueryToUserInterface(ProductQueryDto query)
        {
            // NOTE : Null checks are necessary to avoid NullReferenceException.
            Keyword         = query.Keyword != null ? query.Keyword : string.Empty;
            Category        = query.Category != null ? query.Category : string.Empty;
            Manufacturer    = query.Manufacturer != null ? query.Manufacturer : string.Empty;

            // NOTE : Null checks are unnecessary here, because ToString() will return an empty string if the value is null.
            MinimumPrice    = query.MinPrice.ToString();
            MaximumPrice    = query.MaxPrice.ToString();

            // Set the sort options based on the query.
            SetSortOption(query.SortBy);
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

            stateMachine.ChangeState(new SearchViewModelIdleState());

            await Shell.Current.GoToAsync($"{nameof(ProductListingPage)}", true,
                new Dictionary<string, object>
                {
                    {"Product", product}
                });
        }


        /// <summary>
        /// Initiates a fetch operation to retrieve products based on the specified category.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        async Task GetProductsAsync()
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
                var productList = await productService.GetProductsRestApi(CurrentQuery);

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


        //
        //private ProductQueryDto BuildQuery()
        //{
        //    // Create a new instance of ProductQueryDto to hold the search criteria.
        //    var query = new ProductQueryDto
        //    {
        //        Category = string.IsNullOrEmpty(this.Category) ? null : this.Category,
        //        Keyword = string.IsNullOrEmpty(this.FilterKeyword) ? null : this.FilterKeyword,
        //        MinPrice = string.IsNullOrEmpty(this.FilterMinimumPrice) ? 0 : decimal.Parse(this.FilterMinimumPrice),
        //        MaxPrice = string.IsNullOrEmpty(this.FilterMaximumPrice) ? (decimal)ProductQueryDto.PracticalMaximumPrice : decimal.Parse(this.FilterMaximumPrice),
        //        PageNumber = 1,     // Default to the first page.
        //        PageSize = 10,      // Default page size.
        //    };

        //    // Determine the sort option based on the selected properties.
        //    if (IsSortOptionNameAscending)
        //    {
        //        query.SortBy = ProductSortOption.NameAsc;
        //    }

        //    else if (IsSortOptionNameDescending)
        //    {
        //        query.SortBy = ProductSortOption.NameDesc;
        //    }

        //    else if (IsSortOptionPriceAscending)
        //    {
        //        query.SortBy = ProductSortOption.PriceAsc;
        //    }

        //    else if (IsSortOptionPriceDescending)
        //    {
        //        query.SortBy = ProductSortOption.PriceDesc;
        //    }

        //    return query;
        //}
    }
}
