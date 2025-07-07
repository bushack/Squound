using SquoundApp_v1.ViewModels;

namespace SquoundApp_v1.Pages;

public partial class SellPage : ContentPage
{
	public SellPage(SellViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;

        _ = viewModel.InitializeAsync();
    }
}