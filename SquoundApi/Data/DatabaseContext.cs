using Microsoft.EntityFrameworkCore;

using SquoundApi.Models;


namespace SquoundApi.Data
{
    public class DatabaseContext(DbContextOptions<DatabaseContext> options) : DbContext(options)
    {
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<CategoryModel> Categories { get; set; }
    }
}
