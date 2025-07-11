using SquoundApp_v1.ViewModels;


namespace SquoundApp_v1.Pages;

public partial class ProductSearchPage : ContentPage
{
	public ProductSearchPage(ProductSearchViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;

        Title = nameof(ProductSearchPage);
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        // This method is called when the page is navigated to.
        // You can use it to perform any actions that need to be done when the page is displayed.
        // For example, you can update the title of the page based on the product name.
        base.OnNavigatedTo(args);

        if ((BindingContext is ProductSearchViewModel viewModel)
            && (viewModel.ProductList != null)      // Ensure the viewModel has a ProductList.
            && (viewModel.ProductList.Count == 0))  // Don't reload if products are already loaded.
        {
            viewModel.GetProductsCommand.Execute(null);
        }
    }
}