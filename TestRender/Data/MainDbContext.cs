using Microsoft.EntityFrameworkCore;
using TestRender.Models.Entities;

namespace TestRender.Data;

public class MainDbContext : DbContext
{
    public MainDbContext(DbContextOptions contextOptions) : base(contextOptions)
    {

    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
          new Category { Id = 1, Name = "Electronics" },
          new Category { Id = 2, Name = "Clothing" },
          new Category { Id = 3, Name = "Food" }
        );
    }
}
