using System.ComponentModel.DataAnnotations;

using Shared.Defaults;


namespace Shared.DataTransfer
{
    public class ItemDetailQueryDto : IValidatableObject
    {
        /// <summary>
        /// Gets or sets the unique identifier for the item.
        /// </summary>
        [Range(DtoDefaults.MinimumItemId, DtoDefaults.MaximumItemId, ErrorMessage = nameof(ItemId) + DtoDefaults.RangeErrorMessage)]
        public required long ItemId { get; set; }


        /// <summary>
        /// Gets or sets the required image width in pixels.
        /// </summary>
        [Range(DtoDefaults.MinimumImageWidth, DtoDefaults.MaximumImageWidth, ErrorMessage = nameof(ImageWidth) + DtoDefaults.RangeErrorMessage)]
        public required int ImageWidth { get; set; } = DtoDefaults.ImageWidth;


        /// <summary>
        /// Gets or sets the required image height in pixels.
        /// </summary>
        [Range(DtoDefaults.MinimumImageHeight, DtoDefaults.MaximumImageHeight, ErrorMessage = nameof(ImageHeight) + DtoDefaults.RangeErrorMessage)]
        public required int ImageHeight { get; set; } = DtoDefaults.ImageHeight;


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