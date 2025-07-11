using CommunityToolkit.Mvvm.Input;
using SquoundApp_v1.Pages;


namespace SquoundApp_v1.ViewModels
{
    public partial class CategoriesViewModel : BaseViewModel
    {
        public CategoriesViewModel()
        {
            Title = "Categories";
        }

        [RelayCommand]
        async Task GoToProductSearchAsync()
        {
            if (Shell.Current.CurrentPage.Title.Equals(nameof(ProductSearchPage)))
                return;

            await Shell.Current.GoToAsync(nameof(ProductSearchPage));
        }
    }
}