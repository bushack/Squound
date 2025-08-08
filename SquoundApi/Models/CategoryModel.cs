using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SquoundApi.Models
{
    public class CategoryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long CategoryId { get; set; }

        [Required]
        public required string Name { get; set; } = null!;

        public ICollection<ItemModel> Items { get; set; } = [];

        public ICollection<SubcategoryModel> Subcategories { get; set; } = [];
    }
}
