using SquoundApp.Pages;
using SquoundApp.Utilities;
using SquoundApp.ViewModels;

namespace SquoundApp.Views;


public partial class ProductFilterView : ContentView
{
	public ProductFilterView()
	{
		InitializeComponent();

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

    //private void OnCheckedChanged(object sender, CheckedChangedEventArgs e)
    //{
    //    if ((BindingContext is ProductFilterViewModel viewModel) && (sender is RadioButton radioButton))
    //    {
    //        switch (radioButton.Content)
    //        {
    //            case "NameAsc":
    //                viewModel.SortOption = Shared.DataTransfer.ProductSortOption.NameAsc;
    //                break;

    //            case "NameDesc":
    //                viewModel.SortOption = Shared.DataTransfer.ProductSortOption.NameDesc;
    //                break;

    //            case "PriceAsc":
    //                viewModel.SortOption = Shared.DataTransfer.ProductSortOption.PriceAsc;
    //                break;

    //            case "PriceDesc":
    //                viewModel.SortOption = Shared.DataTransfer.ProductSortOption.PriceDesc;
    //                break;

    //            default:
    //                viewModel.SortOption = Shared.DataTransfer.ProductSortOption.PriceAsc;
    //                break;
    //        }
    //    }
    //}
}