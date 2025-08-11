using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SquoundApp.Pages;
using SquoundApp.Services;
using SquoundApp.States;

using Shared.DataTransfer;


namespace SquoundApp.ViewModels
{
    public partial class SortAndFilterViewModel : BaseViewModel
    {
        // Singleton state managing the current search parameters.
        // This state can be accessed from anywhere in the application to retrieve or reset the current search parameters.
        private readonly SearchState m_SearchState;


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
        private string subcategory = string.Empty;

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
        /// Constructor for the SortAndFilterViewModel class.
        /// </summary>
        /// <param name="searchState">The <see cref="SearchState"/> instance used
        /// to manage the current search parameters. Cannot be null.</param>
        public SortAndFilterViewModel(SearchState searchState)
        {
            m_SearchState = searchState ?? throw new ArgumentNullException(nameof(searchState));
        }


        // Partial methods to handle changes in filter properties.
        partial void OnCategoryChanged(string value)
        {
            m_SearchState.Category = value;
        }
        partial void OnSubcategoryChanged(string value)
        {
            m_SearchState.Subcategory = value;
        }
        partial void OnManufacturerChanged(string value)
        {
            m_SearchState.Manufacturer = value;
        }
        partial void OnKeywordChanged(string value)
        {
            m_SearchState.Keyword = value;
        }
        partial void OnMinimumPriceChanged(string value)
        {
            // Extract only the digits from the string.
            var digits = new string(value.Where(char.IsDigit).ToArray());

            // Try to parse the digits to a decimal.
            if (decimal.TryParse(digits, out decimal price))
            {
                // If the parsed price is out of the valid range.
                if (price < 0 || price > (decimal)SearchQueryDto.PracticalMaximumPrice)
                {
                    m_SearchState.MinPrice = null;
                }

                // If the parsed price is greater than the maximum price.
                // Set both minimum and maximum prices to the parsed price.
                else if (price > m_SearchState.MaxPrice)
                {
                    m_SearchState.MinPrice = price;
                    m_SearchState.MaxPrice = price;

                    RestoreQueryToUserInterface(m_SearchState.CopyOfCurrentQuery);
                }

                // If the parsed price is valid and within the range.
                else
                {
                    m_SearchState.MinPrice = price;
                }
            }

            else
            {
                // If parsing fails.
                m_SearchState.MinPrice = null;
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
                if (price < 0 || price > (decimal)SearchQueryDto.PracticalMaximumPrice)
                {
                    m_SearchState.MaxPrice = null;
                }

                // If the parsed price is less than the minimum price.
                // Set both minimum and maximum prices to the parsed price.
                else if (price < m_SearchState.MinPrice)
                {
                    m_SearchState.MinPrice = price;
                    m_SearchState.MaxPrice = price;

                    RestoreQueryToUserInterface(m_SearchState.CopyOfCurrentQuery);
                }

                // If the parsed price is valid and within the range.
                else
                {
                    m_SearchState.MaxPrice = price;
                }
            }

            else
            {
                // If parsing fails.
                m_SearchState.MaxPrice = null;
            }
        }
        partial void OnSortByNameAscendingChanged(bool value)
        {
            m_SearchState.SortBy = value ? ItemSortOption.NameAsc : m_SearchState.SortBy;
        }
        partial void OnSortByNameDescendingChanged(bool value)
        {
            m_SearchState.SortBy = value ? ItemSortOption.NameDesc : m_SearchState.SortBy;
        }
        partial void OnSortByPriceAscendingChanged(bool value)
        {
            m_SearchState.SortBy = value ? ItemSortOption.PriceAsc : m_SearchState.SortBy;
        }
        partial void OnSortByPriceDescendingChanged(bool value)
        {
            m_SearchState.SortBy = value ? ItemSortOption.PriceDesc : m_SearchState.SortBy;
        }


