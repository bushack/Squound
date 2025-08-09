using System.Collections.ObjectModel;

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
        /// <returns><see cref="ItemModel"/> Data Transfer Object (DTO) containing summary information about a single database item.</returns>
        public ItemSummaryDto CreateItemSummaryDto(ItemModel model)
        {
            return new ItemSummaryDto()
            {
                // Identifier
                ItemId  = model.ItemId,

                // Strings
                Name    = model.Name,

                // Decimals
                Price   = model.Price,

                // Images
                ThumbnailImageUrls = [.. model.Images.Select(image => image.ImageUrl)]
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
        public ItemDetailDto CreateItemDetailDto(ItemModel model)
        {
            return new ItemDetailDto()
            {
                // Identifier
                ItemId          = model.ItemId,

                // Strings
                Name            = model.Name,
                Manufacturer    = model.Manufacturer ?? string.Empty,   // Nullable!
                Description     = model.Description,
                Material        = model.Material ?? string.Empty,       // Nullable!

                // Decimals
                Price           = model.Price,

                // Integers
                Width           = model.Width ?? -1,                    // Nullable!
                Height          = model.Height ?? -1,                   // Nullable!
                Depth           = model.Depth ?? -1,                    // Nullable!
                Quantity        = model.Quantity,

                // Images
                ThumbnailImageUrls = [],                                // Unused by detail page.
                LargeImageUrls = [.. model.Images.Select(image => image.ImageUrl)]
            };
        }
    }
}
