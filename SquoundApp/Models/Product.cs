using System.ComponentModel.DataAnnotations;


namespace SquoundApp.Models
{
    public class Product
    {
        [Required]
        public required int Id { get; set; }

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
