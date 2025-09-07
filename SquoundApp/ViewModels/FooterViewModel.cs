using CommunityToolkit.Mvvm.Input;

using SquoundApp.Interfaces;
using SquoundApp.Pages;


namespace SquoundApp.ViewModels
{
    public partial class FooterViewModel : BaseViewModel
    {
        private readonly INavigationService _Navigation;


        public FooterViewModel(INavigationService navigation)
        {
            _Navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
        }


        [RelayCommand]
        async Task GoToAboutPageAsync()
        {
            // Disallow redundant navigation to the same page.
            if (_Navigation.IsCurrentPage(nameof(AboutPage)))
                return;

            await _Navigation.GoToAsync(nameof(AboutPage));
        }


        [RelayCommand]
        async Task GoToSellPageAsync()
        {
            // Disallow redundant navigation to the same page.
            if (_Navigation.IsCurrentPage(nameof(SellPage)))
                return;

            await _Navigation.GoToAsync(nameof(SellPage));
        }
    }
}
