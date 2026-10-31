using System;
using System.Security.Cryptography.X509Certificates;

using Core.Abstractions;
using Core.Entities;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public sealed class ProductRepository(StoreDbContext dbContext) : IProductRepository
{
    public void Add(Product product)
    {
        dbContext.Products.Add(product);
    }

    public void Delete(Product product)
    {
        dbContext.Remove(product);
    }

    public bool Exists(int id)
    {
        return dbContext.Products.Any(p => p.Id == id);
    }

    public async Task<IReadOnlyList<Product>> GetAllAsync(string? brand, string? type, string? sort, CancellationToken cancellationToken)
    {
        var query = dbContext.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(brand))
            query = query.Where(p => p.Brand == brand);

        if (!string.IsNullOrWhiteSpace(type))
            query = query.Where(p => p.Type == type);

        query = sort switch
        {
            "priceAsc" => query.OrderBy(p => p.Price),
            "priceDesc" => query.OrderByDescending(p => p.Price),
            _ => query.OrderBy(x => x.Name)
        };

        var products = await query.ToListAsync();
        return products;
    }

    public async Task<IReadOnlyList<string>> GetBrandsAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Products
              .Select(p => p.Brand)
              .Distinct()
              .ToListAsync();
    }

    public async Task<IReadOnlyList<string>> GetTypesAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Products
              .Select(p => p.Type)
              .Distinct()
              .ToListAsync();
    }

    public async Task<Product?> FindById(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Products.FindAsync(id, cancellationToken);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await dbContext.SaveChangesAsync() >= 0;
    }

    public void Update(Product product)
    {
        dbContext.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
    }
}
