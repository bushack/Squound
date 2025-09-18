using Microsoft.Extensions.Logging;

using SquoundApp.Events;
using SquoundApp.Interfaces;

using Shared.DataTransfer;
using Shared.Interfaces;


namespace SquoundApp.Contexts
{
	public partial class SearchContext : ISearchContext
    {
		private readonly ILogger<SearchContext> _Logger;
		private readonly IEventService _Events;


        /// <summary>
		/// Internal class responsible for caching current item detail request.
		/// To submit a search query to the API, first request an ItemDetailQueryDto using the
		/// method 'ToItemDetailQueryDto'. This object can then be used to produce the query
		/// string section of a URL that can be submitted to a REST API GET endpoint with
		/// the method 'AsQueryString'.
		/// </summary>
        private class RequestState
        {
            public DateTime TimeStamp;
            public long? ItemId { get; set; } = null;


            public ItemDetailQueryDto AsItemDetailQueryDto(int imageWidth, int imageHeight)
            {
                return new ItemDetailQueryDto
                {
                    ItemId          = ItemId ?? Defaults.MinimumItemId,
                    ImageWidth      = imageWidth,
                    ImageHeight     = imageHeight
                };
            }
        }


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

			public int PageNumber { get; set; } = Shared.DataTransfer.Defaults.PageNumber;
			public int PageSize { get; set; } = Shared.DataTransfer.Defaults.PageSize;

			public ItemSortOption SortBy { get; set; } = ItemSortOption.PriceAsc;


			public SearchState Clone()
			{
				return new SearchState
				{
					TimeStamp       = TimeStamp,
					Category        = Category,
					Subcategory     = Subcategory,
					Manufacturer    = Manufacturer,
					Material        = Material,
					Keyword         = Keyword,
					MinPrice        = MinPrice,
					MaxPrice        = MaxPrice,
					PageNumber      = PageNumber,
					PageSize        = PageSize,
					SortBy          = SortBy
				};
			}


			public SearchQueryDto AsSearchQueryDto(int imageWidth, int imageHeight)
			{
				return new SearchQueryDto
				{
					Category        = Category?.Name ?? null,
					Subcategory     = Subcategory?.Name ?? null,
					Manufacturer    = Manufacturer ?? null,
					Material        = Material ?? null,
					Keyword         = Keyword ?? null,
					MinPrice        = MinPrice ?? null,
					MaxPrice        = MaxPrice ?? null,
					PageNumber      = PageNumber,
					PageSize        = PageSize,
                    ImageWidth      = imageWidth,
                    ImageHeight     = imageHeight,
                    SortBy          = SortBy
				};
			}
		}


        // Responsible for caching user-selected item id parameter.
        // To be used when the user wishes to view a specific item in detail.
        private RequestState _CurrentRequest = new();

		// Responsible for caching user-selected filter and sort parameters.
		// To be used when the user wishes to apply a filter or sort and perform a database query.
		private SearchState _CurrentState = new();

		// Responsible for caching user-selected filter and sort parameters from the last successful database query.
		// To be used when the user wishes to cancel changes made to the current state.
		private SearchState _PreviousState = new();

		// The data should be considered out-of-date if the interval since the last successful database query exceeds 30 minutes.
		private readonly TimeSpan _MaxInterval = TimeSpan.FromMinutes(30);

        // Responsible for caching the required image dimensions for item thumbnails.
        private int _RequiredImageWidth = Shared.DataTransfer.Defaults.ImageWidth;
        private int _RequiredImageHeight = Shared.DataTransfer.Defaults.ImageHeight;



        /// <summary>
        /// Returns true if the internal state has changed since it was last saved.
        /// </summary>
        public bool HasChanged => GetChangedFields().Any() || DateTime.UtcNow - _CurrentState.TimeStamp > _MaxInterval;


