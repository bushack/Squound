using Microsoft.Extensions.Logging;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SquoundApp.ViewModels;

using Shared.DataTransfer;


namespace SquoundApp.Services
{
	public partial class SearchService(ILogger<SearchService> logger) : ObservableObject
	{
		private readonly ILogger<SearchService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));


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

			public int PageNumber { get; set; } = SearchQueryDto.DefaultPageNumber;
			public int PageSize { get; set; } = SearchQueryDto.DefaultPageSize;

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


		// Responsible for caching user-selected filter and sort parameters.
		// To be used when the user wishes to apply a filter or sort and perform a database query.
		private SearchState _currentState = new();

		// Responsible for caching user-selected filter and sort parameters from the last successful database query.
		// To be used when the user wishes to cancel changes made to the current state.
		private SearchState _previousState = new();

        // Flags if any parameter has changed since the last successful database query.
        private bool _hasChanged = true;

        // The data should be considered out-of-date if the interval since the last successful database query exceeds 30 minutes.
        private readonly TimeSpan _maxInterval = TimeSpan.FromMinutes(30);

        // Observable properties for user interface binding.
        [ObservableProperty] private CategoryDto? category = null;
		[ObservableProperty] private SubcategoryDto? subcategory = null;
		[ObservableProperty] private string? manufacturer = null;
		[ObservableProperty] private string? material = null;
		[ObservableProperty] private string? keyword = null;
		[ObservableProperty] private string? minimumPrice = null;
		[ObservableProperty] private string? maximumPrice = null;
		[ObservableProperty] private int pageNumber = SearchQueryDto.DefaultPageNumber;
		[ObservableProperty] private int pageSize = SearchQueryDto.DefaultPageSize;
		[ObservableProperty] private ItemSortOption sortBy = ItemSortOption.PriceAsc;


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
			_logger.LogDebug("Saving current state.");

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
			_logger.LogDebug("Cancelling changes to current state.");

			_currentState = _previousState.Clone();
			_hasChanged = false;

			SyncUserInterface();
		}


		/// <summary>
		/// Discards all changes to the current search state, reverting the service back to it's default state.
		/// It is intended that this method be used by the <see cref="SortAndFilterViewModel"/>
		/// </summary>
		public void ResetState(CategoryDto? defaultCategory = null)
		{
			_logger.LogDebug("Resetting current state.");

			// Overwrite the current state with a default state.
			_currentState = new SearchState { TimeStamp = DateTime.Now };

			// Assign default category and sync user interface.
			SetCategory(defaultCategory, true);
		}


		//
		public void SetCategory(CategoryDto? category, bool forceSubcategory)
        {
            _logger.LogDebug("Setting Category to {category}.", category?.Name);

            _currentState.Category = category;

			// If possible, forcibly assign the new category's first subcategory.
			if (forceSubcategory && _currentState.Category?.Subcategories.Count > 0)
            {
                _logger.LogDebug("Setting Subcategory to {subcategory}.", _currentState.Category.Subcategories.First().Name);

                _currentState.Subcategory = _currentState.Category.Subcategories.First();
			}

			_hasChanged = true;
			SyncUserInterface();
		}


		public void SetSubcategory(SubcategoryDto? subcategory)
        {
            _logger.LogDebug("Setting Subcategory to {subcategory}.", subcategory?.Name);

            _currentState.Subcategory = subcategory;
			_hasChanged = true;
			SyncUserInterface();

		}


		public void SetManufacturer(string? manufacturer)
        {
            _logger.LogDebug("Setting Manufacturer to {manufacturer}.", manufacturer);

            _currentState.Manufacturer = manufacturer;
			_hasChanged = true;
			SyncUserInterface();
		}


		public void SetMaterial(string? material)
        {
            _logger.LogDebug("Setting Material to {material}.", material);

            _currentState.Material = material;
			_hasChanged = true;
			SyncUserInterface();
		}


		public void SetKeyword(string? keyword)
        {
            _logger.LogDebug("Setting Keyword to {keyword}.", keyword);

            _currentState.Keyword = keyword;
			_hasChanged = true;
			SyncUserInterface();
		}


		public void SetMinPrice(string? minPrice)
        {
            _logger.LogDebug("Attempting to set MinPrice to {minPrice}.", minPrice);

            // Attempt to parse the string to a decimal.
            if (TryParsePrice(minPrice, out decimal? parsePrice) is false || parsePrice is null)
			{
				_currentState.MinPrice = null;
			}

			// If successfully parsed.
			else
			{
				// Clamp price to within practical range.
				decimal price = Math.Clamp(parsePrice.Value,
					(decimal)SearchQueryDto.PracticalMinimumPrice,
						(decimal)SearchQueryDto.PracticalMaximumPrice);

				// Ensure max price is greater-than or equal-to new min price.
				if (_currentState.MaxPrice < price)
				{
					_currentState.MinPrice = price;
					_currentState.MaxPrice = price;
				}

				// No conflict; simply assign new min price.
				else
				{
					_currentState.MinPrice = price;
				}
            }

            _logger.LogDebug("MinPrice set to {price}.", _currentState.MinPrice);

            _hasChanged = true;
			SyncUserInterface();
		}


		public void SetMaxPrice(string? maxPrice)
        {
            _logger.LogDebug("Attempting to set MinPrice to {maxPrice}.", maxPrice);

            // Attempt to parse the string to a decimal.
            if (TryParsePrice(maxPrice, out decimal? parsePrice) is false || parsePrice is null)
			{
				_currentState.MaxPrice = null;
			}

			// If successfully parsed.
			else
			{
				// Clamp price to within practical range.
				decimal price = Math.Clamp(parsePrice.Value,
					(decimal)SearchQueryDto.PracticalMinimumPrice,
						(decimal)SearchQueryDto.PracticalMaximumPrice);

				// Ensure min price is less-than or equal-to new max price.
				if (_currentState.MinPrice > price)
				{
					_currentState.MinPrice = price;
					_currentState.MaxPrice = price;
				}

				// No conflict; simply assign new max price.
				else
				{
					_currentState.MaxPrice = price;
				}
            }

            _logger.LogDebug("MaxPrice set to {price}.", _currentState.MaxPrice);

            _hasChanged = true;
			SyncUserInterface();
		}


		public void SetPageNumber(int pageNumber)
		{
			_currentState.PageNumber = pageNumber;
			_hasChanged = true;

            _logger.LogDebug("PageNumber set to {pageNumber}.", _currentState.PageNumber);

            SyncUserInterface();
		}


		public void IncrementPageNumber()
		{
			_currentState.PageNumber++;
			_hasChanged = true;

            _logger.LogDebug("Incremented PageNumber. PageNumber is {pageNumber}.", _currentState.PageNumber);

            SyncUserInterface();
		}


		public void DecrementPageNumber()
		{
			_currentState.PageNumber--;
			_hasChanged = true;

            _logger.LogDebug("Decremented PageNumber. PageNumber is {pageNumber}.", _currentState.PageNumber);

            SyncUserInterface();
		}


		public void SetPageSize(int pageSize)
		{
			_currentState.PageSize = pageSize;
			_hasChanged = true;

            _logger.LogDebug("PageSize set to {PageSize}.", _currentState.PageSize);

            SyncUserInterface();
		}


		[RelayCommand]
		public void SetSortBy(ItemSortOption sortBy)
		{
			_currentState.SortBy = sortBy;
			_hasChanged = true;

            _logger.LogDebug("SortBy set to {SortBy}.", _currentState.SortBy);

            SyncUserInterface();
		}


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


		/// <summary>
		/// Overwrite all observable properties which ensures UI bindings are updated.
		/// </summary>
		private void SyncUserInterface()
		{
			_logger.LogDebug("Synchronising user interface");

			this.Category       = _currentState.Category;
			this.Subcategory    = _currentState.Subcategory;
			this.Manufacturer   = _currentState.Manufacturer;
			this.Material       = _currentState.Material;
			this.Keyword        = _currentState.Keyword;
			this.MinimumPrice   = _currentState.MinPrice.ToString();
			this.MaximumPrice   = _currentState.MaxPrice.ToString();
			this.PageNumber     = _currentState.PageNumber;
			this.PageSize       = _currentState.PageSize;
			this.SortBy         = _currentState.SortBy;
		}


		//
		private bool TryParsePrice(string? input, out decimal? result)
        {
            _logger.LogDebug("Attempting to parse {input} to decimal", input);

            result = null;

			if (string.IsNullOrWhiteSpace(input))
				return false;

			var digits = string.Concat(input.Where(char.IsDigit));

			if (decimal.TryParse(digits, out decimal parsed))
            {
                _logger.LogDebug("Parsing successful. Value is {parsed}", parsed);

                result = parsed;
				return true;
			}

            _logger.LogDebug("Parsing unsuccessful.");

            return false;
		}
	}
}