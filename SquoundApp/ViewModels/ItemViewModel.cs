using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using SquoundApp.Interfaces;

using Shared.DataTransfer;


namespace SquoundApp.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public partial class ItemViewModel : BaseViewModel
    {
        private readonly ILogger<ItemViewModel> _Logger;
        private readonly IItemService _Items;

        // Item
        [ObservableProperty]
        private ItemDetailDto item = new();

        // ItemId
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
                    _ = LoadItemAsync(itemId);
                }
            }
        }


        // Material
        public bool HasMaterial => Item.Material.IsNullOrEmpty() is false;
        public string Material => $"Primary material: {Item.Material}";


        // Dimensions
        public bool HasDimensions => Item.Width > 0 && Item.Height > 0 && Item.Depth > 0;
        public string Dimensions => $"Dimensions: (w) {Item.Width}mm, (d) {Item.Depth}mm, (h) {Item.Height}mm";


        //
        public ItemViewModel(ILogger<ItemViewModel> logger, IItemService items)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _Items = items ?? throw new ArgumentNullException(nameof(items));
        }


        //
        private async Task LoadItemAsync(long id)
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                // Note that awaiting the result will unpack the ItemDetailDto from the containing Task object.
                var response = await _Items.GetDataAsync(id);

                if (response.Success is false)
                {
                    Item = new();
                    await Shell.Current.DisplayAlert($"Error", response.ErrorMessage, "OK");
                    return;
                }

                if (response.Data is null || response.Data.ItemId != id)
                {
                    Item = new();
                    await Shell.Current.DisplayAlert($"Error", "Item {id} not found", "OK");
                    return;
                }

                // Successfully retrieved the item.
                Item = response.Data;
            }

            catch (Exception ex)
            {
                _Logger.LogWarning(ex, "Undefined error while retrieving item {id} from server.", id);

                // Display an alert to the user indicating that an error occurred while fetching data.
                await Shell.Current.DisplayAlert(
                    "Error",
                    $"An undefined error occurred while retrieving item {id} from the server",
                    "OK");
            }

            finally
            {
                IsBusy = false;
            }
        }


        //
        partial void OnItemChanged(ItemDetailDto value)
        {
            OnPropertyChanged(nameof(Dimensions));
            OnPropertyChanged(nameof(HasDimensions));

            OnPropertyChanged(nameof(Material));
            OnPropertyChanged(nameof(HasMaterial));
        }
    }
}
