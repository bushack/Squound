using SquoundApp.Interfaces;
using SquoundApp.ViewModels;


namespace SquoundApp.Pages;

public partial class ItemPage : ContentPage
{
    private readonly INavigationService _Navigation;


    public ItemPage(ItemViewModel viewModel, INavigationService navigation)
	{
		InitializeComponent();

		BindingContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        _Navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
    }

	protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        // This method is called when the page is navigated to.
        // You can use it to perform any actions that need to be done when the page is displayed.
        // For example, you can update the title of the page based on the Item name.
        base.OnNavigatedTo(args);

		if (BindingContext is ItemViewModel viewModel && viewModel.Item != null)
		{
			Title = viewModel.Item.Name;
		}
    }

	protected override void OnAppearing()
	{
		base.OnAppearing();

		if (BindingContext is ItemViewModel viewModel && viewModel.Item != null)
		{
            // Default text to be used as Email subject and/or WhatsApp message.
            var message = $"I%20am%20interested%20in%20the%20{viewModel.Item.Name}";

            EmailButton.Url = $"mailto:squoundstuff@gmail.com" +
				$"?subject={message}" +		// Email subject.
                $"&body=";					// Email body.

            WhatsAppButton.Url = $"https://wa.me/447884002384" +
				$"?text={message}%20";		// WhatsApp message.
        }
    }


    /// <summary>
    /// This is the handler for Operating System/Hardware back button events such as those
    /// used by the Android and Windows Operating Systems.
    /// For the iOS handler, see the BackButtonView class which creates a XAML back button
    /// that is a part of the user interface.
    /// </summary>
    protected override bool OnBackButtonPressed()
    {
        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await _Navigation.GoBackOrHomeAsync();
        });

        return true;
    }
}