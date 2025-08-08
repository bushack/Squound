using CommunityToolkit.Mvvm.ComponentModel;

using Shared.DataTransfer;


namespace SquoundApp.ViewModels
{
    [QueryProperty(nameof(Item), "Item")]
    public partial class ItemViewModel : BaseViewModel
    {
        // NOTE - The [QueryProperty] attribute is used to bind the Item property
        // to a query parameter named "Item". This allows the ItemViewModel
        // to receive a Item object when the page is navigated to, enabling it to
        // display the details of the selected item.
        // This binding is typically set up in the navigation logic of the application,
        // where the Item object is passed as a parameter when navigating to the ItemPage.
        // See SquoundApp/Pages/RefinedSearchPage.xaml.cs for the navigation logic which can be
        // found in the TapGestureRecognizer command handler for the item item.
        [ObservableProperty]
        private ItemDto item = new();
    }
}
