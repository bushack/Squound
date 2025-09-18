using SquoundApp.Interfaces;
using SquoundApp.ViewModels;


namespace SquoundApp.Pages;

public partial class ItemSummaryPage : ContentPage
{
    private readonly INavigationService _Navigation;

    private int _RequiredImageWidth = 0;
    private int _RequiredImageHeight = 0;
    private bool _RequiredImageDimensionsSet = false;


    public ItemSummaryPage(ItemSummaryViewModel viewModel, INavigationService navigation)
	{
		InitializeComponent();

		BindingContext = viewModel ?? throw new ArgumentNullException(nameof(viewModel));
        _Navigation = navigation ?? throw new ArgumentNullException(nameof(navigation));
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ItemSummaryViewModel viewModel)
        {
            // Every time the page appears we fetch the items matching the current criteria from the REST API.
            //viewModel.ApplyQueryCommand.Execute(null);
        }
    }


    protected async override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

        if (BindingContext is ItemSummaryViewModel viewModel)
        {
            await viewModel.OnNavigatedTo();
        }
    }


    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);

        if (BindingContext is ItemSummaryViewModel viewModel)
        {
            viewModel.OnNavigatedFrom();
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


    /// <summary>
    /// Handler for item image border size changes.
    /// </summary>
    private void ImageBorder_SizeChanged(object sender, EventArgs e)
    {
        if (_RequiredImageDimensionsSet)
            return;

        if (sender is not Border imageBorder)
            return;

        if (BindingContext is not ItemSummaryViewModel viewModel)
            return;

        // Calculate the required image width in device pixels.
        // Performance note: This code runs only once per page instance.
        var borderWidth = (int)Math.Ceiling(imageBorder.Width);
        var borderHeight = (int)Math.Ceiling(imageBorder.Height);
        var displayDensity = (int)Math.Ceiling(DeviceDisplay.MainDisplayInfo.Density);

        _RequiredImageWidth = borderWidth * displayDensity;
        _RequiredImageHeight = borderHeight * displayDensity;
        _RequiredImageDimensionsSet = true;

        viewModel.RequiredImageDimensions(_RequiredImageWidth, _RequiredImageHeight);
    }
}