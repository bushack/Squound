using SquoundApp.Pages;
using SquoundApp.Services;
using SquoundApp.ViewModels;
using SquoundApp.Utilities;


namespace SquoundApp.Views;

public partial class SearchCategoriesView : ContentView
{
    private bool hasLoaded = false;

    public SearchCategoriesView()
	{
		InitializeComponent();

        this.Loaded += OnLoaded;

        // The ViewModel is the decision maker working in the background for the View.
        // Therefore the DataType in the file SearchCategoriesView.xaml is set to SearchCategoriesView,
        // this assists the XAML compiler in providing IntelliSense and type checking.
        // However, the actual instance of the ViewModel is provided by the ServiceLocator,
        // The instance is created in the file MauiProgram.cs with the line:
        // builder.Services.AddSingleton<CategoriesViewModel>();
        BindingContext = ServiceLocator.GetService<SearchCategoriesViewModel>()
            ?? throw new ArgumentNullException(nameof(SearchCategoriesViewModel));
    }


    //
    private async void OnLoaded(object? sender, EventArgs e)
    {
        // Unsubscribe from the Loaded event to ensure that the data is only loaded one time
        // when the view is first displayed. This also has the benefit of preventing memory leaks
        this.Loaded -= OnLoaded;

        // Prevent multiple reloads of the data.
        // We only want to load the data once when the view is first displayed.
        if (hasLoaded)
            return;

        hasLoaded = true;

        if (BindingContext is SearchCategoriesViewModel viewModel)
        {
            await viewModel.InitAsync();
        }
    }


    private async void OnButtonClicked(Object sender, EventArgs e)
    {
        // Prevent navigation if the current page is already RefinedSearchPage.
        // if (Shell.Current.CurrentPage.Title.Equals(nameof(RefinedSearchPage)))
        //    return;

        if (sender is Button button && button.CommandParameter is string category)
        {
            if (BindingContext is SearchCategoriesViewModel viewModel)
            {
                // Set the Category property in the ViewModel to the chosen category.
                //viewModel.Category = category;

                // Then navigate to the RefinedSearchPage.
                //await Shell.Current.GoToAsync(nameof(RefinedSearchPage));

                //var navService = ServiceLocator.GetService<NavigationService>();
                //await navService.GoToAsync(nameof(RefinedSearchPage));
            }
        }
    }
}