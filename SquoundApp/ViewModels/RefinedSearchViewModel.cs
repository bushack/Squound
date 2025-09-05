using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using Shared.DataTransfer;
using SquoundApp.Interfaces;
using SquoundApp.Pages;
using SquoundApp.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;


namespace SquoundApp.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public partial class RefinedSearchViewModel : BaseViewModel
    {
        private readonly ILogger<RefinedSearchViewModel> _Logger;
        private readonly IItemService _Items;
        private readonly ISearchContext _Search;

        // Collection of items retrieved from the REST API based on the current search criteria.
        public ObservableCollection<ItemSummaryDto> ItemList { get; } = [];

        // For UI to display number of items found by most recent database search.
        public string ItemsFound => TotalItems == 1 ? $"{TotalItems} Item" : $"{TotalItems} Items";

        // For UI to display current page number and total number of pages.
        public string PageNumber => $"Page {CurrentPage} of {TotalPages}";

        //For UI on/off toggling of PageNumber label.
        public bool HasAnyItems => TotalItems > 0;
        
        // For UI on/off toggling of Next Page button.
        public bool HasNextPage => CurrentPage < TotalPages;

        // For UI on/off toggling of Previous Page button.
        public bool HasPrevPage => CurrentPage > 1;

        // Total number of items returned by the item service.
        [ObservableProperty]
        private int totalItems = 0;

        // Total number of pages required to display all items at the current page size.
        [ObservableProperty]
        private int totalPages = 0;

        // Number of the page currently displayed by the user interface.
        [ObservableProperty]
        private int currentPage = 0;


        //
        public RefinedSearchViewModel(ILogger<RefinedSearchViewModel> logger, IItemService items, ISearchContext search)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _Items = items ?? throw new ArgumentNullException(nameof(items));
            _Search = search ?? throw new ArgumentNullException(nameof(search));
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


        //
        [RelayCommand]
        private async Task NextPageAsync()
        {
            if (HasNextPage)
            {
                _Search.IncrementPageNumber();
                await ApplyQueryAsync();
            }
        }


        //
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

                // Save a deep copy of the current query to the PreviousQuery property.
                // This is useful for scenarios where you might want to revert to the
                // previous query or to compare the current query with the previous one.
                //_SearchService.SaveState();

                // Clear the existing items in the ObservableCollection.
                // This ensures that the collection is updated with whatever is returned by the API.
                // ObservableCollection is used here so that the UI can automatically update
                // when items are added or removed, without needing to manually refresh the UI.
                // This is a key feature of ObservableCollection, which is designed for data binding in UI frameworks.
                ItemList.Clear();

                // Reset pagination metadata for user.
                TotalItems = 0;
                TotalPages = 0;
                CurrentPage = 0;

                // Prepare page title.
                Title = _Search.Keyword ??
                        _Search.Subcategory?.Name ??
                        _Search.Category?.Name ??
                        "Search";

                // Retrieve items from the item service.
                // This method is expected to return a list of items asynchronously.
                // The retrieved items will be added to the itemList collection.
                // To retrieve items from a remote JSON file, use:
                // var itemList = await _ItemService.GetItemsRemoteJson
                // ("https://raw.githubusercontent.com/bushack/files/refs/heads/main/items.json");
                // To retrieve items from an embedded JSON file instead, use:
                // var itemList = await _ItemService.GetItemsEmbeddedJson();
                var response = await _Items.GetDataAsync(_Search);

                // Null response from API.
                if (response.Success is false)
                {
                    await Shell.Current.DisplayAlert("Error", response.ErrorMessage, "OK");
                    return;
                }

                // Null or empty item list from API.
                if (response.Data is null || response.Data.TotalItems == 0)
                {
                    await Shell.Current.DisplayAlert("Sorry", "No items matched the search criteria", "OK");
                    return;
                }

                // Prepare new pagination metadata for user.
                TotalItems  = response.Data.TotalItems;
                TotalPages  = response.Data.TotalPages;
                CurrentPage = response.Data.CurrentPage;

                // The foreach loop iterates over the fetched items and adds each one to the Items collection.
                // This ensures that the UI reflects the latest data.
                // The Items collection is an ObservableCollection, which means that any changes to it
                // will automatically notify the UI to update, making it easy to display dynamic data.
                // This is particularly useful in MVVM (Model-View-ViewModel) patterns where the ViewModel
                // holds the data and the View binds to it.
                //ItemList.Clear();
                foreach (var item in response.Data.Items)
                {
                    // Add each item to the ObservableCollection.
                    // This will trigger the UI to update and display the new items.
                    // The ObservableCollection is designed to notify the UI of changes,
                    // so when we add items to it, the UI will automatically reflect those changes.
                    // ObservableRangeCollection would be more efficient if you want to add multiple items at once,
                    // and delay the UI update until all items are added.
                    ItemList.Add(item);
                }
            }

            catch (Exception ex)
            {
                // Handle exceptions, e.g., log them.
                Debug.WriteLine($"Search error while attempting to fetch data: {ex.Message}");

                // Display an alert to the user indicating that an error occurred while fetching data.
                await Shell.Current.DisplayAlert(
                    "Error",
                    "An undefined error occurred while fetching data from the server",
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

            var navService = ServiceLocator.GetService<NavigationService>();
            await navService.GoToAsync(nameof(ItemPage), true,
                new Dictionary<string, object>
                {
                    {"ItemId", item.ItemId }
                });
        }
    }
}
