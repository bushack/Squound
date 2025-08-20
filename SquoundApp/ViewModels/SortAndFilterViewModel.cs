using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using System.Collections.ObjectModel;

using SquoundApp.Pages;
using SquoundApp.Services;

using Shared.DataTransfer;


namespace SquoundApp.ViewModels
{
	public partial class SortAndFilterViewModel : BaseViewModel
	{
		private readonly CategoryService _categoryService;

		[ObservableProperty]
		private SearchService searchService;

		[ObservableProperty]
		private ObservableCollection<CategoryDto> categoryList = [];

		[ObservableProperty]
		private ObservableCollection<SubcategoryDto> subcategoryList = [];


		//
		public SortAndFilterViewModel(CategoryService categoryService, SearchService searchService)
		{
			this._categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
			this.SearchService = searchService ?? throw new ArgumentNullException(nameof(searchService));
		}

		//[ObservableProperty]
		//private CategoryDto? selectedCategory = null;

		//[ObservableProperty]
		//private SubcategoryDto? selectedSubcategory = null;


		// Variables responsible for adjusting the sort options.
		//[ObservableProperty]
		//private bool sortByNameAscending = false;   <<< continue migration over to SearchState

		//[ObservableProperty]
		//private bool sortByNameDescending = false;

		//[ObservableProperty]
		//private bool sortByPriceAscending = true;

		//[ObservableProperty]
		//private bool sortByPriceDescending = false;


		// Variables responsible for adjusting the filter options.
		//[ObservableProperty]
		//private string category = string.Empty;

		//[ObservableProperty]
		//private string subcategory = string.Empty;

		//[ObservableProperty]
		//private string manufacturer = string.Empty;

		//[ObservableProperty]
		//private string material = string.Empty;

		//[ObservableProperty]
		//private string keyword = string.Empty;

		//[ObservableProperty]
		//private string minimumPrice = string.Empty;

		//[ObservableProperty]
		//private string maximumPrice = string.Empty;


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
		/// Populates the DynamicList with categories when the CategoryList changes.
		/// This typically happens only once when the CoarseSearchPage's OnAppearing method
		/// is called for the first time.
		/// </summary>
		/// <param name="value"></param>
		partial void OnCategoryListChanged(ObservableCollection<CategoryDto> value)
		{
			if (SearchService.Category is null)
			{
				//SearchService.SetCategory(value.FirstOrDefault(), true);
			}

			// If the user has selected a category on a previous
			// screen we need to preselect that same category here.
			//if (SearchState.Category is not null)
			//{
			//    SelectedCategory = CategoryList.FirstOrDefault(c => c.Name == SearchState.Category);

			//    //this whole selecting of category/subcategory needs to be recoded
			//}

			// TODO : What if null?
		}

		//
		//partial void OnSelectedCategoryChanged(CategoryDto? value)
		//{
		//    if (CategoryList.Count == 0)
		//        return;

		//    // If the value is null (likely after a reset) we need to assign a default value.
		//    if (value is null)
		//    {
		//        value = CategoryList.FirstOrDefault();
		//    }

		//    // Writes the category to the search state.
		//    Category = value.Name;

		//    // Clear any previously selected subcategory.
		//    SelectedSubcategory = null;

		//    // Clear the subcategories list.
		//    SubcategoryList.Clear();

		//    // Then repopulate it with subcategories of the selected category.
		//    foreach (var subcategory in value.Subcategories)
		//    {
		//        SubcategoryList.Add(subcategory);
		//    }

		//    // Preselect the first subcategory as default.
		//    if (SubcategoryList.Count > 0)
		//    {
		//        SelectedSubcategory = SubcategoryList.FirstOrDefault();
		//    }
		//}

		//
		partial void OnSubcategoryListChanged(ObservableCollection<SubcategoryDto> value)
		{
		}

		//
		//partial void OnSelectedSubcategoryChanged(SubcategoryDto? value)
		//{
		//    // Write the subcategory to the search state.
		//    Subcategory = value != null ? value.Name : string.Empty;
		//}


