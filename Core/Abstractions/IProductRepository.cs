using System;

using Core.Entities;

namespace Core.Abstractions;

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllAsync(string? brand, string? type, string? sort, CancellationToken cancellationToken = default);

    Task<Product?> FindById(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> GetBrandsAsync(CancellationToken cancellationToken = default);

    Task<IReadOnlyList<string>> GetTypesAsync(CancellationToken cancellationToken = default);

    void Add(Product product);

    void Update(Product product);

    void Delete(Product product);

    bool Exists(int id);

    Task<bool> SaveChangesAsync();
}
