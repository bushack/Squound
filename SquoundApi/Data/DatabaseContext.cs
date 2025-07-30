using Microsoft.EntityFrameworkCore;

using SquoundApi.Models;


namespace SquoundApi.Data
{
    public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
    {
        // Map the internal data models to the database tables.

        public DbSet<ProductModel> Products { get; set; }
        public DbSet<ProductImageModel> ProductImages { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
    }
}
