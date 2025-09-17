using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SquoundApp.Exceptions;
using SquoundApp.Interfaces;

using Shared.DataTransfer;
using SquoundApp.Resources.Strings;


// TODO : Detect and render an empty ItemDetailDto differently when exception caught.
// (e.g., show "No details available" instead of blank labels).


namespace SquoundApp.ViewModels
{
    public partial class ItemDetailViewModel : BaseViewModel
    {
        private readonly ILogger<ItemDetailViewModel> _Logger;
        private readonly IItemDetailRepository _Repository;
        private readonly ISearchContext _Search;

        // For user interface data binding.
        [ObservableProperty]
        private ItemDetailDto item = new();


        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="logger">Reference to a logger instance.</param>
        /// <param name="repository">Reference to an item repository instance.</param>
        /// <exception cref="ArgumentNullException">Thrown if any parameter is null.</exception>
        public ItemDetailViewModel(ILogger<ItemDetailViewModel> logger, IItemDetailRepository repository, ISearchContext search)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _Repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _Search = search ?? throw new ArgumentNullException(nameof(search));
        }


        // Material.
        public bool HasMaterial =>
            Item.Material.IsNullOrEmpty() is false;

        public string Material =>
            $"Primary material: {Item.Material}";


        // Dimensions.
        public bool HasDimensions =>
            Item.Width > 0 &&
            Item.Height > 0 &&
            Item.Depth > 0;

        public string Dimensions =>
            $"Dimensions:" +
            $"(w) {Item.Width}mm," +
            $"(d) {Item.Depth}mm," +
            $"(h) {Item.Height}mm";


        // Email Us.
        public string EmailUrl =>
            $"mailto:{AppResources.UrlEmail}" + 
            $"?subject={AppResources.UserInterestedMessage}{Item.Name}" +   // Email subject.
            $"&body=";                                                      // Email body.


        // WhatsApp Us.
        public string WhatsAppUrl =>
            $"{AppResources.UrlWhatsApp}" +
            $"?text={AppResources.UserInterestedMessage}{Item.Name}%20";	// WhatsApp message.


        /// <summary>
        /// Retrieves item detail from the repository asynchronously and handles exceptions.
        /// </summary>
        [RelayCommand]
        private async Task GetItemAsync()
        {
            if (IsBusy)
            {
                return;
            }

            try
            {
                IsBusy = true;
                Item = await _Repository.GetItemAsync(_Search);
            }

            catch (ItemRepositoryException ex)
            {
                _Logger.LogWarning(ex, "Invalid response while attempting to retrieve Item Id: {itemId}.", _Search.ItemId);

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
                _Logger.LogWarning(ex, "Undefined error while attempting to retrieve Item Id: {itemId}.", _Search.ItemId);

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
            // Set the page title to the name of the item.
            Title = value.Name;

            OnPropertyChanged(nameof(Dimensions));
            OnPropertyChanged(nameof(HasDimensions));

            OnPropertyChanged(nameof(Material));
            OnPropertyChanged(nameof(HasMaterial));

            OnPropertyChanged(nameof(EmailUrl));
            OnPropertyChanged(nameof(WhatsAppUrl));
        }
    }
}