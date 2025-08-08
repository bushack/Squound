using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SquoundApi.Models
{
    /// <summary>
    /// This class maps to the ItemImages table in the database.
    /// It's purpose is to store an image related to a item.
    /// </summary>
    public class ItemImageModel
    {
        /// <summary>
        /// Defines the primary key for the ItemImageModel.
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public required long ImageId { get; set; }

        /// <summary>
        /// Defines the foreign key relationship to the ItemModel.
        /// </summary>
        [Required]
        public required long ItemId { get; set; }

        /// <summary>
        /// Defines the URL of the item image.
        /// </summary>
        [Required]
        public required string ImageUrl { get; set; }

        /// <summary>
        /// Specifies whether this image is the primary image for the item.
        /// </summary>
        public bool? IsPrimary { get; set; } = false;

        /// <summary>
        /// Defines the order in which images are displayed for the item.
        /// </summary>
        public int? SortOrder { get; set; } = 0;

        /// <summary>
        /// Defines the foreign key relationship to the ItemModel.
        /// </summary>
        public required ItemModel Item { get; set; }
    }
}
