using SquoundApp.Pages;
using SquoundApp.Utilities;
using SquoundApp.ViewModels;

namespace SquoundApp.Views;


public partial class SearchPreferencesView : ContentView
{
	public SearchPreferencesView()
	{
		InitializeComponent();

        BindingContext = ServiceLocator.GetService<RefinedSearchViewModel>();
    }

    //private async void OnButtonClicked(Object sender, EventArgs e)
    //{
    //    if (Shell.Current.CurrentPage.Title.Equals(nameof(SearchPage)))
    //        return;

    //    if (sender is Button button && button.CommandParameter is string category)
    //    {
    //        await Shell.Current.GoToAsync($"{nameof(SearchPage)}?category={Uri.EscapeDataString(category)}");
    //    }
    //}
}