using SquoundApp.ViewModels;


namespace SquoundApp.Pages;

public partial class CoarseSearchPage : ContentPage
{
	public CoarseSearchPage(CoarseSearchViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;

        Title = nameof(CoarseSearchPage);
    }

    protected override void OnAppearing()
    {
        // This method is called when the page appears.
        // It is a good place to start any tasks that need to be done when the
        // page is shown, such as refreshing data or starting animations.
        base.OnAppearing();

        if (BindingContext is CoarseSearchViewModel viewModel)
        {
            // Execute the existing query command to ensure the view model is ready.
            viewModel.ApplyQueryCommand.Execute(null);
        }
    }
}