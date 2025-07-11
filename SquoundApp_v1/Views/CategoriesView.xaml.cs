using SquoundApp_v1.ViewModels;
using SquoundApp_v1.Utilities;

namespace SquoundApp_v1.Views;


public partial class CategoriesView : ContentView
{
	public CategoriesView()
	{
		InitializeComponent();

        // The ViewModel is the decision maker working in the background for the View.
        // Therefore the DataType in the file CategoriesView.xaml is set to CategoriesViewModel,
        // this assists the XAML compiler in providing IntelliSense and type checking.
        // However, the actual instance of the ViewModel is provided by the ServiceLocator,
        // The instance is created in the file MauiProgram.cs with the line:
        // builder.Services.AddSingleton<CategoriesViewModel>();
        BindingContext = ServiceLocator.GetService<CategoriesViewModel>();
    }
}