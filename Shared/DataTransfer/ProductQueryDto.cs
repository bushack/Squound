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

        /// <summary>
        /// Gets a value indicating whether the current sort option is set to sort products by price in ascending order.
        /// </summary>
        //public bool IsSortOptionPriceAsc => this.SortBy == ProductSortOption.PriceAsc;
        //public bool IsSortOptionNotPriceAsc => this.SortBy != ProductSortOption.PriceAsc;
        //public bool IsSortOptionPriceDesc => this.SortBy == ProductSortOption.PriceDesc;
        //public bool IsSortOptionNotPriceDesc => this.SortBy != ProductSortOption.PriceDesc;
        //public bool IsSortOptionNameAsc => this.SortBy == ProductSortOption.NameAsc;
        //public bool IsSortOptionNotNameAsc => this.SortBy != ProductSortOption.NameAsc;
        //public bool IsSortOptionNameDesc => this.SortBy == ProductSortOption.NameDesc;
        //public bool IsSortOptionNotNameDesc => this.SortBy != ProductSortOption.NameDesc;

        /// <summary>
        /// Resets all filter properties to their default values.
        /// </summary>
        //public void ResetFilter()
        //{
        //    this.Id = null;
        //    this.Category = null;
        //    this.Manufacturer = null;
        //    this.Keyword = null;

        //    this.MinPrice = 0;
        //    this.MaxPrice = (decimal)PracticalMaximumPrice;
        //}

        /// <summary>
        /// Converts the DTO properties into a query string format for use in REST API calls.
        /// </summary>
        public string ToQueryString()
        {
            var queryString = string.Empty;

            // If the Id is provided, we are looking for one specific
            // product and therefore all other filters are irrelevant.
            if (this.Id is not null)
                return queryString += $"id={this.Id}";

            // Append the category, manufacturer, keyword, and price filters if they are set.
            if (string.IsNullOrEmpty(this.Category) is false)
                queryString += $"category={this.Category.ToLower()}&";

            if (string.IsNullOrEmpty(this.Manufacturer) is false)
                queryString += $"manufacturer={this.Manufacturer.ToLower()}&";

            if (string.IsNullOrEmpty(this.Keyword) is false)
                queryString += $"keyword={this.Keyword.ToLower()}&";

            // Append the MinPrice and MaxPrice if they are non-zero.
            if (this.MinPrice > 0)
                queryString += $"minprice={this.MinPrice}&";

            if (this.MaxPrice > 0)
                queryString += $"maxprice={this.MaxPrice}&";

            // Append the sorting and pagination parameters.
            // Note that SortBy is always set to a default value, so it does not need to be checked.
            // The PageNumber and PageSize are also always set to default values, so they do not need to be checked.
            queryString += $"sortby={this.SortBy.ToString().ToLower()}" +
                $"&pagenumber={this.PageNumber.ToString()}" +
                $"&pagesize={this.PageSize.ToString()}";

            // Remove the trailing '&' if it exists.
            return queryString.TrimEnd('&');
        }
    }
}
