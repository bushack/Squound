using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public IReadOnlyList<string> LargeImageUrls { get; set; } = [];

        /// <summary>
        /// Returns the URL of the item's primary image or a default image if none are available.
        /// </summary>
        public string PrimaryImageUrl => LargeImageUrls?.FirstOrDefault() ?? "default_product.png";
    }
}
