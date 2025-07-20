using Shared.DataTransfer;


namespace SquoundApp.ViewModels
{
    public partial class ProductFilterViewModel : BaseViewModel
    {
        private ProductQueryDto query = new();

        public ProductFilterViewModel()
        {
            Title = "Product Filter";
        }

        public ProductSortOption SortOption { get { return query.SortBy; } set { query.SortBy = value; } }

        public bool IsSortOptionNameAsc { get { return query.SortBy == ProductSortOption.NameAsc; } }
        public bool IsSortOptionNameDesc { get { return query.SortBy == ProductSortOption.NameDesc; } }
        public bool IsSortOptionPriceAsc { get { return query.SortBy == ProductSortOption.PriceAsc; } }
        public bool IsSortOptionPriceDesc { get { return SortOption == ProductSortOption.PriceDesc; } }
    }
}
