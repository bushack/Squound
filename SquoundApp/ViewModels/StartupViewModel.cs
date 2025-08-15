using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SquoundApp.Services;


namespace SquoundApp.ViewModels
{
    public partial class StartupViewModel(CategoryService categoryService) : BaseViewModel
    {
        private readonly CategoryService _CategoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));

        [ObservableProperty]
        private string loadingMessage = "Connecting";

        [ObservableProperty]
        private string errorMessage = "Unable to establish a connection";

        [ObservableProperty]
        private bool canRetry = true;



        //
        [RelayCommand]
        public async Task GetDataAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var response = await _CategoryService.GetDataAsync();

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
