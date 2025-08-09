using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SquoundApi.Models
{
    public class ItemModel
    {
        /// <summary>
        /// ItemId is the primary key for the ItemModel and is therefore a requirement.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required long ItemId { get; set; }

        /// <summary>
        /// The item must be assigned to a category and therefore a CategoryId is a requirement.
        /// </summary>
        [Required]
        public required long CategoryId { get; set; }

        /// <summary>
        /// The item must be assigned to a subcategory and therefore a SubcategoryId is a requirement.
        /// </summary>
        [Required]
        public required long SubcategoryId { get; set; }

        /// <summary>
        /// A name of the item is a requirement.
        /// </summary>
        [Required]
        public required string Name { get; set; } = null!;

        /// <summary>
        /// The item manufacturer may be unknown therefore it is a nullable string.
        /// </summary>
        public string? Manufacturer { get; set; } = null;

        /// <summary>
        /// A brief description of the item is a requirement.
        /// </summary>
        [Required]
        public required string Description { get; set; } = null!;

        /// <summary>
        /// The item material may be unknown therefore it is a nullable string.
        /// </summary>
        public string? Material { get; set; } = null;

        /// <summary>
        /// The item width may be unknown therefore it is a nullable integer.
        /// </summary>
        public int? Width { get; set; } = null;

        /// <summary>
        /// The item height may be unknown therefore it is a nullable integer.
        /// </summary>
        public int? Height { get; set; } = null;

        /// <summary>
        /// The item depth may be unknown therefore it is a nullable integer.
        /// </summary>
        public int? Depth { get; set; } = null;

        /// <summary>
        /// The item quantity is a requirement.
        /// </summary>
        [Required]
        public required int Quantity { get; set; }

        /// <summary>
        /// The item price is a requirement.
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public required decimal Price { get; set; }

        /// <summary>
        /// The item must be assigned to a category and therefore a CategoryModel is a requirement.
        /// </summary>
        [ForeignKey("CategoryId")]
        public required CategoryModel Category { get; set; }

        /// <summary>
        /// The item must be assigned to a subcategory and therefore a SubcategoryModel is a requirement.
        /// </summary>
        [ForeignKey("SubcategoryId")]
        public required SubcategoryModel Subcategory { get; set; }

        /// <summary>
        /// The item must have a list of image urls and therefore a list of ItemImageModels are a requirement.
        /// Note the while the list must exist it is valid for the list to be empty.
        /// </summary>
        public required List<ItemImageModel> Images { get; set; } = [];
    }
}
