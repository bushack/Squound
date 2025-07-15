using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SquoundApi.Models
{
    public class ProductModel
    {
        /* Primary Key, Identifier */
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public required long CategoryId { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Manufacturer { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public required int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public required decimal Price { get; set; }

        public CategoryModel Category { get; set; }

        [Required]
        public required string Image0 { get; set; }

        [Required]
        public required string Image1 { get; set; }

        [Required]
        public required string Image2 { get; set; }

        [Required]
        public required string Image3 { get; set; }

        [Required]
        public required string Image4 { get; set; }

        [Required]
        public required string Image5 { get; set; }
    }
}
