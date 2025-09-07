using SquoundApp.Interfaces;
using SquoundApp.ViewModels;


namespace SquoundApp.Pages;

public partial class AboutPage : ContentPage
{
	private readonly INavigationService _Navigation;


    public AboutPage(AboutViewModel viewModel, INavigationService navigation)
	{
		InitializeComponent();

		BindingContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
		_Navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));

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
			// Goes to the previous page. If no previous page exists, go to the HomePage.
			await _Navigation.GoBackOrHomeAsync();
		});

		return true;
	}
}