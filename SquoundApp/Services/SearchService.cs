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


		private SearchState _currentState = new();
		private SearchState _previousState = new();

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

			SyncUserInterface();
		}


		/// <summary>
		/// Discards all changes to the current search state, reverting the service back to it's default state.
		/// It is intended that this method be used by the <see cref="SortAndFilterViewModel"/>
		/// </summary>
		public void ResetState(CategoryDto? defaultCategory = null)
		{
			// Overwrite the current state with a default state.
			_currentState = new SearchState { TimeStamp = DateTime.Now };

			// Assign default category and sync user interface.
			SetCategory(defaultCategory, true);
		}


		//
		public void SetCategory(CategoryDto? category, bool forceSubcategory)
		{
			_currentState.Category = category;

			// If possible, forcibly assign the new category's first subcategory.
			if (forceSubcategory && _currentState.Category?.Subcategories.Count > 0)
			{
				_currentState.Subcategory = _currentState.Category.Subcategories.First();
			}

			_hasChanged = true;
			SyncUserInterface();
		}


		public void SetSubcategory(SubcategoryDto? subcategory)
		{
			_currentState.Subcategory = subcategory;
            _hasChanged = true;
            SyncUserInterface();
		}


		public void SetManufacturer(string? manufacturer)
		{
			_currentState.Manufacturer = manufacturer;
            _hasChanged = true;
            SyncUserInterface();
		}


		public void SetMaterial(string? material)
		{
			_currentState.Material = material;
            _hasChanged = true;
            SyncUserInterface();
		}


		public void SetKeyword(string? keyword)
		{
			_currentState.Keyword = keyword;
            _hasChanged = true;
            SyncUserInterface();
		}


		public void SetMinPrice(string? minPrice)
        {
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

            _hasChanged = true;
            SyncUserInterface();
        }


		public void SetMaxPrice(string? maxPrice)
        {
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
			
			_hasChanged = true;
            SyncUserInterface();
		}


		public void SetPageNumber(int pageNumber)
		{
			_currentState.PageNumber = pageNumber;
			_hasChanged = true;
			SyncUserInterface();
		}


		public void IncrementPageNumber()
		{
			_currentState.PageNumber++;
			_hasChanged = true;
            SyncUserInterface();
        }


		public void DecrementPageNumber()
		{
			_currentState.PageNumber--;
			_hasChanged = true;
            SyncUserInterface();
        }


		public void SetPageSize(int pageSize)
		{
			_currentState.PageSize = pageSize;
			_hasChanged = true;
			SyncUserInterface();
        }


        [RelayCommand]
        public void SetSortBy(ItemSortOption sortBy)
        {
            _currentState.SortBy = sortBy;
            _hasChanged = true;
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
		private static bool TryParsePrice(string? input, out decimal? result)
		{
			result = null;

			if (string.IsNullOrWhiteSpace(input))
				return false;

			var digits = string.Concat(input.Where(char.IsDigit));

			if (decimal.TryParse(digits, out decimal parsed))
			{
				result = parsed;
				return true;
			}

			return false;
		}
	}
}