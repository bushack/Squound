using SquoundApp.Services;
using SquoundApp.ViewModels;


namespace SquoundApp.Views;

public partial class SortAndFilterView : ContentView
{
    private bool _HasLoaded = false;

    public SortAndFilterView()
	{
		InitializeComponent();

        this.Loaded += OnLoaded;

        BindingContext = ServiceLocator.GetService<SortAndFilterViewModel>()
            ?? throw new ArgumentNullException(nameof(BindingContext));
    }


    //
    private async void OnLoaded(object? sender, EventArgs e)
    {
        // Unsubscribe from the Loaded event to ensure that the data is only loaded one time
        // when the view is first displayed. This also has the benefit of preventing memory leaks.
        this.Loaded -= OnLoaded;

        // Prevent multiple reloads of the data.
        // We only want to load the data once when the view is first displayed.
        if (_HasLoaded)
            return;

        _HasLoaded = true;

        if (BindingContext is SortAndFilterViewModel viewModel)
        {
            await viewModel.InitAsync();
        }
    }
}