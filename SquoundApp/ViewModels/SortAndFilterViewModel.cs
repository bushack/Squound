using Microsoft.Extensions.Logging;

using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SquoundApp.Events;
using SquoundApp.Interfaces;
using SquoundApp.Pages;
using SquoundApp.Services;

using Shared.DataTransfer;
using Shared.Interfaces;


namespace SquoundApp.ViewModels
{
	public partial class SortAndFilterViewModel : BaseViewModel
	{
		// Services & Contexts.
		private readonly ILogger<SortAndFilterViewModel> _Logger;
		private readonly IEventService _Events;
		private readonly ICategoryRepository _Categories;
		private readonly ISearchContext _Search;

		// Search state binding properties.
		[ObservableProperty] private ObservableCollection<CategoryDto> categoryList = [];
		[ObservableProperty] private ObservableCollection<SubcategoryDto> subcategoryList = [];
		
		[ObservableProperty] private CategoryDto? category = null;
		[ObservableProperty] private SubcategoryDto? subcategory = null;

		[ObservableProperty] private string? manufacturer = null;
		[ObservableProperty] private string? material = null;
		[ObservableProperty] private string? keyword = null;
		[ObservableProperty] private string? minPrice = null;
		[ObservableProperty] private string? maxPrice = null;
		
		[ObservableProperty] private int pageNumber = SearchQueryDto.DefaultPageNumber;
		[ObservableProperty] private int pageSize = SearchQueryDto.DefaultPageSize;
		
		[ObservableProperty] private ItemSortOption sortBy = ItemSortOption.PriceAsc;

        // User Interface state binding properties.
        [ObservableProperty] private bool sortByNameAscending = false;
        [ObservableProperty] private bool sortByNameDescending = false;
        [ObservableProperty] private bool sortByPriceAscending = true;
        [ObservableProperty] private bool sortByPriceDescending = false;

        [ObservableProperty] private bool isTitleLabelVisible = true;
		[ObservableProperty] private bool isSortButtonActive = true;
		[ObservableProperty] private bool isFilterButtonActive = true;
		[ObservableProperty] private bool isSortMenuActive = false;
		[ObservableProperty] private bool isFilterMenuActive = false;

        // To guard against recursive updates when synchronising the user interface.
        private bool _IsSyncing = false;


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="events"></param>
        /// <param name="categories"></param>
        /// <param name="search"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public SortAndFilterViewModel(ILogger<SortAndFilterViewModel> logger, IEventService events,
            ICategoryRepository categories, ISearchContext search)
		{
			_Logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_Events = events ?? throw new ArgumentNullException(nameof(events));
            _Categories = categories ?? throw new ArgumentNullException(nameof(categories));
			_Search = search ?? throw new ArgumentNullException(nameof(search));

			_Events.Subscribe<SearchContextChangedEvent>(OnSearchContextChanged);
		}


		partial void OnCategoryListChanged(ObservableCollection<CategoryDto> value)
		{
		}


		partial void OnSubcategoryListChanged(ObservableCollection<SubcategoryDto> value)
		{
		}


		partial void OnCategoryChanged(CategoryDto? value)
        {
            if (_IsSyncing)
                return;

            _Search.Category = value;

			// Sync the subcategory list with the selected category.
			SubcategoryList.Clear();
			if (value is not null)
			{
				foreach (var subcategory in value.Subcategories)
				{
					SubcategoryList.Add(subcategory);
				}

				// Preselect the first subcategory as default.
				if (SubcategoryList.Count > 0)
				{
					Subcategory = SubcategoryList.FirstOrDefault();
				}
			}
		}


		partial void OnSubcategoryChanged(SubcategoryDto? value)
        {
            if (_IsSyncing)
                return;

            _Search.Subcategory = value;
		}


        partial void OnManufacturerChanged(string? value)
        {
            if (_IsSyncing)
                return;

            _Search.Manufacturer = value;
		}


        partial void OnMaterialChanged(string? value)
        {
            if (_IsSyncing)
                return;

            _Search.Material = value;
        }


        partial void OnKeywordChanged(string? value)
        {
            if (_IsSyncing)
                return;

            _Search.Keyword = value;
        }


        partial void OnMinPriceChanged(string? value)
        {
            if (_IsSyncing)
                return;

            _Search.MinPrice = value;
        }


        partial void OnMaxPriceChanged(string? value)
        {
            if (_IsSyncing)
                return;

            _Search.MaxPrice = value;
        }


