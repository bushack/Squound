using CommunityToolkit.Mvvm.ComponentModel;

using System.Diagnostics;

using SquoundApp.Services;

using Shared.DataTransfer;
using Microsoft.IdentityModel.Tokens;


namespace SquoundApp.ViewModels
{
    [QueryProperty(nameof(ItemId), "ItemId")]
    public partial class ItemViewModel(ItemService itemService) : BaseViewModel
    {
        // Responsible for retrieving items from the REST API.
        // This data is presented to the user on the ItemPage, where the user can view a specific item in detail.
        private readonly ItemService itemService = itemService ?? throw new ArgumentNullException(nameof(itemService));

        //
        [ObservableProperty]
        private ItemDetailDto item = new();

        //
        private long itemId;

        //
        public long ItemId
        {
            get => itemId;

            set
            {
                // 'itemId' is passed as a reference parameter from RefinedSearchViewModel.
                SetProperty(ref itemId, value);

                LoadItemAsync(itemId);
            }
        }


        //
        public bool HasMaterial => Item.Material.IsNullOrEmpty() is false;


        //
        public string Material => $"Primary material: {Item.Material}";


        //
        public bool HasDimensions => Item.Width > 0 && Item.Height > 0 && Item.Depth > 0;


        //
        public string Dimensions => $"Dimensions: (w) {Item.Width}mm, (d) {Item.Depth}mm, (h) {Item.Height}mm";


        //
        private async void LoadItemAsync(long id)
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                // Item cannot be null as GetItemDetailsAsync always returns a valid object.
                // Note that awaiting the result will unpack the ItemDetailDto from the containing Task object.
                Item = await itemService.GetItemDetailAsync(id);
            }

            catch (Exception ex)
            {
                // Handle exceptions, e.g., log them.
                Debug.WriteLine($"Error while attempting to fetch item {id}: {ex.Message}");

                // Display an alert to the user indicating that an error occurred while fetching data.
                await Shell.Current.DisplayAlert(
                    "Error",
                    "An undefined error occurred while fetching the item from the server",
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
