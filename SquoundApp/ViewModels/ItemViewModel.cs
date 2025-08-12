using CommunityToolkit.Mvvm.ComponentModel;

using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using SquoundApp.Services;

using Shared.DataTransfer;


namespace SquoundApp.ViewModels
{
    [QueryProperty(nameof(ItemId), nameof(ItemId))]
    public partial class ItemViewModel(ItemService itemService, ILogger<ItemViewModel> logger) : BaseViewModel
    {
        private readonly ILogger<ItemViewModel> m_Logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // Responsible for retrieving items from the REST API.
        // This data is presented to the user on the ItemPage, where the user can view a specific item in detail.
        private readonly ItemService m_ItemService = itemService ?? throw new ArgumentNullException(nameof(itemService));

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
        private async Task LoadItemAsync(long id)
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                // Note that awaiting the result will unpack the ItemDetailDto from the containing Task object.
                var response = await m_ItemService.GetItemDetailAsync(id);

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
                m_Logger.LogWarning(ex, "Undefined error while retrieving item {id} from server.", id);

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
