using CommunityToolkit.Mvvm.Input;
using SquoundApp.Pages;


namespace SquoundApp.ViewModels
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
            // Prevent navigation if the current page is already AboutPage.
            if (Shell.Current.CurrentPage.Title.Equals(nameof(AboutPage)))
                return;

            // This is the call that initiates the change of page.
            await Shell.Current.GoToAsync(nameof(AboutPage));
        }

        [RelayCommand]
        async Task GoToSellPageAsync()
        {
            // Prevent navigation if the current page is already SellPage.
            if (Shell.Current.CurrentPage.Title.Equals(nameof(SellPage)))
                return;

            // This is the call that initiates the change of page.
            await Shell.Current.GoToAsync(nameof(SellPage));
        }
    }
}
