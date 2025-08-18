using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SquoundApp.ViewModels;

using Shared.DataTransfer;


namespace SquoundApp.States
{
    public partial class SearchService : ObservableObject
    {
        /// <summary>
        /// Internal class responsible for caching current and previous search parameters.
        /// To submit a search query to the API, first request a SearchQueryDto using the
        /// method 'ToSearchQueryDto'. This object can then be used to produce the query
        /// string section of a URL that can be submitted to a REST API GET endpoint with
        /// the method 'AsQueryString'.
        /// </summary>
        private class SearchState
        {
            public DateTime TimeStamp;
            public CategoryDto? Category { get; set; } = null;
            public SubcategoryDto? Subcategory { get; set; } = null;

            public string? Manufacturer { get; set; } = null;
            public string? Material { get; set; } = null;
            public string? Keyword { get; set; } = null;

            public decimal? MinPrice { get; set; } = null;
            public decimal? MaxPrice { get; set; } = null;

            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 10;

            public ItemSortOption SortBy { get; set; } = ItemSortOption.PriceAsc;


            public SearchState Clone()
            {
                return new SearchState
                {
                    TimeStamp       = this.TimeStamp,
                    Category        = this.Category,
                    Subcategory     = this.Subcategory,
                    Manufacturer    = this.Manufacturer,
                    Material        = this.Material,
                    Keyword         = this.Keyword,
                    MinPrice        = this.MinPrice,
                    MaxPrice        = this.MaxPrice,
                    PageNumber      = this.PageNumber,
                    PageSize        = this.PageSize,
                    SortBy          = this.SortBy
                };
            }


            public SearchQueryDto AsSearchQueryDto()
            {
                return new SearchQueryDto
                {
                    ItemId          = null,
                    Category        = this.Category?.Name ?? null,
                    Subcategory     = this.Subcategory?.Name ?? null,
                    Manufacturer    = this.Manufacturer ?? null,
                    Material        = this.Material ?? null,
                    Keyword         = this.Keyword ?? null,
                    MinPrice        = this.MinPrice ?? null,
                    MaxPrice        = this.MaxPrice ?? null,
                    PageNumber      = this.PageNumber,
                    PageSize        = this.PageSize,
                    SortBy          = this.SortBy
                };
            }
        }


        private SearchState _currentState = new();
        private SearchState _previousState = new();

        // Flags if any search parameter has changed since last fetch.
        private bool _hasChanged = true;

        // The data should be considered out-of-date if the time span since the last fetch exceeds 30 minutes.
        private readonly TimeSpan _maxInterval = TimeSpan.FromMinutes(30);


        /// <summary>
        /// Returns true if any internal data has changed since the last API call.
        /// </summary>
        public bool HasChanged => _hasChanged || ((DateTime.UtcNow - _currentState.TimeStamp) > _maxInterval);

        /// <summary>
        /// Save the current state directly following a successful call to the API.
        /// This will ensure that the internal flag is reset and the method 'HasChanged'
        /// will return false until changes are made to the current search state.
        /// </summary>
        public void SaveState()
        {
            _currentState.TimeStamp = DateTime.UtcNow;

            _previousState = _currentState.Clone();
            _hasChanged = false;
        }


        /// <summary>
        /// Discards all changes to the current search state, reverting the service back to the last saved state.
        /// It is intended that this method be used by the <see cref="SortAndFilterViewModel"/>
        /// </summary>
        public void CancelChanges()
        {
            _currentState = _previousState.Clone();
            _hasChanged = false;
        }


        /// <summary>
        /// Discards all changes to the current search state, reverting the service back to it's default state.
        /// It is intended that this method be used by the <see cref="SortAndFilterViewModel"/>
        /// </summary>
        public void ResetState(CategoryDto? defaultCategory = null)
        {
            // Overwrite the current state with a default state.
            _currentState = new SearchState { TimeStamp = DateTime.Now };

            // By setting member variable 'Category', a default Subcategory will be chosen from the
            // Category's internal list.
            Category = defaultCategory;
        }


        //
        [ObservableProperty]
        private CategoryDto? category = null;
        partial void OnCategoryChanged(CategoryDto? value)
        {
            _currentState.Category = value;
            _hasChanged = true;

            // When category changes, clear Subcategory...
            if (value is null)
            {
                Subcategory = null;
            }

            // ...or assign a default value.
            else if (value.Subcategories.Count > 0 && Subcategory is null)
            {
                Subcategory = value.Subcategories.First();
            }
        }


