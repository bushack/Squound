using SquoundApp.Utilities;
using SquoundApp.ViewModels;


namespace SquoundApp.Views;

public partial class SearchHeadingView : ContentView
{
	public SearchHeadingView()
	{
		InitializeComponent();

        BindingContext = ServiceLocator.GetService<RefinedSearchViewModel>()
			?? throw new ArgumentNullException(nameof(RefinedSearchViewModel));
    }
}