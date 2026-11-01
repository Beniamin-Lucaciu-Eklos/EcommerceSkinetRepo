using System;

using API.RequestHelpers;

using Core.Abstractions;
using Core.Entities;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class BaseApiController : ControllerBase
{
    protected async Task<ActionResult> CreatePagedResult<TEntity>(
        IGenericRepository<TEntity> repository,
        ISpecification<TEntity> spec,
        int pageIndex,
        int pageSize) where TEntity : BaseEntity
    {
        var items = await repository.GetAllWithSpec(spec);

        var count = await repository.CountAsync(spec);

        var pagination = new Pagination<TEntity>(
            pageIndex,
            pageSize,
            count,
            items);

        return Ok(pagination);
    }

}
