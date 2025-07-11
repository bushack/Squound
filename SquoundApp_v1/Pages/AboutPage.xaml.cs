using SquoundApp_v1.ViewModels;

namespace SquoundApp_v1.Pages;

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