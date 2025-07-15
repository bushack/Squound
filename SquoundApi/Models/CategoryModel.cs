using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SquoundApi.Models
{
    public class CategoryModel
    {
        /* Constructor */
        public CategoryModel()
        {
            Products = new List<ProductModel>();
        }

        /* Primary Key, Identifier */
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required]
        public required string Name { get; set; }

        public ICollection<ProductModel> Products { get; set; }
    }
}
