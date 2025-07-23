using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using SquoundApp.Pages;


namespace SquoundApp.ViewModels
{
    public partial class HeaderViewModel : BaseViewModel
    {
        // Variables involved in showing and hiding the sort and filter menus.
        [ObservableProperty]
        public bool isTitleLabelVisible = true;

        [ObservableProperty]
        public bool isSortButtonActive = true;

        [ObservableProperty]
        public bool isFilterButtonActive = true;

        [ObservableProperty]
        public bool isSortMenuActive = false;

        [ObservableProperty]
        public bool isFilterMenuActive = false;


        // Variables for sort options.
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsSortOptionNotNameAscending))]
        public bool isSortOptionNameAscending = false;
        public bool IsSortOptionNotNameAscending => !IsSortOptionNameAscending;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsSortOptionNotNameDescending))]
        public bool isSortOptionNameDescending = false;
        public bool IsSortOptionNotNameDescending => !IsSortOptionNameDescending;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsSortOptionNotPriceAscending))]
        public bool isSortOptionPriceAscending = true;
        public bool IsSortOptionNotPriceAscending => !IsSortOptionPriceAscending;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsSortOptionNotPriceDescending))]
        public bool isSortOptionPriceDescending = false;
        public bool IsSortOptionNotPriceDescending => !IsSortOptionPriceDescending;


        // Variables for filter options.
        [ObservableProperty]
        public string filterKeyword = "";

        [ObservableProperty]
        public string filterMinimumPrice = "";

        [ObservableProperty]
        public string filterMaximumPrice = "";


        /// <summary>
        /// Default constructor.
        /// </summary>
        public HeaderViewModel()
        {
        }

        /// <summary>
        /// OnChanged methods are called AFTER the value has been set.
        /// </summary>
        /// <param name="value">The value applied to isSortOptionNameAscending.</param>
        partial void OnIsSortOptionNameAscendingChanged(bool value)
        {
            if (value)
            {
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

        [RelayCommand]
        private void OnSortButton()
        {
            Title = "Sort Options";

            IsTitleLabelVisible = true;

            IsSortButtonActive = false;
            IsFilterButtonActive = false;

            IsSortMenuActive = true;
            IsFilterMenuActive = false;
        }

        [RelayCommand]
        private void OnFilterButton()
        {
            Title = "Filter Options";

            IsTitleLabelVisible = true;

            IsSortButtonActive = false;
            IsFilterButtonActive = false;

            IsSortMenuActive = false;
            IsFilterMenuActive = true;
        }

        [RelayCommand]
        private void OnApplySort()
        {
            IsTitleLabelVisible = false;

            IsSortButtonActive = true;
            IsFilterButtonActive = true;

            IsSortMenuActive = false;
            IsFilterMenuActive = false;
        }

        [RelayCommand]
        private void OnCancelSort()
        {
            IsTitleLabelVisible = false;

            IsSortButtonActive = true;
            IsFilterButtonActive = true;

            IsSortMenuActive = false;
            IsFilterMenuActive = false;
        }

        [RelayCommand]
        private void OnApplyFilter()
        {
            IsTitleLabelVisible = false;

            IsSortButtonActive = true;
            IsFilterButtonActive = true;

            IsSortMenuActive = false;
            IsFilterMenuActive = false;
        }

        [RelayCommand]
        private void OnResetFilter()
        {
            FilterKeyword = "";
            FilterMinimumPrice = "";
            FilterMaximumPrice = "";
        }

        [RelayCommand]
        private void OnCancelFilter()
        {
            IsTitleLabelVisible = false;

            IsSortButtonActive = true;
            IsFilterButtonActive = true;

            IsSortMenuActive = false;
            IsFilterMenuActive = false;
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
