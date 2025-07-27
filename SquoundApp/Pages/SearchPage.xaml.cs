using SquoundApp.ViewModels;


namespace SquoundApp.Pages;

public partial class SearchPage : ContentPage, IQueryAttributable
{
	public SearchPage(SearchViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;

        Title = nameof(SearchPage);
    }

    //protected override void OnNavigatedTo(NavigatedToEventArgs args)
    //{
    //    // This method is called when the page is navigated to.
    //    // You can use it to perform any actions that need to be done when the page is displayed.
    //    // For example, you can update the title of the page based on the product name.
    //    base.OnNavigatedTo(args);

    //    if ((BindingContext is ProductSearchViewModel viewModel)
    //        && (viewModel.ProductList != null)      // Ensure the viewModel has a ProductList.
    //        && (viewModel.ProductList.Count == 0))  // Don't reload if products are already loaded.
    //    {
    //        viewModel.GetProductsCommand.Execute(null);
    //    }
    //}

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("category", out var categoryObj) && categoryObj is string category)
        {
            if (BindingContext is SearchViewModel viewModel)
            {
                // Previously we passed the category as a string, but now we use the ProductQueryDto.
                // This allows us to pass more parameters if needed in the future.
                // Therefore this method is effectively obsolete.
                viewModel.GetProductsCommand.Execute(null);
            }
        }
    }
}