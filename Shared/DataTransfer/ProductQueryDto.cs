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

    public class ProductQueryDto : IValidatableObject
    {
        // Limits for string properties.
        public const int MinimumStringLength = 3;
        public const int MaximumStringLength = 25;

        // Limits for pagination.
        public const int MinimumPageSize = 10;
        public const int MaximumPageSize = 100;

        // Limits for price range queries.
        public const double PracticalMinimumPrice = 0.0;
        public const double PracticalMaximumPrice = 999999.99;

        // Regular expressions for validating string content.
        private const string AlphabeticRegex = @"^[a-zA-Z\s]+$";
        private const string AlphanumericRegex = @"^[a-zA-Z0-9\s]+$";
        private const string KeywordRegex = @"^[a-zA-Z0-9\s-']+$";

        // Error messages for regular expression validation.
        private const string AlphabeticRegexErrorMessage = "Only letters and spaces are allowed";
        private const string AlphanumericRegexErrorMessage = "Only letters, numbers and spaces are allowed";
        private const string KeywordRegexErrorMessage = "Only letters, numbers, spaces, hyphens and apostrophes are allowed";

        // Error messages for string length and range validation.
        private const string StringLengthErrorMessage = " string length out of range";
        private const string RangeErrorMessage = " value out of range";

        /// <summary>
        /// Gets or sets the the method used for sorting operations.
        /// </summary>
        public ProductSortOption SortBy { get; set; } = ProductSortOption.PriceAsc;

        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
        [Range(1, long.MaxValue, ErrorMessage = nameof(ProductId) + RangeErrorMessage)]
        public long? ProductId { get; set; } = null;

        /// <summary>
        /// Gets or sets the name of the product category.
        /// </summary>
        [StringLength(MaximumStringLength, MinimumLength = MinimumStringLength, ErrorMessage = nameof(Category) + StringLengthErrorMessage)]
        [RegularExpression(AlphabeticRegex, ErrorMessage = AlphabeticRegexErrorMessage)]
        public string? Category { get; set; } = null;

        /// <summary>
        /// Gets or sets the name of the product subcategory.
        /// </summary>
        [StringLength(MaximumStringLength, MinimumLength = MinimumStringLength, ErrorMessage = nameof(Subcategory) + StringLengthErrorMessage)]
        [RegularExpression(AlphabeticRegex, ErrorMessage = AlphabeticRegexErrorMessage)]
        public string? Subcategory { get; set; } = null;

        /// <summary>
        /// Gets or sets the name of the product manufacturer.
        /// </summary>
        [StringLength(MaximumStringLength, MinimumLength = MinimumStringLength, ErrorMessage = nameof(Manufacturer) + StringLengthErrorMessage)]
        [RegularExpression(AlphanumericRegex, ErrorMessage = AlphanumericRegexErrorMessage)]
        public string? Manufacturer { get; set; } = null;

        /// <summary>
        /// Gets or sets the keyword used for filtering or searching operations.
        /// </summary>
        [StringLength(MaximumStringLength, MinimumLength = MinimumStringLength, ErrorMessage = nameof(Keyword) + StringLengthErrorMessage)]
        [RegularExpression(KeywordRegex, ErrorMessage = KeywordRegexErrorMessage)]
        public string? Keyword { get; set; } = null;

        /// <summary>
        /// Gets or sets the minimum value for queries restricted by price range.
        /// </summary>
        [Range(PracticalMinimumPrice, PracticalMaximumPrice, ErrorMessage = nameof(MinPrice) + RangeErrorMessage)]
        public decimal? MinPrice { get; set; } = null;

        /// <summary>
        /// Gets or sets the maximum value for queries restricted by price range.
        /// </summary>
        [Range(PracticalMinimumPrice, PracticalMaximumPrice, ErrorMessage = nameof(MaxPrice) + RangeErrorMessage)]
        public decimal? MaxPrice { get; set; } = null;

        /// <summary>
        /// Gets or sets the current page number for pagination.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = nameof(PageNumber) + RangeErrorMessage)]
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Gets or sets the maximum number of products to return per page.
        /// </summary>
        [Range(MinimumPageSize, MaximumPageSize, ErrorMessage = nameof(PageSize) + RangeErrorMessage)]
        public int PageSize { get; set; } = 10;


        /// <summary>
        /// Converts the DTO properties into a query string format for use in REST API calls.
        /// </summary>
        public string ToQueryString()
        {
            var queryString = string.Empty;

            // If the Id is provided, we are looking for one specific
            // product and therefore all other filters are irrelevant.
            if (this.ProductId is not null)
                return queryString += $"productid={this.ProductId}";

            // Append the category, manufacturer, keyword, and price filters if they are set.
            if (string.IsNullOrEmpty(this.Category) is false)
                queryString += $"category={Uri.EscapeDataString(this.Category.ToLower())}&";

            if (string.IsNullOrEmpty(this.Subcategory) is false)
                queryString += $"subcategory={Uri.EscapeDataString(this.Subcategory.ToLower())}&";

            if (string.IsNullOrEmpty(this.Manufacturer) is false)
                queryString += $"manufacturer={Uri.EscapeDataString(this.Manufacturer.ToLower())}&";

            if (string.IsNullOrEmpty(this.Keyword) is false)
                queryString += $"keyword={Uri.EscapeDataString(this.Keyword.ToLower())}&";

            // Append the MinPrice and/or MaxPrice if they are within the valid range.
            if (this.MinPrice is not null && this.MinPrice > 0 && this.MinPrice <= (decimal)PracticalMaximumPrice)
                queryString += $"minprice={this.MinPrice}&";

            if (this.MaxPrice is not null && this.MaxPrice > 0 && this.MaxPrice <= (decimal)PracticalMaximumPrice)
                queryString += $"maxprice={this.MaxPrice}&";

            // Append the sorting and pagination parameters.
            // Note that SortBy is always set to a default value, so it does not need to be checked.
            // The PageNumber and PageSize are also always set to default values, so they do not need to be checked.
            queryString += $"sortby={this.SortBy.ToString().ToLower()}" +
                $"&pagenumber={this.PageNumber}" +
                $"&pagesize={this.PageSize}";

            // Remove the trailing '&' if it exists.
            return queryString.TrimEnd('&');
        }


        /// <summary>
        /// Ensures the validity of the DTO properties according to the specified validation rules.
        /// This method is used by the ASP.NET Core model validation system.
        /// The [ApiController] attribute on the controller class will automatically
        /// perform this validation before the server processes the request.
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (MinPrice.HasValue && MaxPrice.HasValue && MinPrice > MaxPrice)
            {
                yield return new ValidationResult(
                    "MinPrice cannot be greater than MaxPrice.",
                    [nameof(MinPrice), nameof(MaxPrice)]);
            }
        }
    }
}