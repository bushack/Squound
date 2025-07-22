using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Shared.DataTransfer;


namespace SquoundApp.ViewModels
{
    public partial class ProductFilterViewModel : BaseViewModel
    {
        [ObservableProperty]
        private bool isSortOptionNameAscending = false;

        [ObservableProperty]
        private bool isSortOptionNameDescending = false;

        [ObservableProperty]
        private bool isSortOptionPriceAscending = true;

        [ObservableProperty]
        private bool isSortOptionPriceDescending = false;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ProductFilterViewModel()
        {
            Title = "Product Filter";
        }

        /// <summary>
        /// OnChanged methods are called AFTER the value has been set.
        /// </summary>
        /// <param name="value">The value applied to isSortOptionNameAscending.</param>
        partial void OnIsSortOptionNameAscendingChanged(bool value)
        {
            if (value)
            {
                //isSortOptionNameDescending = false;
                //isSortOptionPriceAscending = false;
                //isSortOptionPriceDescending = false;
            }
        }

        /// <summary>
        /// OnChanged methods are called AFTER the value has been set.
        /// </summary>
        /// <param name="value">The value applied to isSortOptionNameDescending.</param>
        partial void OnIsSortOptionNameDescendingChanged(bool value)
        {
            if (value)
            {
                //isSortOptionNameAscending = false;
                //isSortOptionPriceAscending = false;
                //isSortOptionPriceDescending = false;
            }
        }

        /// <summary>
        /// OnChanged methods are called AFTER the value has been set.
        /// </summary>
        /// <param name="value">The value applied to isSortOptionPriceAscending.</param>
        partial void OnIsSortOptionPriceAscendingChanged(bool value)
        {
            if (value)
            {
                //isSortOptionNameAscending = false;
                //isSortOptionNameDescending = false;
                //isSortOptionPriceDescending = false;
            }
        }

        /// <summary>
        /// OnChanged methods are called AFTER the value has been set.
        /// </summary>
        /// <param name="value">The value applied to isSortOptionPriceDescending.</param>
        partial void OnIsSortOptionPriceDescendingChanged(bool value)
        {
            if (value)
            {
                //isSortOptionNameAscending = false;
                //isSortOptionNameDescending = false;
                //isSortOptionPriceAscending = false;
            }
        }

        [RelayCommand]
        private void SetSortOptionAsNameAscending()
        {
            IsSortOptionNameAscending = true;
            IsSortOptionNameDescending = false;
            IsSortOptionPriceAscending = false;
            IsSortOptionPriceDescending = false;
        }
        [RelayCommand]
        private void SetSortOptionAsNameDescending()
        {
            IsSortOptionNameAscending = false;
            IsSortOptionNameDescending = true;
            IsSortOptionPriceAscending = false;
            IsSortOptionPriceDescending = false;
        }

        [RelayCommand]
        private void SetSortOptionAsPriceAscending()
        {
            IsSortOptionNameAscending = false;
            IsSortOptionNameDescending = false;
            IsSortOptionPriceAscending = true;
            IsSortOptionPriceDescending = false;
        }
        [RelayCommand]
        private void SetSortOptionAsPriceDescending()
        {
            IsSortOptionNameAscending = false;
            IsSortOptionNameDescending = false;
            IsSortOptionPriceAscending = false;
            IsSortOptionPriceDescending = true;
        }
    }
}
