using System;

using Core.Abstractions;
using Core.Entities;

using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class GenericRepository<TEntity>(StoreDbContext dbContext)
    : IGenericRepository<TEntity> where TEntity : BaseEntity
{
    public void Add(TEntity entity)
    {
        dbContext.Set<TEntity>().Add(entity);
    }

    public bool Exists(int id)
    {
        return dbContext.Set<TEntity>().Any(entity => entity.Id == id);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<TEntity>().ToListAsync(cancellationToken);
    }

    public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Set<TEntity>().FindAsync(id, cancellationToken);
    }

    public async Task<TEntity?> GetEntityWithSpec(ISpecification<TEntity> specification)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<TEntity>> GetAllWithSpec(ISpecification<TEntity> specification)
    {
        return await ApplySpecification(specification).ToListAsync();
    }

    public async Task<TResult?> GetEntityWithSpec<TResult>(ISpecification<TEntity, TResult> specification)
    {
        return await ApplySpecification(specification).FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<TResult>> GetAllWithSpec<TResult>(ISpecification<TEntity, TResult> specification)
    {
        return await ApplySpecification(specification).ToListAsync();
    }

    private IQueryable<TEntity> ApplySpecification(ISpecification<TEntity> specification)
    {
        var query = dbContext.Set<TEntity>().AsQueryable();

        return SpecificationEvaluator<TEntity>.GetQuery(query, specification);
    }

    private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<TEntity, TResult> specification)
    {
        var query = dbContext.Set<TEntity>().AsQueryable();

        return SpecificationEvaluator<TEntity>.GetQuery<TEntity, TResult>(query, specification);
    }

    public async Task<int> CountAsync(ISpecification<TEntity> specification, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Set<TEntity>().AsQueryable();
        
        query  = specification.ApplyCriteria(query);

        return await query.CountAsync(cancellationToken);
    }

    public void Remove(TEntity entity)
    {
        dbContext.Set<TEntity>().Remove(entity);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await dbContext.SaveChangesAsync() >= 0;
    }

    public void Update(TEntity entity)
    {
        dbContext.Set<TEntity>().Attach(entity);
        dbContext.Entry(entity).State = EntityState.Modified;
    }
}
