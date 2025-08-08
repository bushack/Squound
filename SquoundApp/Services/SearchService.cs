using Shared.DataTransfer;


namespace SquoundApp.Services
{
    public class SearchService
    {
        public SearchQueryDto CurrentQuery { get; private set; } = new SearchQueryDto();
        public SearchQueryDto PreviousQuery { get; private set; } = new SearchQueryDto();


        /// <summary>
        /// Resets the current search query to its default state.
        /// </summary>
        public void ResetSearch()
        {
            CurrentQuery = new SearchQueryDto();
        }


        /// <summary>
        /// Restores the previous search query to the current search query.
        /// </summary>
        public void RestorePreviousSearch()
        {
            CurrentQuery = new SearchQueryDto
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
            PreviousQuery = new SearchQueryDto
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
