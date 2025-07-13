using SquoundApp.ViewModels;

namespace SquoundApp.Pages;

public partial class SellPage : ContentPage
{
	public SellPage(SellViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;

        Title = nameof(SellPage);

        _ = viewModel.InitializeAsync();
    }
}