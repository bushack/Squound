using System.ComponentModel.DataAnnotations;


namespace Shared.DataTransfer
{
    public enum ProductSortOption
    {
        //Relevance = 0, : TODO
        PriceAsc = 1,
        PriceDesc = 2,
        NameAsc = 3,
        NameDesc = 4,
    }

    public class ProductQueryDto
    {
        /// <summary>
        /// A sensible and pracitcal upper limit for maximum price.
        /// </summary>
        public const double PracticalMaximumPrice = 999999.99;

        /// <summary>
        /// Gets or sets the the method used for sorting operations.
        /// </summary>
        public ProductSortOption SortBy { get; set; } = ProductSortOption.PriceAsc;

        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
        public long? Id { get; set; } = null;

        /// <summary>
        /// Gets or sets the name of the product category.
        /// </summary>
        public string? Category { get; set; } = null;

        /// <summary>
        /// Gets or sets the name of the product manufacturer.
        /// </summary>
        public string? Manufacturer { get; set; } = null;

        /// <summary>
        /// Gets or sets the keyword used for filtering or searching operations.
        /// </summary>
        public string? Keyword { get; set; } = null;

        /// <summary>
        /// Gets or sets the minimum value for queries restricted by price range.
        /// </summary>
        [Range(0, PracticalMaximumPrice)]
        public decimal MinPrice { get; set; } = 0;

        /// <summary>
        /// Gets or sets the maximum value for queries restricted by price range.
        /// </summary>
        [Range(0, PracticalMaximumPrice)]
        public decimal MaxPrice { get; set; } = (decimal)PracticalMaximumPrice;

        /// <summary>
        /// Gets or sets the current page number for pagination.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be at least 1")]
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Gets or sets the maximum number of products to return per page.
        /// </summary>
        [Range(10, 100, ErrorMessage = "Page size must be between 10 and 100")]
        public int PageSize { get; set; } = 10;
    }
}
