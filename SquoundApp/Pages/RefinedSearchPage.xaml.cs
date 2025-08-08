using SquoundApp.ViewModels;


namespace SquoundApp.Pages;

public partial class RefinedSearchPage : ContentPage
{
	public RefinedSearchPage(RefinedSearchViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is RefinedSearchViewModel viewModel)
        {
            // Every time the page appears we fetch the items matching the current criteria from the REST API.
            viewModel.ApplyQueryCommand.Execute(null);
        }
    }
}