using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using CommunityToolkit.Mvvm.ComponentModel;

using SquoundApp.Exceptions;
using SquoundApp.Interfaces;

using Shared.DataTransfer;


// TODO : Detect and render an empty ItemDetailDto differently when exception caught.
// (e.g., show "No details available" instead of blank labels).


namespace SquoundApp.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public partial class ItemViewModel : BaseViewModel
    {
        private readonly ILogger<ItemViewModel> _Logger;
        private readonly IItemDetailRepository _Repository;

        // For user interface data binding.
        [ObservableProperty]
        private ItemDetailDto item = new();


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger">Reference to a logger instance.</param>
        /// <param name="repository">Reference to an item repository instance.</param>
        /// <exception cref="ArgumentNullException">Thrown if any parameter is null.</exception>
        public ItemViewModel(ILogger<ItemViewModel> logger, IItemDetailRepository repository)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _Repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }


        // For receiving the ItemId as a QueryProperty.
        private long itemId;
        public long ItemId
        {
            get => itemId;
            set
            {
                // 'itemId' is passed as a reference parameter from RefinedSearchViewModel.
                if (SetProperty(ref itemId, value))
                {
                    // Fire and forget, but safe because exceptions handled inside LoadItemAsync.
                    _ = GetItemAsync(itemId);
                }
            }
        }


        // Material information.
        public bool HasMaterial => Item.Material.IsNullOrEmpty() is false;
        public string Material => $"Primary material: {Item.Material}";


        // Dimensions information.
        public bool HasDimensions => Item.Width > 0 && Item.Height > 0 && Item.Depth > 0;
        public string Dimensions => $"Dimensions: (w) {Item.Width}mm, (d) {Item.Depth}mm, (h) {Item.Height}mm";


        /// <summary>
        /// Retrieves item detail from the repository asynchronously and handles exceptions.
        /// </summary>
        /// <param name="itemId">The unique identifier of the item to retrieve.</param>
        private async Task GetItemAsync(long itemId)
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;
                Item = await _Repository.GetItemAsync(itemId);
            }

            catch (ItemRepositoryException ex)
            {
                _Logger.LogWarning(ex, "Invalid response while attempting to retrieve Item Id: {itemId}.", itemId);

                // Assign a new empty item to avoid null reference exceptions in the view.
                Item = ItemDetailDto.Empty;

                // Display an alert to the user indicating that an error occurred while fetching data.
                await Shell.Current.DisplayAlert(
                    "Error",
                    $"An error occurred while attempting to retrieve the item.",
                    "OK");
            }

            catch (Exception ex)
            {
                _Logger.LogWarning(ex, "Undefined error while attempting to retrieve Item Id: {itemId}.", itemId);

                // Assign a new empty item to avoid null reference exceptions in the view.
                Item = ItemDetailDto.Empty;

                // Display an alert to the user indicating that an error occurred while fetching data.
                await Shell.Current.DisplayAlert(
                    "Error",
                    $"An undefined error occurred while attempting to retrieve the item.",
                    "OK");
            }

            finally
            {
                IsBusy = false;
            }
        }


        /// <summary>
        /// Responsible for updating dependent properties when Item changes.
        /// </summary>
        /// <param name="value"></param>
        partial void OnItemChanged(ItemDetailDto value)
        {
            OnPropertyChanged(nameof(Dimensions));
            OnPropertyChanged(nameof(HasDimensions));

            OnPropertyChanged(nameof(Material));
            OnPropertyChanged(nameof(HasMaterial));
        }
    }
}
