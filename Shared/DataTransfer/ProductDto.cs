using System.Collections.ObjectModel;


namespace Shared.DataTransfer
{
    public class ProductDto
    {
        public ObservableCollection<string> Images { get; set; } = [];

        /// <summary>
        /// ProductId is the primary key for the ProductDto.
        /// </summary>
        public long ProductId { get; set; } = -1;

        public string Name { get; set; } = string.Empty;

        public string Manufacturer { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; } = 0.0m;

        public int Quantity { get; set; } = 0;

        /// <summary>
        /// Returns the URL of the product's primary image or a default image if none are available.
        /// </summary>
        public string PrimaryImageUrl { get => Images?.FirstOrDefault() ?? "default_product.png"; }
    }
}