        /// <summary>
        /// Returns true if the internal state has not changed since it was last saved.
        /// </summary>
        public bool HasNotChanged => !HasChanged;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="events"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SearchContext(ILogger<SearchContext> logger, IEventService events)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_Events = events ?? throw new ArgumentNullException(nameof(events));
		}


        /// <summary>
        /// Saves all changes to the internal state. To be called following a successful response from the API.
        /// </summary>
        public void SaveChanges()
		{
            _CurrentState.TimeStamp = DateTime.UtcNow;

            _PreviousState = _CurrentState.Clone();
            _Logger.LogDebug("Saved changes. Saved: {changes}", string.Join(", ", GetChangedFields()));
            _Events.Publish(new SearchContextChangedEvent(this));
        }


        /// <summary>
        /// Discards all changes to the internal state, reverting the service back to the last saved state.
        /// </summary>
        public void CancelChanges()
		{
            _CurrentState = _PreviousState.Clone();
            _Logger.LogDebug("Discarded changes. Discarded: {changes}", string.Join(", ", GetChangedFields()));
            _Events.Publish(new SearchContextChangedEvent(this));
		}


        /// <summary>
        /// Performs a total reset of the internal state, including the sorting method.
        /// All changes will be discarded and the context will revert to it's default state.
        /// </summary>
        public void HardReset(CategoryDto? defaultCategory = null)
		{
            // Overwrite the current state with a default state.
            _CurrentState = new SearchState { TimeStamp = DateTime.UtcNow };

            Category = defaultCategory;
            _Logger.LogDebug("Reset changes. Reset: {changes}", string.Join(", ", GetChangedFields()));
            _Events.Publish(new SearchContextChangedEvent(this));
        }


        /// <summary>
        /// Performs a limited reset of the internal state, excluding the sorting method.
        /// To perform a total reset, use method 'HardReset'.
        /// </summary>
        public void SoftReset(CategoryDto? defaultCategory = null)
        {
            _CurrentState = new SearchState
            {
                SortBy = this.SortBy,
                TimeStamp = DateTime.UtcNow
            };

            Category = defaultCategory;
            _Logger.LogDebug("Reset changes. Reset: {changes}", string.Join(", ", GetChangedFields()));
            _Events.Publish(new SearchContextChangedEvent(this));
        }


        public long? ItemId
        {
            get => _CurrentRequest.ItemId;
            set
            {
                _CurrentRequest.ItemId = value;
                _Logger.LogDebug("Set ItemId to {id}.", _CurrentRequest.ItemId);
                _Events.Publish(new SearchContextChangedEvent(this));
            }
        }


		public CategoryDto? Category
		{
			get => _CurrentState.Category;
			set
            {
                _CurrentState.Category = value;
                _CurrentState.Subcategory = null;
                _Logger.LogDebug("Set Category to {category}.", _CurrentState.Category);
                _Logger.LogDebug("Set Subcategory to {subcategory}.", _CurrentState.Subcategory);
                _Events.Publish(new SearchContextChangedEvent(this));
            }
        }


        public SubcategoryDto? Subcategory
		{
			get => _CurrentState.Subcategory;
			set
            {
                // TODO : Ensure the subcategory belongs to the currently selected category or is null.

                _CurrentState.Subcategory = value;
                _Logger.LogDebug("Set Subcategory to {subcategory}.", _CurrentState.Subcategory);
                _Events.Publish(new SearchContextChangedEvent(this));
            }
        }


        public string? Manufacturer
		{
			get => _CurrentState.Manufacturer;
			set
			{
				_CurrentState.Manufacturer = value;
				_Logger.LogDebug("Set Manufacturer to {manufacturer}.", _CurrentState.Manufacturer);
				_Events.Publish(new SearchContextChangedEvent(this));
            }
		}


        public string? Material
		{
			get => _CurrentState.Material;
            set
			{
				_CurrentState.Material = value;
				_Logger.LogDebug("Set Material to {material}.", _CurrentState.Material);
				_Events.Publish(new SearchContextChangedEvent(this));
            }
		}


        public string? Keyword
		{
			get => _CurrentState.Keyword;
            set
            {
				_CurrentState.Keyword = value;
				_Logger.LogDebug("Set Keyword to {keyword}.", _CurrentState.Keyword);
				_Events.Publish(new SearchContextChangedEvent(this));
            }
		}


        public string? MinPrice
		{
			get => _CurrentState.MinPrice?.ToString() ?? string.Empty;
			set
            {
                // Attempt to parse the string to a decimal.
                if (TryParsePrice(value, out decimal? parsePrice) is false || parsePrice is null)
                {
                    _CurrentState.MinPrice = null;
                }

                // If successfully parsed.
                else
                {
                    // Clamp price to within practical range.
                    decimal price = Math.Clamp(parsePrice.Value,
                        (decimal)Shared.DataTransfer.Defaults.PracticalMinimumPrice,
                            (decimal)Shared.DataTransfer.Defaults.PracticalMaximumPrice);

                    // Ensure max price is greater-than or equal-to new min price.
                    if (_CurrentState.MaxPrice < price)
                    {
                        _CurrentState.MinPrice = price;
                        _CurrentState.MaxPrice = price;
                    }

                    // No conflict; simply assign new min price.
                    else
                    {
                        _CurrentState.MinPrice = price;
                    }
                }

                _Logger.LogDebug("Set MinPrice to {price}.", _CurrentState.MinPrice);
                _Events.Publish(new SearchContextChangedEvent(this));
            }
		}


        public string? MaxPrice
		{
			get => _CurrentState.MaxPrice?.ToString() ?? string.Empty;
			set
            {
                // Attempt to parse the string to a decimal.
                if (TryParsePrice(value, out decimal? parsePrice) is false || parsePrice is null)
                {
                    _CurrentState.MaxPrice = null;
                }

                // If successfully parsed.
                else
                {
                    // Clamp price to within practical range.
                    decimal price = Math.Clamp(parsePrice.Value,
                        (decimal)Shared.DataTransfer.Defaults.PracticalMinimumPrice,
                            (decimal)Shared.DataTransfer.Defaults.PracticalMaximumPrice);

                    // Ensure min price is less-than or equal-to new max price.
                    if (_CurrentState.MinPrice > price)
                    {
                        _CurrentState.MinPrice = price;
                        _CurrentState.MaxPrice = price;
                    }

                    // No conflict; simply assign new max price.
                    else
                    {
                        _CurrentState.MaxPrice = price;
                    }
                }

                _Logger.LogDebug("Set MaxPrice to {price}.", _CurrentState.MaxPrice);
                _Events.Publish(new SearchContextChangedEvent(this));
            }
		}


        public int PageNumber
		{
            get => _CurrentState.PageNumber;
            set
            {
                _CurrentState.PageNumber = value;
                _Logger.LogDebug("Set PageNumber to {pageNumber}.", _CurrentState.PageNumber);
                _Events.Publish(new SearchContextChangedEvent(this));
            }
		}


		public void IncrementPageNumber()
		{
            _CurrentState.PageNumber++;
            _Logger.LogDebug("Incremented PageNumber. PageNumber is {pageNumber}.", _CurrentState.PageNumber);
            _Events.Publish(new SearchContextChangedEvent(this));
		}


		public void DecrementPageNumber()
		{
            _CurrentState.PageNumber--;
            _Logger.LogDebug("Decremented PageNumber. PageNumber is {pageNumber}.", _CurrentState.PageNumber);
            _Events.Publish(new SearchContextChangedEvent(this));
		}


        public int PageSize
		{
            get => _CurrentState.PageSize;
            set
            {
                _CurrentState.PageSize = value;
                _Logger.LogDebug("Set PageSize to {PageSize}.", _CurrentState.PageSize);
                _Events.Publish(new SearchContextChangedEvent(this));
            }
        }


        public int RequiredImageWidth
        {
            get => _RequiredImageWidth;
            set
            {
                _RequiredImageWidth = value;
                _Logger.LogDebug("Set RequiredImageWidth to {Width}.", _RequiredImageWidth);
            }
        }


        public int RequiredImageHeight
        {
            get => _RequiredImageHeight;
            set
            {
                _RequiredImageHeight = value;
                _Logger.LogDebug("Set RequiredImageHeight to {Height}.", _RequiredImageHeight);
            }
        }


        public ItemSortOption SortBy
		{
            get => _CurrentState.SortBy;
            set
            {
                _CurrentState.SortBy = value;
                _Logger.LogDebug("Set SortBy to {SortBy}.", _CurrentState.SortBy);
                _Events.Publish(new SearchContextChangedEvent(this));
            }
		}


        /// <summary>
        /// Builds a SearchQueryDto representation of the internal state.
        /// </summary>
        /// <returns></returns>
        public SearchQueryDto AsSearchQueryDto() =>
            _CurrentState.AsSearchQueryDto(_RequiredImageWidth, _RequiredImageHeight);


        /// <summary>
        /// Builds an ItemDetailQueryDto representation of the internal state.
        /// </summary>
        /// <returns></returns>
        public ItemDetailQueryDto AsItemDetailQueryDto() =>
            _CurrentRequest.AsItemDetailQueryDto(_RequiredImageWidth, _RequiredImageHeight);


        /// <summary>
        /// Builds the query section of a URL string which can be submitted to a REST API GET endpoint.
        /// </summary>
        /// <returns></returns>
        public string BuildItemSummaryUrlQueryString() =>
            _CurrentState.AsSearchQueryDto(_RequiredImageWidth, _RequiredImageHeight).AsQueryString();


        /// <summary>
        /// Builds the query section of a URL string which can be submitted to a REST API endpoint.
        /// </summary>
        /// <returns></returns>
        public string BuildItemDetailUrlQueryString() =>
            _CurrentRequest.AsItemDetailQueryDto(_RequiredImageWidth, _RequiredImageHeight).AsQueryString();


        private IEnumerable<string> GetChangedFields()
        {
            if (_CurrentState.Category?.Name != _PreviousState.Category?.Name)
                yield return nameof(_CurrentState.Category);

            if (_CurrentState.Subcategory?.Name != _PreviousState.Subcategory?.Name)
                yield return nameof(_CurrentState.Subcategory);

            if (_CurrentState.Manufacturer != _PreviousState.Manufacturer)
                yield return nameof(_CurrentState.Manufacturer);

            if (_CurrentState.Material != _PreviousState.Material)
                yield return nameof(_CurrentState.Material);

            if (_CurrentState.Keyword != _PreviousState.Keyword)
                yield return nameof(_CurrentState.Keyword);

            if (_CurrentState.MinPrice != _PreviousState.MinPrice)
                yield return nameof(_CurrentState.MinPrice);

            if (_CurrentState.MaxPrice != _PreviousState.MaxPrice)
                yield return nameof(_CurrentState.MaxPrice);

            if (_CurrentState.PageNumber != _PreviousState.PageNumber)
                yield return nameof(_CurrentState.PageNumber);

            if (_CurrentState.PageSize != _PreviousState.PageSize)
                yield return nameof(_CurrentState.PageSize);

            if (_CurrentState.SortBy != _PreviousState.SortBy)
                yield return nameof(_CurrentState.SortBy);
        }


		private bool TryParsePrice(string? input, out decimal? result)
		{
            _Logger.LogDebug("Attempting to parse {input} to decimal", input);

			result = null;

			if (string.IsNullOrWhiteSpace(input))
				return false;

			var digits = string.Concat(input.Where(char.IsDigit));

			if (decimal.TryParse(digits, out decimal parsed))
			{
                _Logger.LogDebug("Parsing successful. Value is {parsed}", parsed);

				result = parsed;
				return true;
			}

            _Logger.LogDebug("Parsing unsuccessful.");

			return false;
		}
	}
}