        //
        [ObservableProperty]
        private SubcategoryDto? subcategory = null;
        partial void OnSubcategoryChanged(SubcategoryDto? value)
        {
            _currentState.Subcategory = value;
            _hasChanged = true;
        }


        //
        [ObservableProperty]
        private string? manufacturer = null;
        partial void OnManufacturerChanged(string? value)
        {
            _currentState.Manufacturer = value;
            _hasChanged = true;
        }


        //
        [ObservableProperty]
        private string? material = null;
        partial void OnMaterialChanged(string? value)
        {
            _currentState.Material = value;
            _hasChanged = true;
        }


        //
        [ObservableProperty]
        private string? keyword = null;
        partial void OnKeywordChanged(string? value)
        {
            _currentState.Keyword = value;
            _hasChanged = true;
        }


        //
        [ObservableProperty]
        private string? minimumPrice = null;
        partial void OnMinimumPriceChanged(string? value)
        {
            if (value is null)
            {
                _currentState.MinPrice = null;
                return;
            }

            // Extract only the digits from the string.
            var digits = new string(value.Where(char.IsDigit).ToArray());

            // Try to parse the digits to a decimal.
            if (decimal.TryParse(digits, out decimal price))
            {
                // If the parsed price is out of the valid range.
                if (price < 0 || price > (decimal)SearchQueryDto.PracticalMaximumPrice)
                {
                    _currentState.MinPrice = null;
                    return;
                }

                // If the parsed price is greater than the maximum price.
                // Set both minimum and maximum prices to the parsed price.
                else if (price > _currentState.MaxPrice)
                {
                    _currentState.MinPrice = price;
                    _currentState.MaxPrice = price;
                }

                // If the parsed price is valid and within the range.
                else
                {
                    _currentState.MinPrice = price;
                }

                _hasChanged = true;
            }
        }


        //
        [ObservableProperty]
        private string? maximumPrice = null;
        partial void OnMaximumPriceChanged(string? value)
        {
            if (value is null)
            {
                _currentState.MaxPrice = null;
                return;
            }

            // Extract only the digits from the string.
            var digits = new string(value.Where(char.IsDigit).ToArray());

            // Try to parse the digits to a decimal.
            if (decimal.TryParse(digits, out decimal price))
            {
                // If the parsed price is out of the valid range.
                if (price < 0 || price > (decimal)SearchQueryDto.PracticalMaximumPrice)
                {
                    _currentState.MaxPrice = null;
                    return;
                }

                // If the parsed price is less than the minimum price.
                // Set both minimum and maximum prices to the parsed price.
                else if (price < _currentState.MinPrice)
                {
                    _currentState.MinPrice = price;
                    _currentState.MaxPrice = price;
                }

                // If the parsed price is valid and within the range.
                else
                {
                    _currentState.MaxPrice = price;
                }

                _hasChanged = true;
            }
        }


        //
        public int PageNumber => _currentState.PageNumber;
        public void IncrementPageNumber() { _currentState.PageNumber++; _hasChanged = true; }
        public void DecrementPageNumber() { _currentState.PageNumber--; _hasChanged = true; }


        //
        public int PageSize
        {
            get => _currentState.PageSize;
            set { _currentState.PageSize = value; _hasChanged = true; }
        }


        //
        [ObservableProperty]
        private ItemSortOption sortBy = ItemSortOption.PriceAsc;
        [RelayCommand]
        private void SetSortBy(ItemSortOption option) => SortBy = option;
        partial void OnSortByChanged(ItemSortOption value) { _currentState.SortBy = value; _hasChanged = true; }


        /// <summary>
        /// Returns a deep copy of the current search state.
        /// </summary>
        //public SearchState CopyOfCurrentState => _currentState.Clone();


        /// <summary>
        /// Returns a deep copy of the previous search state.
        /// </summary>
        //public SearchState CopyOfPreviousState => _previousState.Clone();


        /// <summary>
        /// Builds a SearchQueryDto representation of the current search state.
        /// </summary>
        /// <returns></returns>
        public SearchQueryDto AsSearchQueryDto() => _currentState.AsSearchQueryDto();

        /// <summary>
        /// Builds the query section of a URL string which can be submitted to a REST API GET endpoint.
        /// </summary>
        /// <returns></returns>
        public string BuildUrlQueryString() => _currentState.AsSearchQueryDto().AsQueryString();
    }
}
