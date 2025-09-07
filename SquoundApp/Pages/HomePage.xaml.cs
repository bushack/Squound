using SquoundApp.Interfaces;


namespace SquoundApp.Pages;

public partial class HomePage : ContentPage
{
    private readonly INavigationService _Navigation;


    public HomePage(INavigationService navigation)
	{
		InitializeComponent();

        _Navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
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