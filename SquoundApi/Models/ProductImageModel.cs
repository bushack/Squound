using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SquoundApi.Models
{
    /// <summary>
    /// This class maps to the ProductImages table in the database.
    /// It's purpose is to store an image related to a product.
    /// </summary>
    public class ProductImageModel
    {
        /// <summary>
        /// Defines the primary key for the ProductImageModel.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required long ImageId { get; set; }

        /// <summary>
        /// Defines the foreign key relationship to the ProductModel.
        /// </summary>
        [Required]
        public required long ProductId { get; set; }

        /// <summary>
        /// Defines the URL of the product image.
        /// </summary>
        [Required]
        public required string ImageUrl { get; set; }

        /// <summary>
        /// Specifies whether this image is the primary image for the product.
        /// </summary>
        public bool? IsPrimary { get; set; } = false;

        /// <summary>
        /// Defines the order in which images are displayed for the product.
        /// </summary>
        public int? SortOrder { get; set; } = 0;

        /// <summary>
        /// Defines the foreign key relationship to the ProductModel.
        /// </summary>
        public required ProductModel Product { get; set; }
    }
}
