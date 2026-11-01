using System;
using System.Linq.Expressions;

namespace Core.Abstractions;

public interface ISpecification<TEntity>
{
    Expression<Func<TEntity, bool>>? Criteria { get; }

    Expression<Func<TEntity, object>>? OrderBy { get; }

    Expression<Func<TEntity, object>>? OrderByDescending { get; }

    bool IsDistinct { get; }

    int Take { get; }
    int Skip { get; }

    bool IsPagingEnabled { get; }

    IQueryable<TEntity> ApplyCriteria(IQueryable<TEntity> query);
}

public interface ISpecification<TEntity, TResult> : ISpecification<TEntity>
{
    Expression<Func<TEntity, TResult>>? Select { get; }
}