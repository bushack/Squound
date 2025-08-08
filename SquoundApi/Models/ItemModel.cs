using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SquoundApi.Models
{
    public class ItemModel
    {
        /// <summary>
        /// ItemId is the primary key for the ItemModel.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required long ItemId { get; set; }

        [Required]
        public required long CategoryId { get; set; }

        [Required]
        public required long SubcategoryId { get; set; }

        [Required]
        public required string Name { get; set; } = null!;

        /// <summary>
        /// Manufacturer may be unknown therefore it is a nullable string.
        /// </summary>
        public string? Manufacturer { get; set; } = null;

        [Required]
        public required string Description { get; set; } = null!;

        [Required]
        public required int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public required decimal Price { get; set; }

        [ForeignKey("CategoryId")]
        public required CategoryModel Category { get; set; }

        [ForeignKey("SubcategoryId")]
        public required SubcategoryModel Subcategory { get; set; }

        public required List<ItemImageModel> Images { get; set; } = [];
    }
}
