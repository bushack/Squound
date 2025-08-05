using SquoundApp.ViewModels;


namespace SquoundApp.Pages;

public partial class CoarseSearchPage : ContentPage
{
	public CoarseSearchPage(CoarseSearchViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is CoarseSearchViewModel viewModel)
        {
            // Every time the page appears we fetch the product categories from the REST API.
            viewModel.ApplyQueryCommand.Execute(null);
        }
    }
}