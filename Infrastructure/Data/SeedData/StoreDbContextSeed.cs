using System;
using System.Text.Json;

using Core.Entities;

namespace Infrastructure.Data.SeedData;

public static class StoreDbContextSeed
{
    public static async Task SeedAsync(StoreDbContext dbContext)
    {
        if (dbContext.Products.Any())
            return;

        var content = await File.ReadAllTextAsync("../Infrastructure/Data/SeedData/products.json");

        var products = JsonSerializer.Deserialize<List<Product>>(content);

        if (products is null) return;

        dbContext.Products.AddRange(products);

        await dbContext.SaveChangesAsync();
    }

}
