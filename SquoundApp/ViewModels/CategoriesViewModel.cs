//using CommunityToolkit.Mvvm.Input;
//using SquoundApp.Pages;


//namespace SquoundApp.ViewModels
//{
//    public partial class CategoriesViewModel : BaseViewModel
//    {
//        public CategoriesViewModel()
//        {
//            Title = "Categories";
//        }

//        [RelayCommand]
//        async Task GoToProductSearchAsync()
//        {
//            if (Shell.Current.CurrentPage.Title.Equals(nameof(ProductSearchPage)))
//                return;

//            await Shell.Current.GoToAsync(nameof(ProductSearchPage));
//        }
//    }
//}