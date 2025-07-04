using SquoundApp_v1.ViewModels;

namespace SquoundApp_v1.Pages;

public partial class ProductSearchPage : ContentPage
{
	public ProductSearchPage(ProductSearchViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
    }
}