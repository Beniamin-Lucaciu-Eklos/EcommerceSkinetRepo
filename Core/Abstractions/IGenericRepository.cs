using System;

using Core.Entities;

namespace Core.Abstractions;

public interface IGenericRepository<TEntity>
    where TEntity : BaseEntity
{
    Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<TEntity?> GetEntityWithSpec(ISpecification<TEntity> specification);
    Task<IReadOnlyList<TEntity>> GetAllWithSpec(ISpecification<TEntity> specification);

    Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<TEntity, TResult> specification);
    Task<IReadOnlyList<TResult>> GetAllWithSpec<TResult>(ISpecification<TEntity, TResult> specification);

    void Add(TEntity entity);

    void Update(TEntity entity);

    void Remove(TEntity entity);

    Task<bool> SaveAllAsync();

    bool Exists(int id);
}
