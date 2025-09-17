

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

        public IReadOnlyList<string> ImageUrls { get; set; } = [];

        /// <summary>
        /// Returns the URL of the item's primary image.
        /// If image not found, use the built-in default image for items.
        /// </summary>
        public string PrimaryImageUrl => ImageUrls?.FirstOrDefault() ?? "default_item.png";
    }
}
