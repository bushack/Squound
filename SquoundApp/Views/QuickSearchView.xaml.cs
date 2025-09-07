using SquoundApp.Services;
using SquoundApp.ViewModels;


namespace SquoundApp.Views;

public partial class QuickSearchView : ContentView
{
    private bool _HasLoaded = false;


    public QuickSearchView()
	{
		InitializeComponent();

        this.Loaded += OnLoaded;

        // The ViewModel is the decision maker working in the background for the View.
        // Therefore the DataType in the file QuickSearchView.xaml is set to QuickSearchView,
        // this assists the XAML compiler in providing IntelliSense and type checking.
        // However, the actual instance of the ViewModel is provided by the ServiceLocator,
        // The instance is created in the file MauiProgram.cs with the line:
        // builder.Services.AddTransient<CategoriesViewModel>();
        BindingContext = ServiceLocator.GetService<QuickSearchViewModel>()
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

        if (BindingContext is QuickSearchViewModel viewModel)
        {
            await viewModel.InitAsync();
        }
    }
}