using CommunityToolkit.Mvvm.Input;
using SquoundApp_v1.Pages;

namespace SquoundApp_v1.ViewModels
{
    public partial class FooterViewModel : BaseViewModel
    {
        public FooterViewModel()
        {
            Title = "Footer";
        }

        [RelayCommand]
        async Task GoToAboutPageAsync()
        {
            // This is the call that initiates the change of page.
            await Shell.Current.GoToAsync(nameof(AboutPage));
        }

        [RelayCommand]
        async Task GoToSellPageAsync()
        {
            // This is the call that initiates the change of page.
            await Shell.Current.GoToAsync(nameof(SellPage));
        }
    }
}
