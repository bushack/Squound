

namespace Shared.DataTransfer
{
    public class ItemDetailDto : ItemSummaryDto
    {
        public string Manufacturer { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Material { get; set; } = string.Empty;

        public int Width { get; set; } = -1;

        public int Height { get; set; } = -1;

        public int Depth { get; set; } = -1;

        public int Quantity { get; set; } = -1;

        /// <summary>
        /// Represents an empty item detail object with default values.
        /// </summary>
        public static ItemDetailDto Empty => new();
    }
}
