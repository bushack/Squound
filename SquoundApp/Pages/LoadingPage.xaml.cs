using SquoundApp.ViewModels;


namespace SquoundApp.Pages;

public partial class LoadingPage : ContentPage
{
	public LoadingPage(StartupViewModel viewModel)
	{
		InitializeComponent();

        BindingContext = viewModel;
    }


    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is StartupViewModel viewModel)
        {
            await viewModel.GetDataAsync();
        }
    }
}