using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using SquoundApp.Interfaces;
using SquoundApp.Services;


namespace SquoundApp.ViewModels
{
    public partial class StartupViewModel : BaseViewModel
    {
        private readonly ILogger<StartupViewModel> _Logger;
        private readonly ICategoryService _Categories;

        [ObservableProperty]
        private string loadingMessage = "Connecting";

        [ObservableProperty]
        private string errorMessage = "Unable to establish a connection";

        [ObservableProperty]
        private bool canRetry = true;


        //
        public StartupViewModel(ILogger<StartupViewModel> logger, ICategoryService categories)
        {
            _Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _Categories = categories ?? throw new ArgumentNullException(nameof(categories));
        }



        //
        [RelayCommand]
        public async Task GetDataAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var response = await _Categories.GetDataAsync();

                if (response.Success)
                {
                    await ServiceLocator.GetService<NavigationService>().GoToAsync("///HomePage");
                }

                else
                {
                    CanRetry = true;
                    await ServiceLocator.GetService<NavigationService>().GoToAsync("///ErrorPage");
                }
            }

            catch (Exception)
            {
                // Display an alert to the user indicating that an error occurred while fetching data.
                CanRetry = false;
                await Shell.Current.DisplayAlert("Error", "An unknown error occurred. Please restart Squound app.", "OK");
            }

            finally
            {
                IsBusy = false;
            }
        }


        //
        [RelayCommand]
        private async Task RetryAsync()
        {
            await ServiceLocator.GetService<NavigationService>().GoToAsync("///LoadingPage");

            await GetDataAsync();
        }
    }
}
