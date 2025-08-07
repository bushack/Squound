using SquoundApp.ViewModels;


namespace SquoundApp.Pages;

public partial class CoarseSearchPage : ContentPage
{
	public CoarseSearchPage(CoarseSearchViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is CoarseSearchViewModel viewModel)
        {
            await viewModel.GetDataAsync();
        }
    }
}