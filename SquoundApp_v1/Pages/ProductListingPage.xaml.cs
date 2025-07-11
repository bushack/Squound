using SquoundApp_v1.ViewModels;

namespace SquoundApp_v1.Pages;

public partial class ProductListingPage : ContentPage
{
	public ProductListingPage(ProductListingViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;

		Title = nameof(ProductListingPage);
    }

	protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        // This method is called when the page is navigated to.
        // You can use it to perform any actions that need to be done when the page is displayed.
        // For example, you can update the title of the page based on the product name.
        base.OnNavigatedTo(args);

		if (BindingContext is ProductListingViewModel viewModel && viewModel.Product != null)
		{
			Title = viewModel.Product.Name;
		}
    }

	protected override void OnAppearing()
	{
		base.OnAppearing();

		if (BindingContext is ProductListingViewModel viewModel && viewModel.Product != null)
		{
            // Default text to be used as Email subject and/or WhatsApp message.
            var message = $"I%20am%20interested%20in%20the%20{viewModel.Product.Name}";

            EmailButton.Url = $"mailto:squoundstuff@gmail.com" +
				$"?subject={message}" +		// Email subject.
                $"&body=";					// Email body.

            WhatsAppButton.Url = $"https://wa.me/447884002384" +
				$"?text={message}%20";		// WhatsApp message.
        }
    }
}