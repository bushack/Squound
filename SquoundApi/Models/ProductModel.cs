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
        public required int Price { get; set; }
    }
}
