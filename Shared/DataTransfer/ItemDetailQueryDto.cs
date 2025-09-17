using System.ComponentModel.DataAnnotations;


namespace Shared.DataTransfer
{
    public class ItemDetailQueryDto : IValidatableObject
    {
        /// <summary>
        /// Gets or sets the unique identifier for the item.
        /// </summary>
        [Range(Defaults.MinimumItemId, Defaults.MaximumItemId, ErrorMessage = nameof(ItemId) + Defaults.RangeErrorMessage)]
        public required long ItemId { get; set; }


        /// <summary>
        /// Gets or sets the required image width in pixels.
        /// </summary>
        [Range(Defaults.MinimumImageWidth, Defaults.MaximumImageWidth, ErrorMessage = nameof(ImageWidth) + Defaults.RangeErrorMessage)]
        public required int ImageWidth { get; set; } = Defaults.ImageWidth;


        /// <summary>
        /// Gets or sets the required image height in pixels.
        /// </summary>
        [Range(Defaults.MinimumImageHeight, Defaults.MaximumImageHeight, ErrorMessage = nameof(ImageHeight) + Defaults.RangeErrorMessage)]
        public required int ImageHeight { get; set; } = Defaults.ImageHeight;


        /// <summary>
        /// Returns the query section of a URL that can be submitted to a REST API endpoint.
        /// </summary>
        public string AsQueryString()
        {
            var queryString = string.Empty;

            queryString += $"itemid={this.ItemId}";
            queryString += $"&imagewidth={this.ImageWidth}";
            queryString += $"&imageheight={this.ImageHeight}";

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
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Object is in valid state, no chance of failure.
            return new List<ValidationResult>();
        }
    }
}