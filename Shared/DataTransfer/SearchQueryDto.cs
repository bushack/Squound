using System.ComponentModel.DataAnnotations;


namespace Shared.DataTransfer
{
    public enum ItemSortOption
    {
        //Relevance = 0, : TODO
        PriceAsc = 1,
        PriceDesc = 2,
        NameAsc = 3,
        NameDesc = 4,
    }

    public class SearchQueryDto : IValidatableObject
    {

        /// <summary>
        /// Gets or sets the the method used for sorting operations.
        /// </summary>
        public ItemSortOption SortBy { get; set; } = ItemSortOption.PriceAsc;

        /// <summary>
        /// Gets or sets the name of the item category.
        /// </summary>
        [StringLength(Defaults.MaximumStringLength, MinimumLength = Defaults.MinimumStringLength, ErrorMessage = nameof(Category) + Defaults.StringLengthErrorMessage)]
        [RegularExpression(Defaults.AlphabeticRegex, ErrorMessage = Defaults.AlphabeticRegexErrorMessage)]
        public string? Category { get; set; } = null;

        /// <summary>
        /// Gets or sets the name of the item subcategory.
        /// </summary>
        [StringLength(Defaults.MaximumStringLength, MinimumLength = Defaults.MinimumStringLength, ErrorMessage = nameof(Subcategory) + Defaults.StringLengthErrorMessage)]
        [RegularExpression(Defaults.AlphabeticRegex, ErrorMessage = Defaults.AlphabeticRegexErrorMessage)]
        public string? Subcategory { get; set; } = null;

        /// <summary>
        /// Gets or sets the name of the item manufacturer.
        /// </summary>
        [StringLength(Defaults.MaximumStringLength, MinimumLength = Defaults.MinimumStringLength, ErrorMessage = nameof(Manufacturer) + Defaults.StringLengthErrorMessage)]
        [RegularExpression(Defaults.AlphanumericRegex, ErrorMessage = Defaults.AlphanumericRegexErrorMessage)]
        public string? Manufacturer { get; set; } = null;

        /// <summary>
        /// Gets or sets the name of the item material.
        /// </summary>
        [StringLength(Defaults.MaximumStringLength, MinimumLength = Defaults.MinimumStringLength, ErrorMessage = nameof(Material) + Defaults.StringLengthErrorMessage)]
        [RegularExpression(Defaults.AlphabeticRegex, ErrorMessage = Defaults.AlphanumericRegexErrorMessage)]
        public string? Material { get; set; } = null;

        /// <summary>
        /// Gets or sets the keyword used for filtering or searching operations.
        /// </summary>
        [StringLength(Defaults.MaximumStringLength, MinimumLength = Defaults.MinimumStringLength, ErrorMessage = nameof(Keyword) + Defaults.StringLengthErrorMessage)]
        [RegularExpression(Defaults.KeywordRegex, ErrorMessage = Defaults.KeywordRegexErrorMessage)]
        public string? Keyword { get; set; } = null;

        /// <summary>
        /// Gets or sets the minimum value for queries restricted by price range.
        /// </summary>
        [Range(Defaults.PracticalMinimumPrice, Defaults.PracticalMaximumPrice, ErrorMessage = nameof(MinPrice) + Defaults.RangeErrorMessage)]
        public decimal? MinPrice { get; set; } = null;

        /// <summary>
        /// Gets or sets the maximum value for queries restricted by price range.
        /// </summary>
        [Range(Defaults.PracticalMinimumPrice, Defaults.PracticalMaximumPrice, ErrorMessage = nameof(MaxPrice) + Defaults.RangeErrorMessage)]
        public decimal? MaxPrice { get; set; } = null;

        /// <summary>
        /// Gets or sets the current page number for pagination.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = nameof(PageNumber) + Defaults.RangeErrorMessage)]
        public int PageNumber { get; set; } = Defaults.PageNumber;

        /// <summary>
        /// Gets or sets the maximum number of items to return per page.
        /// </summary>
        [Range(Defaults.MinimumPageSize, Defaults.MaximumPageSize, ErrorMessage = nameof(PageSize) + Defaults.RangeErrorMessage)]
        public int PageSize { get; set; } = Defaults.PageSize;

        /// <summary>
        /// Gets or sets the required image width in pixels.
        /// </summary>
        [Range(Defaults.MinimumImageWidth, Defaults.MaximumImageWidth, ErrorMessage = nameof(ImageWidth) + Defaults.RangeErrorMessage)]
        public int ImageWidth { get; set; } = Defaults.ImageWidth;

        /// <summary>
        /// Gets or sets the required image height in pixels.
        /// </summary>
        [Range(Defaults.MinimumImageHeight, Defaults.MaximumImageHeight, ErrorMessage = nameof(ImageHeight) + Defaults.RangeErrorMessage)]
        public int ImageHeight { get; set; } = Defaults.ImageHeight;


        /// <summary>
        /// Returns the query section of a URL that can be submitted to a REST API endpoint.
        /// </summary>
        public string AsQueryString()
        {
            var queryString = string.Empty;

            // Append the category, subcategory, manufacturer, material, keyword, and price filters if they are set.
            if (Category is not null)
                queryString += $"category={Uri.EscapeDataString(this.Category.ToLower())}&";

            if (Subcategory is not null)
                queryString += $"subcategory={Uri.EscapeDataString(this.Subcategory.ToLower())}&";

            if (string.IsNullOrEmpty(this.Manufacturer) is false)
                queryString += $"manufacturer={Uri.EscapeDataString(this.Manufacturer.ToLower())}&";

            if (string.IsNullOrEmpty(this.Material) is false)
                queryString += $"material={Uri.EscapeDataString(this.Material.ToLower())}&";

            if (string.IsNullOrEmpty(this.Keyword) is false)
                queryString += $"keyword={Uri.EscapeDataString(this.Keyword.ToLower())}&";

            // Append the MinPrice and/or MaxPrice if they are within the valid range.
            if (this.MinPrice is not null && this.MinPrice > 0 && this.MinPrice <= (decimal)Defaults.PracticalMaximumPrice)
                queryString += $"minprice={this.MinPrice}&";

            if (this.MaxPrice is not null && this.MaxPrice > 0 && this.MaxPrice <= (decimal)Defaults.PracticalMaximumPrice)
                queryString += $"maxprice={this.MaxPrice}&";

            // Append the sorting and pagination parameters.
            // Note that SortBy is always set to a default value, so it does not need to be checked.
            // The PageNumber and PageSize are also always set to default values, so they do not need to be checked.
            queryString += $"sortby={this.SortBy.ToString().ToLower()}" +
                $"&pagenumber={this.PageNumber}" +
                $"&pagesize={this.PageSize}" +
                $"&imagewidth={this.ImageWidth}" +
                $"&imageheight={this.ImageHeight}";

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