

using System.ComponentModel.DataAnnotations;

namespace Shared.DataTransfer
{
    public class ProductQueryDto
    {
        private decimal minPrice = 0;
        private decimal maxPrice = decimal.MaxValue;

        public string? SortBy { get; set; }

        public string? Category { get; set; }

        public string? Manufacturer { get; set; }

        public decimal MinPrice
        {
            get;

            set
            {
                minPrice = value;

                if (minPrice < 0) minPrice = 0;

                if (minPrice > maxPrice) minPrice = maxPrice;
            }
        }

        public decimal MaxPrice
        {
            get;

            set
            {
                maxPrice = value;

                if (maxPrice > decimal.MaxValue) maxPrice = decimal.MaxValue;

                if (maxPrice < minPrice) maxPrice = minPrice;
            }
        }

        /// <summary>
        /// Gets or sets the current page number for pagination.
        /// </summary>
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be at least 1")]
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Gets or sets the maximum number of products to return per page.
        /// </summary>
        [Range(10, 100, ErrorMessage = "Page size must be between 10 and 100")]
        public int PageSize { get; set; } = 10;
    }
}
