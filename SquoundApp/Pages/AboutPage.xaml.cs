using SquoundApp.Services;
using SquoundApp.ViewModels;


namespace SquoundApp.Pages;

public partial class AboutPage : ContentPage
{
	public AboutPage(AboutViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

        _ = viewModel.InitializeAsync();
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
			var navService = ServiceLocator.GetService<NavigationService>();

			// Goes to the previous page. If no previous page exists, go to the HomePage.
			await navService.GoBackOrHomeAsync();
		});

		return true;
	}
}