        partial void OnPageNumberChanged(int value)
        {
            if (_IsSyncing)
                return;

            _Search.PageNumber = value;
        }


        partial void OnPageSizeChanged(int value)
		{
			if (_IsSyncing)
				return;

            _Search.PageSize = value;
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


		//
		public async Task InitAsync()
		{
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

				CategoryList = [.. await _Categories.GetCategoriesAsync()];

				// Retrieve item categories from the category service.
				// This method is expected to return a list of item categories asynchronously.
				// The retrieved categories will be added to the categoryList collection.
				//var response = await _Categories.GetDataAsync();

				//if (response.Success is false)
				//{
				//	await Shell.Current.DisplayAlert("Error", response.ErrorMessage, "OK");
				//	return;
				//}

				//// Null or empty item list from API.
				//if (response.Data is null || response.Data.Count == 0)
				//{
				//	await Shell.Current.DisplayAlert("Sorry", "No items matched the search criteria", "OK");
				//	return;
				//}

				//// By assigning the fetched categories to the CategoryList, a notification is triggered
				//// that the collection has changed and the OnCategoryListChanged method is called.
				//CategoryList = [.. response.Data];
			}

			catch (Exception)
			{
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


		/// <summary>
		/// Handles the apply sort and apply filter button click events.
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		private async Task ApplyQueryAsync()
		{
			HideMenus();

			await ServiceLocator.GetService<NavigationService>().GoToAsync(nameof(RefinedSearchPage));
		}


		/// <summary>
		/// Handles the cancel sort and cancel filter button click events.
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		private void OnCancelButton()
		{
			_Search.CancelChanges();

			HideMenus();
		}


		/// <summary>
		/// Handles the reset button click event.
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		private void OnResetButton()
		{
			// Reset the search but also assign the first category in the list as default.
			// This will also assign a default subcategory.
			// We want to avoid a situation where no category is selected as that would
			// result in all items being returned from the API.
			_Search.ResetChanges(CategoryList.FirstOrDefault());
		}


		/// <summary>
		/// Handles sorting option changes.
		/// </summary>
		/// <param name="sortBy"></param>
		[RelayCommand]
		public void OnSortByButton(ItemSortOption sortBy)
		{
			_Search.SortBy = sortBy;
		}


        //
        private void OnSearchContextChanged(SearchContextChangedEvent e)
        {
            _Logger.LogDebug("Synchronising user interface");

			try
			{
                // Avoid recursive updates.
				_IsSyncing = true;

                // Synchronise the user interface with the search context.
                if (this.Category != e.Context.Category)
					this.Category = e.Context.Category;

				if (this.Subcategory != e.Context.Subcategory)
					this.Subcategory = e.Context.Subcategory;

				if (this.Manufacturer != e.Context.Manufacturer)
					this.Manufacturer = e.Context.Manufacturer;

				if (this.Material != e.Context.Material)
					this.Material = e.Context.Material;

				if (this.Keyword != e.Context.Keyword)
					this.Keyword = e.Context.Keyword;

				var minPriceString = e.Context.MinPrice?.ToString() ?? string.Empty;
				if (this.MinPrice != minPriceString)
					this.MinPrice = minPriceString;

				var maxPriceString = e.Context.MaxPrice?.ToString() ?? string.Empty;
				if (this.MaxPrice != maxPriceString)
					this.MaxPrice = maxPriceString;

				if (this.PageNumber != e.Context.PageNumber)
					this.PageNumber = e.Context.PageNumber;

				if (this.PageSize != e.Context.PageSize)
					this.PageSize = e.Context.PageSize;

				if (this.SortBy != e.Context.SortBy)
					this.SortBy = e.Context.SortBy;

				if (this.SortByNameAscending != (e.Context.SortBy == ItemSortOption.NameAsc))
					this.SortByNameAscending = (e.Context.SortBy == ItemSortOption.NameAsc);

				if (this.SortByNameDescending != (e.Context.SortBy == ItemSortOption.NameDesc))
					this.SortByNameDescending = (e.Context.SortBy == ItemSortOption.NameDesc);

				if (this.SortByPriceAscending != (e.Context.SortBy == ItemSortOption.PriceAsc))
					this.SortByPriceAscending = (e.Context.SortBy == ItemSortOption.PriceAsc);

				if (this.SortByPriceDescending != (e.Context.SortBy == ItemSortOption.PriceDesc))
					this.SortByPriceDescending = (e.Context.SortBy == ItemSortOption.PriceDesc);
			}

			finally
			{
				_IsSyncing = false;
			}
        }
    }
}