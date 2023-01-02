using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ProductsDbContext : DbContext
    {
        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            optionsBuilder.UseInMemoryDatabase(databaseName: "ProductsDb");
        }*/
        public DbSet<Product> Products { get; set; }
        public ProductsDbContext(DbContextOptions options): base(options)
        {

        }
    }
}
