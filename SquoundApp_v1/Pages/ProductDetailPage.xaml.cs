using SquoundApp_v1.ViewModels;

namespace SquoundApp_v1.Pages;

public partial class ProductDetailPage : ContentPage
{
	public ProductDetailPage(ProductDetailViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
    }

	protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        // This method is called when the page is navigated to.
        // You can use it to perform any actions that need to be done when the page is displayed.
        // For example, you can update the title of the page based on the product name.
        base.OnNavigatedTo(args);

		if (BindingContext is ProductDetailViewModel viewModel && viewModel.Product != null)
		{
			Title = viewModel.Product.Name;
		}
    }
}