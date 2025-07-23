using SquoundApp.Pages;
using SquoundApp.ViewModels;
using SquoundApp.Utilities;

namespace SquoundApp.Views;


public partial class ProductCategoriesView : ContentView
{
	public ProductCategoriesView()
	{
		InitializeComponent();

        // The ViewModel is the decision maker working in the background for the View.
        // Therefore the DataType in the file ProductCategoriesView.xaml is set to CategoriesViewModel,
        // this assists the XAML compiler in providing IntelliSense and type checking.
        // However, the actual instance of the ViewModel is provided by the ServiceLocator,
        // The instance is created in the file MauiProgram.cs with the line:
        // builder.Services.AddSingleton<CategoriesViewModel>();
        BindingContext = ServiceLocator.GetService<HeaderViewModel>();
    }

    private async void OnButtonClicked(Object sender, EventArgs e)
    {
        if (Shell.Current.CurrentPage.Title.Equals(nameof(ProductSearchPage)))
            return;

        if (sender is Button button && button.CommandParameter is string category)
        {
            await Shell.Current.GoToAsync($"{nameof(ProductSearchPage)}?category={Uri.EscapeDataString(category)}");
        }
    }
}