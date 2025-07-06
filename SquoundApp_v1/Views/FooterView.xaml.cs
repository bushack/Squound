using SquoundApp_v1.ViewModels;
using SquoundApp_v1.Utilities;

namespace SquoundApp_v1.Views;

public partial class FooterView : ContentView
{
	public FooterView()
	{
		InitializeComponent();

        BindingContext = ServiceLocator.GetService<FooterViewModel>();
    }
}