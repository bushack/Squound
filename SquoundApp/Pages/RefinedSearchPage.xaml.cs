using SquoundApp.ViewModels;


namespace SquoundApp.Pages;

public partial class RefinedSearchPage : ContentPage
{
	public RefinedSearchPage(RefinedSearchViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        // This method is called when the page appears.
        // It is a good place to start any tasks that need to be done when the
        // page is shown, such as refreshing data or starting animations.
        base.OnAppearing();

        if (BindingContext is RefinedSearchViewModel viewModel)
        {
            // Execute the existing query command to ensure the view model is ready.
            viewModel.ApplyQueryCommand.Execute(null);
        }
    }
}