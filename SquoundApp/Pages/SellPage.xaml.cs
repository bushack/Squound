using SquoundApp.ViewModels;

namespace SquoundApp.Pages;

public partial class SellPage : ContentPage
{
	public SellPage(SellViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

        _ = viewModel.InitializeAsync();
    }
}