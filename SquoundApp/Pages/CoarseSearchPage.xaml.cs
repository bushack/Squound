using SquoundApp.Interfaces;
using SquoundApp.ViewModels;


namespace SquoundApp.Pages;

public partial class CoarseSearchPage : ContentPage
{
    private readonly INavigationService _Navigation;


    public CoarseSearchPage(CoarseSearchViewModel viewModel, INavigationService navigation)
	{
		InitializeComponent();

		BindingContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        _Navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is CoarseSearchViewModel viewModel)
        {
            await viewModel.GetDataAsync();
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