using SquoundApp.ViewModels;

namespace SquoundApp.Pages;

public partial class AboutPage : ContentPage
{
	public AboutPage(AboutViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

        _ = viewModel.InitializeAsync();
    }
}