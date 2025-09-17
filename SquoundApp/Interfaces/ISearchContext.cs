using Shared.DataTransfer;


namespace SquoundApp.Interfaces
{
    public interface ISearchContext
    {
        /// <summary>
        /// Get or set the unique item identifier.
        /// </summary>
        long? ItemId { get; set; }
        CategoryDto? Category { get; set; }
        SubcategoryDto? Subcategory { get; set; }
        string? Manufacturer { get; set; }
        string? Material { get; set; }
        string? Keyword { get; set; }
        string? MinPrice { get; set; }
        string? MaxPrice { get; set; }
        int PageNumber { get; set; }
        int PageSize { get; set; }
        int RequiredImageWidth { get; set; }
        int RequiredImageHeight { get; set; }
        ItemSortOption SortBy { get; set; }
        bool HasChanged { get; }
        bool HasNotChanged { get; }


        /// <summary>
        /// Saves all changes to the internal state. To be called following a successful response from the API.
        /// </summary>
        public void SaveChanges();


        /// <summary>
        /// Discards all changes to the internal state, reverting the service back to the last saved state.
        /// </summary>
        public void CancelChanges();


        /// <summary>
        /// Discards all changes to the internal state, reverting the service back to it's default state.
        /// </summary>
        public void ResetChanges(CategoryDto? defaultCategory = null);


        public void IncrementPageNumber();


        public void DecrementPageNumber();


        public SearchQueryDto AsSearchQueryDto();


        public ItemDetailQueryDto AsItemDetailQueryDto();


        public string BuildItemSummaryUrlQueryString();


        public string BuildItemDetailUrlQueryString();
    }
}