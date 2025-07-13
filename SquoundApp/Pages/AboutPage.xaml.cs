using SquoundApp.ViewModels;

namespace SquoundApp.Pages;

public partial class AboutPage : ContentPage
{
	public AboutPage(AboutViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;

        Title = nameof(AboutPage);

        _ = viewModel.InitializeAsync();
    }
}