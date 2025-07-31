using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SquoundApi.Models
{
    public class SubcategoryModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SubcategoryId { get; set; }

        [Required]
        public required long CategoryId { get; set; }

        [Required]
        public required string Name { get; set; } = null!;

        [ForeignKey("CategoryId")]
        public CategoryModel Category { get; set; } = null!;
    }
}
