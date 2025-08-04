using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;

using SquoundApp.Pages;
using SquoundApp.Services;
using SquoundApp.States;

using Shared.DataTransfer;
using Shared.StateMachine;


namespace SquoundApp.ViewModels
{
    public partial class RefinedSearchViewModel : BaseViewModel
    {
        /// <summary>
        /// Represents the state machine used to manage the states and transitions of the <see cref="RefinedSearchViewModel"/>.
        /// </summary>
        /// <remarks>This state machine is responsible for controlling the behavior and lifecycle of the
        /// <see cref="RefinedSearchViewModel"/>. It ensures that the view model transitions between states in a predictable
        /// and controlled manner.</remarks>
        internal readonly StateMachine<RefinedSearchViewModel> StateMachine;

        // Responsible for retrieving data from the REST API.
        internal readonly ProductService ProductService;

        // Collection of products matching the search criteria.
        public ObservableCollection<ProductDto> ProductList { get; } = [];

        // Query DTO that holds the most recently applied search criteria.
        public ProductQueryDto PreviousQuery { get; internal set; } = new();

        // Query DTO that holds the current but not yet applied search criteria.
        public ProductQueryDto CurrentQuery { get; private set; } = new();


        // Variables responsible for adjusting the sort options.
        [ObservableProperty]
        private bool sortByNameAscending = false;

        [ObservableProperty]
        private bool sortByNameDescending = false;

        [ObservableProperty]
        private bool sortByPriceAscending = true;

        [ObservableProperty]
        private bool sortByPriceDescending = false;


        // Variables responsible for adjusting the filter options.
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
        /// Initializes a new instance of the <see cref="RefinedSearchViewModel"/> class
        /// with the specified product service.
        /// </summary>
        /// <param name="service">The <see cref="ProductService"/> instance used
        /// to retrieve product data. Cannot be null.</param>
        public RefinedSearchViewModel(ProductService service)
        {
            StateMachine = new StateMachine<RefinedSearchViewModel>(this);

            StateMachine.ChangeState(new IdleState());

            ProductService = service;
        }


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


        /// <summary>
        /// Handles the sort button click event.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task OnSortButton()
        {
            // Activate the sort menu.
            await StateMachine.ChangeState(new SortState());
        }


        /// <summary>
        /// Handles the filter button click event.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task OnFilterButton()
        {
            // Activate the filter menu.
            await StateMachine.ChangeState(new FilterState());
        }


        /// <summary>
        /// Handles the apply sort and apply filter button click events.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task ApplyQueryAsync()
        {
            // Trigger the product retrieval based on the current sort and filter options.
            await StateMachine.ChangeState(new LoadingState());

            // Return to the idle state after applying the query.
            await StateMachine.ChangeState(new IdleState());
        }


        /// <summary>
        /// Handles the cancel sort and cancel filter button click events.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task OnCancelButton()
        {
            await StateMachine.ChangeState(new CancelState());

            await StateMachine.ChangeState(new IdleState());
        }


        /// <summary>
        /// Handles the reset button click event.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task OnResetButton()
        {
            await StateMachine.ChangeState(new ResetState());

            await StateMachine.ChangeState(StateMachine.PreviousState);
        }


        //
        internal void RestoreQueryToUserInterface(ProductQueryDto query)
        {
            // NOTE : Null checks are necessary to avoid NullReferenceException.
            Keyword         = query.Keyword ?? string.Empty;
            Category        = query.Category ?? string.Empty;
            Manufacturer    = query.Manufacturer ?? string.Empty;

            // NOTE : Null checks are unnecessary because ToString() will return an empty string if the value is null.
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

            // Close the sort and filter menus if they are active.
            await StateMachine.ChangeState(new IdleState());

            // Navigate to the ProductListingPage and pass the selected product as a parameter.
            await Shell.Current.GoToAsync($"{nameof(ProductListingPage)}", true,
                new Dictionary<string, object>
                {
                    {"Product", product}
                });
        }
    }
}
