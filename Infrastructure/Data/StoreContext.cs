using System.Reflection;
using System.Text.Json;
using Core.Entities;
using Infrastructure.Data.Config;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class StoreContext : DbContext
{
    public StoreContext(DbContextOptions<StoreContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<ProductBrand> ProductBrands { get; set; } = default!;
    public DbSet<ProductType> ProductTypes { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetCallingAssembly());

        if (Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(decimal));

                foreach (var property in properties)
                {
                    modelBuilder.Entity(entityType.Name).Property(property.Name).HasConversion<double>();
                }
            }
        }


        var brandsData = File.ReadAllText("../Infrastructure/Data/SeedData/brands.json");
        var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
        modelBuilder.Entity<ProductBrand>().HasData(brands);

        var typesData = File.ReadAllText("../Infrastructure/Data/SeedData/types.json");
        var types = JsonSerializer.Deserialize<List<ProductType>>(typesData);
        modelBuilder.Entity<ProductType>().HasData(types);

        var productsData = File.ReadAllText("../Infrastructure/Data/SeedData/products.json");
        var products = JsonSerializer.Deserialize<List<Product>>(productsData);

        int idProduct = 0;
        foreach (var product in products)
            product.Id = ++idProduct;

        modelBuilder.Entity<Product>().HasData(products);
    }
}
