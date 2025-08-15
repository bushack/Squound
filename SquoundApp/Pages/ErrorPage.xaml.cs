using SquoundApp.ViewModels;


namespace SquoundApp.Pages;

public partial class ErrorPage : ContentPage
{
	public ErrorPage(StartupViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}