using System.Collections.ObjectModel;


namespace Shared.DataTransfer
{
    public class ItemSummaryDto
    {
        /// <summary>
        /// ItemId is the primary key for the ItemDto.
        /// </summary>
        public long ItemId { get; set; } = -1;

        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; } = 0.0m;

        public IReadOnlyList<string> ThumbnailImageUrls { get; set; } = [];

        /// <summary>
        /// Returns the URL of the item's primary image or a default image if none are available.
        /// </summary>
        public string PrimaryThumbnailUrl => ThumbnailImageUrls?.FirstOrDefault() ?? "default_product.png";
    }
}