        // Methods responsible for setting the sort options.
        [RelayCommand]
        private void SetSortOption(ItemSortOption sortBy)
        {
            // Reset all sort options to false.
            SortByNameAscending = false;
            SortByNameDescending = false;
            SortByPriceAscending = false;
            SortByPriceDescending = false;

            // Set the appropriate sort option based on the provided sortBy parameter.
            switch (sortBy)
            {
                case ItemSortOption.NameAsc:
                    SortByNameAscending = true;
                    break;

                case ItemSortOption.NameDesc:
                    SortByNameDescending = true;
                    break;

                case ItemSortOption.PriceAsc:
                    SortByPriceAscending = true;
                    break;

                case ItemSortOption.PriceDesc:
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
        /// Shows and activates the sort menu, allowing users to choose how they want to sort the items.
        /// </summary>
        private void ShowSortMenu()
        {
            Title = "Sort Options";

            IsTitleLabelVisible     = true;
            IsFilterButtonActive    = false;
            IsSortButtonActive      = false;
            IsFilterMenuActive      = false;
            IsSortMenuActive        = true;
        }


        /// <summary>
        /// Shows and activates the filter menu, allowing users to refine their search criteria.
        /// </summary>
        private void ShowFilterMenu()
        {
            Title = "Filter Options";

            IsTitleLabelVisible     = true;
            IsFilterButtonActive    = false;
            IsSortButtonActive      = false;
            IsFilterMenuActive      = true;
            IsSortMenuActive        = false;
        }


        /// <summary>
        /// Hides and disables the sort and filter menus, resetting the user interface to its default state.
        /// </summary>
        private void HideMenus()
        {
            Title = string.Empty;

            IsTitleLabelVisible     = false;
            IsFilterButtonActive    = true;
            IsSortButtonActive      = true;
            IsFilterMenuActive      = false;
            IsSortMenuActive        = false;
        }


        /// <summary>
        /// Handles the sort button click event.
        /// </summary>
        [RelayCommand]
        private void OnSortButton()
        {
            ShowSortMenu();
        }


        /// <summary>
        /// Handles the filter button click event.
        /// </summary>
        [RelayCommand]
        private void OnFilterButton()
        {
            ShowFilterMenu();
        }


        /// <summary>
        /// Handles the apply sort and apply filter button click events.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private async Task ApplyQueryAsync()
        {
            HideMenus();

            //await Shell.Current.GoToAsync(nameof(RefinedSearchPage));

            var navService = ServiceLocator.GetService<NavigationService>();
            await navService.GoToAsync(nameof(RefinedSearchPage));
        }


        /// <summary>
        /// Handles the cancel sort and cancel filter button click events.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private void OnCancelButton()
        {
            // Present the previous search state to the user.
            RestoreQueryToUserInterface(m_SearchState.CopyOfPreviousQuery);

            HideMenus();
        }


        /// <summary>
        /// Handles the reset button click event.
        /// </summary>
        /// <returns></returns>
        [RelayCommand]
        private void OnResetButton()
        {
            m_SearchState.ResetSearch();

            // Present the newly reset search state to the user.
            RestoreQueryToUserInterface(m_SearchState.CopyOfCurrentQuery);
        }


        /// <summary>
        /// Updates the user interface with the provided <see cref="SearchQueryDto"/>.
        /// This ensures that the user interface reflects the given search criteria.
        /// </summary>
        /// <param name="query">The <see cref="SearchQueryDto"/> containing the search criteria to restore.</param>
        private void RestoreQueryToUserInterface(SearchQueryDto query)
        {
            // NOTE : Null checks are necessary to avoid NullReferenceException.
            this.Keyword        = query.Keyword ?? string.Empty;
            this.Category       = query.Category ?? string.Empty;
            this.Subcategory    = query.Subcategory ?? string.Empty;
            this.Manufacturer   = query.Manufacturer ?? string.Empty;

            // NOTE : Null checks are not actually required because ToString() would return an empty string if the value is null.
            this.MinimumPrice   = query.MinPrice.ToString() ?? string.Empty;
            this.MaximumPrice   = query.MaxPrice.ToString() ?? string.Empty;

            // Set the sort options based on the query.
            SetSortOption(query.SortBy);
        }
    }
}