using API.RequestHelpers;

using Core.Abstractions;
using Core.Entities;
using Core.Specifications;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ProductsController(IGenericRepository<Product> productRepository) : BaseApiController
{
    [HttpGet]
    public async Task<ActionResult<Pagination<Product>>> GetProducts([FromQuery] ProductSpecParams productSpecParams)
    {
        var spec = new ProductSpecification(productSpecParams);

        return await CreatePagedResult(
            productRepository,
            spec,
            productSpecParams.PageIndex,
            productSpecParams.PageSize);
    }

    [HttpGet("brands")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetBrands()
    {
        var spec = new BrandListSpecification();

        return Ok(await productRepository.GetAllWithSpec(spec));
    }

    [HttpGet("types")]
    public async Task<ActionResult<IReadOnlyList<string>>> GetTypes()
    {
        var spec = new TypeListSpecification();

        return Ok(await productRepository.GetAllWithSpec(spec));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProduct(int id)
    {
        var product = await productRepository.GetByIdAsync(id);

        if (product is null)
            return NotFound();

        return product;
    }

    [HttpPost]
    public async Task<ActionResult<Product>> CreateProduct(Product product)
    {
        productRepository.Add(product);

        if (await productRepository.SaveAllAsync())
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);

        return BadRequest("Problem creating product");
    }


    [HttpPut("{id:int}")]
    public async Task<ActionResult> UpdateProduct(int id, Product product)
    {
        if (product.Id != id || !ProductExists(id))
            return BadRequest("Cannot update this product");

        productRepository.Update(product);

        if (await productRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Problem updateting product");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteProduct(int id)
    {
        var product = await productRepository.GetByIdAsync(id);

        if (product is null) return NotFound();

        productRepository.Remove(product);

        if (await productRepository.SaveAllAsync())
            return NoContent();

        return BadRequest("Problem deleting product");
    }

    private bool ProductExists(int id)
    {
        return productRepository.Exists(id);
    }
}
