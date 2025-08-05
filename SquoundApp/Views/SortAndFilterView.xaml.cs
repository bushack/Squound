using SquoundApp.Utilities;
using SquoundApp.ViewModels;

namespace SquoundApp.Views;


public partial class SortAndFilterView : ContentView
{
	public SortAndFilterView()
	{
		InitializeComponent();

        BindingContext = ServiceLocator.GetService<SortAndFilterViewModel>()
			?? throw new ArgumentNullException(nameof(SortAndFilterViewModel));
    }
}