		// Partial methods to handle changes in filter properties.
		//partial void OnCategoryChanged(string value)
		//{
		//    SearchState.Category = value;
		//}
		//partial void OnSubcategoryChanged(string value)
		//{
		//    SearchState.Subcategory = value;
		//}
		//partial void OnManufacturerChanged(string value)
		//{
		//    SearchState.Manufacturer = value;
		//}
		//partial void OnMaterialChanged(string value)
		//{
		//    SearchState.Material = value;
		//}
		//partial void OnKeywordChanged(string value)
		//{
		//    SearchState.Keyword = value;
		//}
		//partial void OnMinimumPriceChanged(string value)
		//{
		//    // Extract only the digits from the string.
		//    var digits = new string(value.Where(char.IsDigit).ToArray());

		//    // Try to parse the digits to a decimal.
		//    if (decimal.TryParse(digits, out decimal price))
		//    {
		//        // If the parsed price is out of the valid range.
		//        if (price < 0 || price > (decimal)SearchQueryDto.PracticalMaximumPrice)
		//        {
		//            SearchState.MinPrice = null;
		//        }

		//        // If the parsed price is greater than the maximum price.
		//        // Set both minimum and maximum prices to the parsed price.
		//        else if (price > SearchState.MaxPrice)
		//        {
		//            SearchState.MinPrice = price;
		//            SearchState.MaxPrice = price;

		//            RestoreQueryToUserInterface(SearchState.CopyOfCurrentQuery);
		//        }

		//        // If the parsed price is valid and within the range.
		//        else
		//        {
		//            SearchState.MinPrice = price;
		//        }
		//    }

		//    else
		//    {
		//        // If parsing fails.
		//        SearchState.MinPrice = null;
		//    }
		//}
		//partial void OnMaximumPriceChanged(string value)
		//{
		//    // Extract only the digits from the string.
		//    var digits = new string(value.Where(char.IsDigit).ToArray());

		//    // Try to parse the digits to a decimal.
		//    if (decimal.TryParse(digits, out decimal price))
		//    {
		//        // If the parsed price is out of the valid range.
		//        if (price < 0 || price > (decimal)SearchQueryDto.PracticalMaximumPrice)
		//        {
		//            SearchState.MaxPrice = null;
		//        }

		//        // If the parsed price is less than the minimum price.
		//        // Set both minimum and maximum prices to the parsed price.
		//        else if (price < SearchState.MinPrice)
		//        {
		//            SearchState.MinPrice = price;
		//            SearchState.MaxPrice = price;

		//            RestoreQueryToUserInterface(SearchState.CopyOfCurrentQuery);
		//        }

		//        // If the parsed price is valid and within the range.
		//        else
		//        {
		//            SearchState.MaxPrice = price;
		//        }
		//    }

		//    else
		//    {
		//        // If parsing fails.
		//        SearchState.MaxPrice = null;
		//    }
		//}
		//partial void OnSortByNameAscendingChanged(bool value)
		//{
		//    SearchState.SortBy = value ? ItemSortOption.NameAsc : SearchState.SortBy;
		//}
		//partial void OnSortByNameDescendingChanged(bool value)
		//{
		//    SearchState.SortBy = value ? ItemSortOption.NameDesc : SearchState.SortBy;
		//}
		//partial void OnSortByPriceAscendingChanged(bool value)
		//{
		//    SearchState.SortBy = value ? ItemSortOption.PriceAsc : SearchState.SortBy;
		//}
		//partial void OnSortByPriceDescendingChanged(bool value)
		//{
		//    SearchState.SortBy = value ? ItemSortOption.PriceDesc : SearchState.SortBy;
		//}


		//// Methods responsible for setting the sort options.
		//[RelayCommand]
		//private void SetSortOption(ItemSortOption sortBy)
		//{
		//    // Reset all sort options to false.
		//    SortByNameAscending = false;
		//    SortByNameDescending = false;
		//    SortByPriceAscending = false;
		//    SortByPriceDescending = false;

