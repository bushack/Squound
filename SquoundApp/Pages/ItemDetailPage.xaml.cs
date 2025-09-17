using SquoundApp.Interfaces;
using SquoundApp.ViewModels;


namespace SquoundApp.Pages;

public partial class ItemDetailPage : ContentPage
{
    private readonly INavigationService _Navigation;


    public ItemDetailPage(ItemDetailViewModel viewModel, INavigationService navigation)
    {
        InitializeComponent();

        BindingContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        _Navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
    }


    /// <summary>
    /// Logic to be executed immediately prior to the page becoming visible.
    /// </summary>
    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ItemDetailViewModel viewModel)
        {
            // Request the item data from the ViewModel.
            // Note that the ViewModel has access to the SearchContext and
            // therefore knows the ID of the Item to display.
            viewModel.GetItemCommand.Execute(null);
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