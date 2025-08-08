using Microsoft.EntityFrameworkCore;

using SquoundApi.Models;


namespace SquoundApi.Data
{
    public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
    {
        // Map the internal data models to the database tables.

        public DbSet<ItemModel> Items { get; set; }
        public DbSet<ItemImageModel> ItemImages { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<SubcategoryModel> Subcategories { get; set; }
    }
}
