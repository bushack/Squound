using Microsoft.IdentityModel.Tokens;

using SquoundApi.Interfaces;
using SquoundApi.Models;

using Shared.DataTransfer;


namespace SquoundApi.Factories
{
    public class DtoFactory : IDtoFactory
    {
        /// <summary>
        /// Converts an <see cref="ItemModel"/> to a <see cref="ItemSummaryDto"/>.
        /// The <see cref="ItemModel"/> is an internal API data model that directly maps to the Items table in the database.
        /// The <see cref="ItemSummaryDto"/> is a Data Transfer Object (DTO) that is used to transfer data between the server and
        /// the client application. As such, it contains only the necessary data for the client application.
        /// </summary>
        /// <param name="model">ItemModel instance to use as the source data.</param>
        /// <param name="requiredImageSize">Image size required by the client application.</param>
        /// <returns><see cref="ItemModel"/> Data Transfer Object (DTO) containing summary information about a single database item.</returns>
        public ItemSummaryDto CreateItemSummaryDto(ItemModel model, string requiredImageSize)
        {
            return new ItemSummaryDto()
            {
                ItemId      = model.ItemId,
                Name        = model.Name,
                Price       = model.Price,
                ImageUrls   = GetImageUrls(model, requiredImageSize)
            };
        }


        /// <summary>
        /// Converts an <see cref="ItemModel"/> to a <see cref="ItemDetailDto"/>.
        /// The <see cref="ItemModel"/> is an internal API data model that directly maps to the Items table in the database.
        /// The <see cref="ItemDetailDto"/> is a Data Transfer Object (DTO) that is used to transfer data between the server and
        /// the client application. As such, it contains only the necessary data for the client application.
        /// </summary>
        /// <param name="model">ItemModel instance to use as the source data.</param>
        /// <returns><see cref="ItemDetailDto"/> Data Transfer Object (DTO) containing detailed information about a single database item.</returns>
        public ItemDetailDto CreateItemDetailDto(ItemModel model, string requiredImageSize)
        {
            return new ItemDetailDto()
            {
                ItemId          = model.ItemId,
                Name            = model.Name,
                Manufacturer    = model.Manufacturer ?? string.Empty,   // Nullable!
                Description     = model.Description,
                Material        = model.Material ?? string.Empty,       // Nullable!
                Price           = model.Price,
                Width           = model.Width ?? -1,                    // Nullable!
                Height          = model.Height ?? -1,                   // Nullable!
                Depth           = model.Depth ?? -1,                    // Nullable!
                Quantity        = model.Quantity,
                ImageUrls       = GetImageUrls(model, requiredImageSize) 
            };
        }



        /// <summary>
        /// Returns a list of image URLs from the ItemModel that match the required image size.
        /// Image filenames are expected to be in the format: {ItemId}_{ImageNumber}_{ImageSize}.ext
        /// Example: 12345_1_large.jpg
        /// </summary>
        /// <param name="model">ItemModel instance to use as the source data.</param>
        /// <param name="requiredImageSize">Image size required by the client application.</param>
        /// <returns></returns>
        private static IReadOnlyList<string> GetImageUrls(ItemModel model, string requiredImageSize)
        {
            if (model.Images.IsNullOrEmpty())
                return [];

            // Attempt to find images that match the required size.
            var matchingImages = model.Images.Where(image =>
            {
                // Remove filename extension and divide into segments.
                var filename = Path.GetFileNameWithoutExtension(image.ImageUrl);
                var segments = filename.Split('_');

                return segments.Length >= 3 && segments.Last().Equals(requiredImageSize, StringComparison.OrdinalIgnoreCase);
            })
            .ToList();

            // Fallback option.
            // Find images of largest size (currently 'large').
            if (matchingImages.IsNullOrEmpty())
            {
                matchingImages = model.Images.Where(image =>
                {
                    // Remove filename extension and divide into segments.
                    var filename = Path.GetFileNameWithoutExtension(image.ImageUrl);
                    var segments = filename.Split('_');

                    return segments.Length >= 3 && segments.Last().Equals("large", StringComparison.OrdinalIgnoreCase);
                })
                .ToList();
            }

            return [.. matchingImages.Select(image => image.ImageUrl)];
        }
    }
}