		//    // Set the appropriate sort option based on the provided sortBy parameter.
		//    switch (sortBy)
		//    {
		//        case ItemSortOption.NameAsc:
		//            SortByNameAscending = true;
		//            break;

		//        case ItemSortOption.NameDesc:
		//            SortByNameDescending = true;
		//            break;

		//        case ItemSortOption.PriceAsc:
		//            SortByPriceAscending = true;
		//            break;

		//        case ItemSortOption.PriceDesc:
		//            SortByPriceDescending = true;
		//            break;

		//        // If no valid sort option is provided, default to sorting by price ascending.
		//        default:
		//            SortByPriceAscending = true;
		//            break;
		//    }
		//}

		//[RelayCommand]
		//private void SetSortOptionAsNameAscending()
		//{
		//    SortByNameAscending = true;
		//    SortByNameDescending = false;
		//    SortByPriceAscending = false;
		//    SortByPriceDescending = false;
		//}

		//[RelayCommand]
		//private void SetSortOptionAsNameDescending()
		//{
		//    SortByNameAscending = false;
		//    SortByNameDescending = true;
		//    SortByPriceAscending = false;
		//    SortByPriceDescending = false;
		//}

		//[RelayCommand]
		//private void SetSortOptionAsPriceAscending()
		//{
		//    SortByNameAscending = false;
		//    SortByNameDescending = false;
		//    SortByPriceAscending = true;
		//    SortByPriceDescending = false;
		//}

		//[RelayCommand]
		//private void SetSortOptionAsPriceDescending()
		//{
		//    SortByNameAscending = false;
		//    SortByNameDescending = false;
		//    SortByPriceAscending = false;
		//    SortByPriceDescending = true;
		//}


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
            if (SearchService.Category is null)
            {
                SearchService.SetCategory(CategoryList.FirstOrDefault(), true);
            }

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

				// Retrieve item categories from the category service.
				// This method is expected to return a list of item categories asynchronously.
				// The retrieved categories will be added to the categoryList collection.
				var response = await _categoryService.GetDataAsync();

				if (response.Success is false)
				{
					await Shell.Current.DisplayAlert("Error", response.ErrorMessage, "OK");
					return;
				}

				// Null or empty item list from API.
				if (response.Data is null || response.Data.Count == 0)
				{
					await Shell.Current.DisplayAlert("Sorry", "No items matched the search criteria", "OK");
					return;
				}

				// By assigning the fetched categories to the CategoryList, a notification is triggered
				// that the collection has changed and the OnCategoryListChanged method is called.
				CategoryList = [.. response.Data];
			}

			catch (Exception ex)
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
			//RestoreQueryToUserInterface(SearchState.CopyOfPreviousQuery);

			SearchService.CancelChanges();

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
			SearchService.ResetState(CategoryList.FirstOrDefault());

			// Present the newly reset search state to the user.
			//RestoreQueryToUserInterface(SearchState.CopyOfCurrentQuery);
		}


		/// <summary>
		/// Updates the user interface with the provided <see cref="SearchQueryDto"/>.
		/// This ensures that the user interface reflects the given search criteria.
		/// </summary>
		/// <param name="query">The <see cref="SearchQueryDto"/> containing the search criteria to restore.</param>
		//private void RestoreQueryToUserInterface(SearchQueryDto query)
		//{
		//    // NOTE : Null checks are necessary to avoid NullReferenceException.
		//    this.Keyword        = query.Keyword ?? string.Empty;
		//    this.Category       = query.Category ?? string.Empty;
		//    this.Subcategory    = query.Subcategory ?? string.Empty;
		//    this.Manufacturer   = query.Manufacturer ?? string.Empty;
		//    this.Material       = query.Material ?? string.Empty;

		//    // NOTE : Null checks are not actually required because ToString() would return an empty string if the value is null.
		//    this.MinimumPrice   = query.MinPrice.ToString() ?? string.Empty;
		//    this.MaximumPrice   = query.MaxPrice.ToString() ?? string.Empty;

		//    // Set the sort options based on the query.
		//    SetSortOption(query.SortBy);
		//}
	}
}