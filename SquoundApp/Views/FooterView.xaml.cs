using SquoundApp.ViewModels;
using SquoundApp.Utilities;

namespace SquoundApp.Views;


public partial class FooterView : ContentView
{
	public FooterView()
	{
		InitializeComponent();

        // The ViewModel is the decision maker working in the background for the View.
        // Therefore the DataType in the file FooterView.xaml is set to FooterViewModel,
        // this assists the XAML compiler in providing IntelliSense and type checking.
        // However, the actual instance of the ViewModel is provided by the ServiceLocator,
        // The instance is created in the file MauiProgram.cs with the line:
        // builder.Services.AddSingleton<FooterViewModel>();
        BindingContext = ServiceLocator.GetService<FooterViewModel>()
            ?? throw new ArgumentNullException(nameof(FooterViewModel));
    }
}