using System.ComponentModel.DataAnnotations;


namespace SquoundApi.Models
{
    public class ProductModel
    {
        [Required]
        public required long Id { get; set; }

        [Required]
        public required string Name { get; set; }

        [Required]
        public required string Manufacturer { get; set; }

        [Required]
        public required string Description { get; set; }

        [Required]
        public required decimal Price { get; set; }

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
