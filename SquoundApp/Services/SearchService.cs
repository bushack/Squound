using Shared.DataTransfer;


namespace SquoundApp.Services
{
    public class SearchService
    {
        public ProductQueryDto CurrentQuery { get; private set; } = new ProductQueryDto();
        public ProductQueryDto PreviousQuery { get; private set; } = new ProductQueryDto();


        /// <summary>
        /// Resets the current search query to its default state.
        /// </summary>
        public void ResetSearch()
        {
            CurrentQuery = new ProductQueryDto();
        }


        /// <summary>
        /// Restores the previous search query to the current search query.
        /// </summary>
        public void RestorePreviousSearch()
        {
            CurrentQuery = new ProductQueryDto
            {
                Keyword         = PreviousQuery.Keyword,
                Category        = PreviousQuery.Category,
                Subcategory     = PreviousQuery.Subcategory,
                Manufacturer    = PreviousQuery.Manufacturer,

                MinPrice        = PreviousQuery.MinPrice,
                MaxPrice        = PreviousQuery.MaxPrice,

                SortBy          = PreviousQuery.SortBy
            };
        }


        /// <summary>
        /// Saves the current search query to the previous search query.
        /// </summary>
        public void SaveCurrentSearch()
        {
            PreviousQuery = new ProductQueryDto
            {
                Keyword         = CurrentQuery.Keyword,
                Category        = CurrentQuery.Category,
                Subcategory     = CurrentQuery.Subcategory,
                Manufacturer    = CurrentQuery.Manufacturer,

                MinPrice        = CurrentQuery.MinPrice,
                MaxPrice        = CurrentQuery.MaxPrice,

                SortBy          = CurrentQuery.SortBy
            };
        }
    }
}
