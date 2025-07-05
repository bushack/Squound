using SquoundApp_v1.ViewModels;

namespace SquoundApp_v1.Pages;

public partial class AboutUsPage : ContentPage
{
	public AboutUsPage(AboutUsViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
    }
}