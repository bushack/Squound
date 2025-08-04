using SquoundApp.Pages;
using SquoundApp.ViewModels;
using SquoundApp.Utilities;

namespace SquoundApp.Views;


public partial class SearchCategoriesView : ContentView
{
	public SearchCategoriesView()
	{
		InitializeComponent();

        // The ViewModel is the decision maker working in the background for the View.
        // Therefore the DataType in the file SearchCategoriesView.xaml is set to SearchCategoriesView,
        // this assists the XAML compiler in providing IntelliSense and type checking.
        // However, the actual instance of the ViewModel is provided by the ServiceLocator,
        // The instance is created in the file MauiProgram.cs with the line:
        // builder.Services.AddSingleton<CategoriesViewModel>();
        BindingContext = ServiceLocator.GetService<RefinedSearchViewModel>();
    }

    private async void OnButtonClicked(Object sender, EventArgs e)
    {
        // Prevent navigation if the current page is already RefinedSearchPage.
        // if (Shell.Current.CurrentPage.Title.Equals(nameof(RefinedSearchPage)))
        //    return;

        if (sender is Button button && button.CommandParameter is string category)
        {
            if (BindingContext is RefinedSearchViewModel viewModel)
            {
                // Set the Category property in the ViewModel to the chosen category.
                viewModel.Category = category;

                // Then navigate to the RefinedSearchPage.
                await Shell.Current.GoToAsync(nameof(RefinedSearchPage));
            }
        }
    }
}