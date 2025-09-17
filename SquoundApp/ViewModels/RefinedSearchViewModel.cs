using Microsoft.Extensions.Logging;

using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SquoundApp.Exceptions;
using SquoundApp.Interfaces;
using SquoundApp.Pages;

using Shared.DataTransfer;


namespace SquoundApp.ViewModels
{
	public partial class RefinedSearchViewModel : BaseViewModel
	{
		private readonly ILogger<RefinedSearchViewModel> _Logger;
		private readonly IItemSummaryRepository _Repository;
		private readonly INavigationService _Navigation;
		private readonly ISearchContext _Search;

		// Collection of items retrieved from the REST API based on the current search criteria.
		[ObservableProperty]
		private ObservableCollection<ItemSummaryDto> itemList = [];

		// For UI to display number of items found by most recent database search.
		public string ItemsFound => TotalItems == 1 ? $"{TotalItems} Item" : $"{TotalItems} Items";

		// For UI to display current page number and total number of pages.
		public string PageNumber => $"Page {CurrentPage} of {TotalPages}";

		//For UI on/off toggling of PageNumber label.
		public bool HasAnyItems => _Repository.IsNotEmpty;
		
		// For UI on/off toggling of Next Page button.
		public bool HasNextPage => _Repository.HasNextPage;

		// For UI on/off toggling of Previous Page button.
		public bool HasPrevPage => _Repository.HasPrevPage;

		// Total number of items returned by the item service.
		[ObservableProperty]
		private int totalItems = 0;

		// Total number of pages required to display all items at the current page size.
		[ObservableProperty]
		private int totalPages = 0;

		// Number of the page currently displayed by the user interface.
		[ObservableProperty]
		private int currentPage = 0;


		public RefinedSearchViewModel(ILogger<RefinedSearchViewModel> logger, IItemSummaryRepository repository,
			INavigationService navigation, ISearchContext search)
		{
			_Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _Repository = repository ?? throw new ArgumentNullException(nameof(repository));
			_Navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
			_Search = search ?? throw new ArgumentNullException(nameof(search));
		}


		/// <summary>
		/// Logic to be executed when the content of the ItemList changes.
		/// </summary>
		/// <param name="value"></param>
        partial void OnItemListChanged(ObservableCollection<ItemSummaryDto> value)
        {
			// Update page title.
			// Note that Keyword overrides Subcategory, which overrides Category.
			Title = _Search.Keyword ??
					_Search.Subcategory?.Name ??
					_Search.Category?.Name ??
					"Search";
        }


		/// <summary>
		/// Invokes a property changed event for search response information variables.
		/// This will ensure that the user interface is updated as soon as a value changes.
		/// </summary>
		/// <param name="value">New total items value.</param>
		partial void OnTotalItemsChanged(int value)
		{
			OnPropertyChanged(nameof(HasAnyItems));
			OnPropertyChanged(nameof(ItemsFound));
		}


		/// <summary>
		/// Invokes a property changed event for pagination variables.
		/// This will ensure that the user interface is updated as soon as a value changes.
		/// </summary>
		/// <param name="value">New total pages value.</param>
		partial void OnTotalPagesChanged(int value)
		{
			OnPropertyChanged(nameof(HasNextPage));
			OnPropertyChanged(nameof(HasPrevPage));
			OnPropertyChanged(nameof(PageNumber));
		}


		/// <summary>
		/// Invokes a property changed event for pagination variables.
		/// This will ensure that the user interface is updated as soon as a value changes.
		/// </summary>
		/// <param name="value">New current page value.</param>
		partial void OnCurrentPageChanged(int value)
		{
			OnPropertyChanged(nameof(HasNextPage));
			OnPropertyChanged(nameof(HasPrevPage));
			OnPropertyChanged(nameof(PageNumber));
		}


		[RelayCommand]
		private async Task NextPageAsync()
		{
			if (HasNextPage)
			{
				_Search.IncrementPageNumber();
				await ApplyQueryAsync();
			}
		}


		[RelayCommand]
		private async Task PrevPageAsync()
		{
			if (HasPrevPage)
			{
				_Search.DecrementPageNumber();
				await ApplyQueryAsync();
			}
		}


		/// <summary>
		/// Query command that is executed whenever the RefinedSearchPage appears.
		/// This command is responsible for fetching items based on the current search criteria.
		/// If the search criteria changes the application should re-navigate to the RefinedSearchPage
		/// and this command will automatically execute a fetch of the latest items.
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		private async Task ApplyQueryAsync()
		{
			// Check if the view model is already busy fetching data.
			// This prevents multiple simultaneous fetch operations which could
			// lead to performance issues or unexpected behaviour.
			// This is a common pattern to avoid re-entrance issues in async methods.
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

				// Clear the existing items in the ObservableCollection.
				// This ensures that the collection is updated with whatever is returned by the API.
				// ObservableCollection is used here so that the UI can automatically update
				// when items are added or removed, without needing to manually refresh the UI.
				// This is a key feature of ObservableCollection, which is designed for data binding in UI frameworks.
				//ItemList.Clear();

				// Assign the items supplied by the repository to the observable collection (updates UI).
				// Note that ObservableCollection constructor accepts IEnumerable<ItemSummaryDto> type.
				ItemList = [.. await _Repository.GetItemsAsync(_Search)];

                // Prepare new pagination metadata for user interface.
                TotalItems	= _Repository.TotalItems;
				TotalPages	= _Repository.TotalPages;
				CurrentPage = _Repository.CurrentPage;
			}

			catch (ItemRepositoryException ex)
			{
                _Logger.LogWarning(ex, "Invalid response while attempting to retrieve items.");

                // Display an alert to the user indicating that an error occurred while fetching data.
                await Shell.Current.DisplayAlert(
					"Error",
					"An error occurred while attempting to retrieve items.",
					"OK");
            }

            catch (Exception ex)
            {
                _Logger.LogWarning(ex, "Undefined error while attempting to retrieve items.");

                // Display an alert to the user indicating that an error occurred while fetching data.
                await Shell.Current.DisplayAlert(
					"Error",
                    "An undefined error occurred while attempting to retrieve items.",
					"OK");
			}

			finally
			{
				IsBusy = false;
			}
		}


		/// <summary>
		/// Asynchronously initiates a navigation to the ItemPage of parameter 'item'.
		/// </summary>
		/// <param name="item">Data transfer object of the item to be displayed.</param>
		/// <returns></returns>
		[RelayCommand]
		async Task GoToItemPageAsync(ItemSummaryDto item)
		{
			if (item is null)
				return;

			// Record the selected item's identifier in the search context.
			_Search.ItemId = item.ItemId;

			// TODO: Pre-fetch the item detail before instigating navigation?

			// Navigate to show the selected item in detail.
			await _Navigation.GoToAsync(nameof(ItemDetailPage));


			// Depreciated.
			//await _Navigation.GoToAsync(nameof(ItemPage), true,
			//	new Dictionary<string, object>
			//	{
			//		{"ItemId", item.ItemId }
			//	});
		}


        /// <summary>
        /// For optimal image display, sets the required image dimensions in pixels.
        /// </summary>
        /// <param name="width">Required image width in pixels.</param>
		/// <param name="height">Required image height in pixels.</param>
        public void RequiredImageDimensions(int width, int height)
		{
			_Search.RequiredImageWidth = width;
			_Search.RequiredImageHeight = height;
        }
    }
}